using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[Serializable]
public class ColorPalette : ScriptableObject
{
    #if UNITY_EDITOR
    [MenuItem("Assets/Create/Color Palette")]
    public static void Create()
    {
        if (Selection.activeObject is Texture2D)
        {
            Texture2D selectedTexture = Selection.activeObject as Texture2D;
            string selectionPath = AssetDatabase.GetAssetPath(selectedTexture);
            selectionPath = selectionPath.Replace(".png", "-color-palette.asset");
            Debug.Log("Creating a Palette " + selectionPath);

            ColorPalette newPalette = CreateAssetFromPath<ColorPalette>(selectionPath);

            newPalette.sourceTexture = selectedTexture;
            newPalette.Reset(); 
        }
        else
        {
            Debug.LogError("Can only create a palette for a Texture2D file");
        }
    }
    
    static T CreateAssetFromPath<T>(string path) where T : ScriptableObject
    {
        T asset = null;

        asset = ScriptableObject.CreateInstance<T>();

        string newPath = AssetDatabase.GenerateUniqueAssetPath(path);

        AssetDatabase.CreateAsset(asset, newPath);

        AssetDatabase.SaveAssets();

        return asset;
    }
    #endif

    public Texture2D sourceTexture;
    public List<Color> palette = new List<Color>();
    public List<Color> newPalette = new List<Color>();

    private Texture2D cachedTexture = null;
    public Texture2D CachedTexture
    {
        get { return cachedTexture; }
        set { cachedTexture = value; }
    }

    private List<Color> BuildPalette(Texture2D texture)
    {
        List<Color> palette = new List<Color>();

        Color[] pixels = texture.GetPixels();

        foreach (var color in pixels)
        {
            if (!palette.Contains(color))
            {
                if (color.a == 1)
                {
                    palette.Add(color);
                }
            }
        }

        return palette;
    }

    public void Reset()
    {
        palette = BuildPalette(sourceTexture);
        newPalette = new List<Color>(palette);
    }

    public Color GetColor(Color color)
    {
        for (int i = 0; i < palette.Count; i++)
        {
            Color c = palette[i];

            if (Mathf.Approximately(color.r, c.r) &&
                Mathf.Approximately(color.g, c.g) &&
                Mathf.Approximately(color.b, c.b) &&
                Mathf.Approximately(color.a, c.a))
            {
                return newPalette[i];
            }
        }

        return color;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor
{
    public ColorPalette colorPalette;

    private void OnEnable()
    {
        colorPalette = target as ColorPalette;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Source Texture");
        colorPalette.sourceTexture = EditorGUILayout.ObjectField(colorPalette.sourceTexture, typeof(Texture2D), false) as Texture2D;


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Current Color");
        GUILayout.Label("New Color");
        EditorGUILayout.EndHorizontal();

        
        for (int i = 0; i < colorPalette.palette.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.ColorField(colorPalette.palette[i]);

            colorPalette.newPalette[i] = EditorGUILayout.ColorField(colorPalette.newPalette[i]);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Revert Palette"))
        {
            colorPalette.Reset();
        }

        EditorUtility.SetDirty(colorPalette);
    }
}
#endif