using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UIConfig
{
    private static UIConfig m_Instance = null;
    private Dictionary<UIType, string> m_Record = null;   // <UIType, UIÂ·¾¶>

    public UIConfig()
    {
        m_Record = new Dictionary<UIType, string>();
    }

    public static UIConfig Instance
    {
        get 
        {
            if (null == m_Instance)
                m_Instance = new UIConfig();
            return m_Instance;
        }
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/UIPathConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/UIPathConfig"));
            return;
        }

        m_Record.Clear();

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement xmlElement in list)
        {
            AnalyzeUILabel(xmlElement);
        }

    }

    private void AnalyzeUILabel(XmlElement xmlElement)
    {
        if (!string.IsNullOrEmpty(xmlElement.Attributes["type"].Value) &&
            !string.IsNullOrEmpty(xmlElement.Attributes["path"].Value))
        {
            UIType uIType = (UIType)int.Parse(xmlElement.Attributes["type"].Value);
            if (!m_Record.ContainsKey(uIType))
                m_Record.Add(uIType, xmlElement.Attributes["path"].Value);
        }
    }

    public Dictionary<UIType, string> GetRecord()
    {
        if (m_Record != null)
            return m_Record;
        else
            return null;
    }
}
