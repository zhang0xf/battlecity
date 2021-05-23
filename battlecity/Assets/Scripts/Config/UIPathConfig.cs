using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UIPathConfig
{
    private static UIPathConfig mInstance = null;
    private Dictionary<UIType, string> UIPath = null;   // <UIType, UIÂ·¾¶>

    public UIPathConfig()
    {
        UIPath = new Dictionary<UIType, string>();
    }

    public static UIPathConfig Instance
    {
        get 
        {
            if (null == mInstance)
                mInstance = new UIPathConfig();
            return mInstance;
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

        UIPath.Clear();

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
            if (!UIPath.ContainsKey(uIType))
                UIPath.Add(uIType, xmlElement.Attributes["path"].Value);
        }
    }

    public string GetPathByUIType(UIType uIType)
    {
        if (!UIPath.ContainsKey(uIType)) { return null; }
        return UIPath[uIType];
    }
}
