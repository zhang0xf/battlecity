using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    private GameObject m_Level; // 唯一Level
    private static LevelManager m_Instance = null;
    private Dictionary<string, string> m_Path = null;   // 路径
    private Dictionary<string, GameObject> m_Resource = null;   // 加载完成的Level

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
}
