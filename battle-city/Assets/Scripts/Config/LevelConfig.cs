using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LevelConfig
{
    private static LevelConfig m_Instance = null;
    private Dictionary<string, string> m_Record = null;   // <level名字, level路径>

    public LevelConfig()
    {
        m_Record = new Dictionary<string, string>();
    }

    public static LevelConfig Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new LevelConfig();
            return m_Instance;
        }
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/LevelPathConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/LevelPathConfig"));
            return;
        }

        m_Record.Clear();

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement xmlElement in list)
        {
            AnalyzeLevelLabel(xmlElement);
        }

    }

    private void AnalyzeLevelLabel(XmlElement xmlElement)
    {
        if (!string.IsNullOrEmpty(xmlElement.Attributes["name"].Value) &&
            !string.IsNullOrEmpty(xmlElement.Attributes["path"].Value))
        {
            string name =xmlElement.Attributes["name"].Value;
            if (!m_Record.ContainsKey(name))
                m_Record.Add(name, xmlElement.Attributes["path"].Value);
        }
    }

    public Dictionary<string, string> GetRecord()
    {
        if (m_Record != null)
            return m_Record;
        else
            return null;
    }
}
