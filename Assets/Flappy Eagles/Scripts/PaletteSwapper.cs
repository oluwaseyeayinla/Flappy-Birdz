using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class PaletteSwapper : MonoBehaviour
{
    public enum SwapRenderer
    {
        SpriteRenderer,
        Image,
        RawImage
    }

    public SwapRenderer swapRenderer = SwapRenderer.SpriteRenderer;
    public ColorPalette[] colorPalettes = { };
    public bool randomiseOnStart = true;

    private SpriteRenderer spriteRenderer = null;
    private RawImage rawImage = null;
    private Image image;
    
    private Texture2D texture = null;
    private MaterialPropertyBlock block = null;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        rawImage = GetComponent<RawImage>();
    }

    void Start()
    {
        if (randomiseOnStart)
        {
            if (colorPalettes != null && colorPalettes.Length > 0)
            {
                SwapColors(colorPalettes[Random.Range(0, colorPalettes.Length)]);
            }
        }
    }

    public void SwapColors(ColorPalette palette)
    {
        SwapColors(palette, null);
    }

    public void SwapColors(ColorPalette palette, Action callback)
    {
        if(palette.CachedTexture == null)
        {
            if (swapRenderer == SwapRenderer.RawImage)
            {
                texture = rawImage.texture as Texture2D;
            }
            else if (swapRenderer == SwapRenderer.SpriteRenderer)
            {
                texture = spriteRenderer.sprite.texture;
            }
            else
            {
                texture = image.sprite.texture;
            }
            
            Texture2D cloneTexture = new Texture2D(texture.width, texture.height);
            cloneTexture.wrapMode = TextureWrapMode.Clamp;
            cloneTexture.filterMode = FilterMode.Point;

            Color[] colors = texture.GetPixels();

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = palette.GetColor(colors[i]);
            }

            cloneTexture.SetPixels(colors);
            cloneTexture.Apply();

            palette.CachedTexture = cloneTexture;
        }

        
        if (swapRenderer == SwapRenderer.RawImage)
        {
            rawImage.texture = palette.CachedTexture as Texture;
        } else if (swapRenderer == SwapRenderer.Image)
        {
            int w = palette.CachedTexture.width;
            int h = palette.CachedTexture.height;
            image.sprite = Sprite.Create(palette.CachedTexture, new Rect(0, 0, w, h), new Vector2(.5f, .5f));
        } else if (swapRenderer == SwapRenderer.SpriteRenderer)
        {
            block = new MaterialPropertyBlock();
            block.SetTexture("_MainTex", palette.CachedTexture);
        }

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    void LateUpdate()
    {
        if(block != null && swapRenderer == SwapRenderer.SpriteRenderer)
        {
            spriteRenderer.SetPropertyBlock(block);
        }
    }
}


[CustomEditor(typeof(PaletteSwapper))]
public class PaletteSwapperEditor : Editor
{

    public PaletteSwapper paletteSwapper;

    private void OnEnable()
    {
        paletteSwapper = target as PaletteSwapper;
    }

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }
}
