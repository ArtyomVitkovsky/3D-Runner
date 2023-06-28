using System;
using UnityEngine;
using System.Reflection;

public interface IDataResolver
{
    bool Resolve(string data, out string resolvedData);
}

public interface IPlayerPrefsData
{
    void Save();
    void Dispose();
}

[Serializable]
public class PlayerPrefsData<T> : IPlayerPrefsData where T : new()
{
    [SerializeField]
    private string m_Version;

    [SerializeField]
    private T m_Value;

    private string m_Key;
    public event Action<T> OnSaved;
    public event Action<T> OnChanged;
    private IDataResolver m_DataResolver;

    public PlayerPrefsData(string key, IDataResolver dataResolver)
    {
        m_Key = key;
        m_DataResolver = dataResolver;

        var s = PlayerPrefs.GetString(m_Key, "");
        if (m_DataResolver != null &&
            m_DataResolver.Resolve(s, out string rS))
        {
            Debug.Log($"Resolved from: {s} to: {rS}");
            s = rS;
        }
        if (string.IsNullOrEmpty(s)) m_Value = new T();
        else m_Value = JsonUtility.FromJson<PlayerPrefsData<T>>(s).m_Value;
    }

    public PlayerPrefsData(string key, T defaultValue = default)
    {
        m_Key = key;
        var s = PlayerPrefs.GetString(m_Key, "");
        if (string.IsNullOrEmpty(s)) m_Value = defaultValue;
        else m_Value = JsonUtility.FromJson<PlayerPrefsData<T>>(s).m_Value;
    }

    public T Value
    {
        get { return m_Value; }
        set
        {
            var valueChanged = !m_Value.Equals(value);
            m_Value = value;
            if (valueChanged)
            {
                OnChanged?.Invoke(m_Value);
                Save();
            }
        }
    }

    public string Version => m_Version;

    public static implicit operator T(PlayerPrefsData<T> d) => d.Value;

    public void Save()
    {
        m_Version = Application.version;
        PlayerPrefs.SetString(m_Key, JsonUtility.ToJson(this));
        OnSaved?.Invoke(m_Value);
        // OnChanged?.Invoke(m_Value);
    }

    public void Dispose()
    {
        m_Value = default(T);
        PlayerPrefs.SetString(m_Key, "");
    }
}

public class PlayerData : MonoBehaviour
{
    public void Initialize()
    {
        Setup();
        SaveAll();
    }

    // ----------------Data Variables-------------------------

    public PlayerPrefsData<Settings> settings;
    
    
    [Serializable]
    public class Settings
    {
        public float soundVolume = 0.5f;
    }


    // -------------------------------------------------------

    public void SaveAll()
    {
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var value = field.GetValue(this) as IPlayerPrefsData;
            if (value != null) value.Save();
        }
    }

    public void ClearAll()
    {
        //PlayerPrefs.DeleteAll();
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var value = field.GetValue(this) as IPlayerPrefsData;
            if (value != null) value.Dispose();
        }
    }

    private void Setup()
    {
        settings = new PlayerPrefsData<Settings>("settings", new Settings());
    }
}

#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomEditor(typeof(PlayerData))]
    public class PlayerDataButtons : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlayerData myScript = (PlayerData)target;
            if (GUILayout.Button("Save all"))
            {
                myScript.SaveAll();
            }
            else if (GUILayout.Button("Clear all"))
            {
                myScript.ClearAll();
            }
        }
    }
}

#endif