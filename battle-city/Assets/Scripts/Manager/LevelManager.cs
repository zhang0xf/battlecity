using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    private static LevelManager m_Instance = null;
    private Dictionary<string, string> m_Path = null;   // 预制体路径
    private Dictionary<string, GameObject> m_Resource = null;   // 加载完成的关卡

    private LevelManager()
    {
        m_Path = LevelConfig.Instance.GetRecord();
        m_Resource = new Dictionary<string, GameObject>();
    }

    public static LevelManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new LevelManager();
            return m_Instance;
        }
    }

    public void PreLoadResources()
    {
        if (m_Path != null && m_Resource != null)
        {
            foreach (KeyValuePair<string, string> kv in m_Path)
            {
                GameObject obj = Resources.Load(kv.Value) as GameObject;    // 尚未Instantiate()
                if (null == obj) { continue; }
                if (!m_Resource.ContainsKey(kv.Key))
                {
                    m_Resource.Add(kv.Key, obj);
                }
            }
        }
    }

    public GameObject LoadLevel(string name)
    {
        if (null == name) { return null; }

        GameObject obj = null;

        if (!m_Resource.ContainsKey(name))
        {
            obj = Resources.Load(m_Path[name]) as GameObject;
            if (null == obj)
            {
                Debug.LogError("can not find prefab");
                return null;
            }
        }
        else 
        {
            obj = m_Resource[name];
        }

        obj = Object.Instantiate(obj);

        return obj;
    }
}
