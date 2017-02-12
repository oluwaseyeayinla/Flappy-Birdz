using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameData 
{
    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// </summary>
    public static bool LoadBoolValue(string key, bool default_value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            bool flag = value == 0 ? false : true;
            return flag;
        }

        return default_value;
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists. Note: Always returns the default 'false' if key does not exist
    /// </summary>
    public static bool LoadBoolValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            bool flag = value == 0 ? false : true;
            return flag;
        }

        return false;
    }

    /// <summary>
    /// Sets the value of the preference identified by key
    /// </summary>
    public static void SaveBoolValue(string key, bool flag)
    {
        int value = flag ? 1 : 0;
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// </summary>
    public static int LoadIntValue(string key, int default_value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            return value;
        }

        return default_value;
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists. Note: Always returns the default 'false' if key does not exist
    /// </summary>
    public static int LoadIntValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            return value;
        }

        return int.MinValue;
    }

    /// <summary>
    /// Sets the value of the preference identified by key
    /// </summary>
    public static void SaveIntValue(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// </summary>
    public static float LoadFloatValue(string key, float default_value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            float value = PlayerPrefs.GetFloat(key);
            return value;
        }

        return default_value;
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists. Note: Always returns the default 'false' if key does not exist
    /// </summary>
    public static float LoadFloatValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int value = PlayerPrefs.GetInt(key);
            return value;
        }

        return float.MinValue;
    }

    /// <summary>
    /// Sets the value of the preference identified by key
    /// </summary>
    public static void SaveFloatValue(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists.
    /// </summary>
    public static string LoadStringValue(string key, string default_value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string value = PlayerPrefs.GetString(key);
            return value;
        }

        return default_value;
    }

    /// <summary>
    /// Returns the value corresponding to key in the preference file if it exists. Note: Always returns the default 'false' if key does not exist
    /// </summary>
    public static string LoadStringValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string value = PlayerPrefs.GetString(key);
            return value;
        }

        return string.Empty;
    }

    /// <summary>
    /// Sets the value of the preference identified by key
    /// </summary>
    public static void SaveStringValue(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

	public static void SaveBinaryData(string file_name, object data)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(Path.Combine(Application.persistentDataPath, file_name));
		binaryFormatter.Serialize(file, data);
		file.Close();
	}
	
	public static void LoadBinaryData(string file_name, out object data)
	{
        if (File.Exists(Path.Combine(Application.persistentDataPath, file_name)))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, file_name), FileMode.Open);
            data = binaryFormatter.Deserialize(file);
            file.Close();
        }
        else
        {
            data = null;
        }
	}
	
    public static void SaveFreshInstall(bool flag)
    {
        SaveBoolValue(Application.productName, flag);
    }

    public static bool LoadFreshInstall()
    {
        return LoadBoolValue(Application.productName, false);
    }

    public static void Erase(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
