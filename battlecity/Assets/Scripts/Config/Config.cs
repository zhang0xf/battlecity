using System.Xml;
using UnityEngine;

public static class Config
{
    public static void LoadConfig()
    {
        UIPathConfig.Instance.LoadConfig();
        PlayerConfig.Instance.LoadConfig();
        EnemyConfig.Instance.LoadConfig();
    }

    public static XmlDocument LoadXmlConfig(string FilePath)
    {
        if (null == FilePath) { return null; }

        TextAsset textAsset = Resources.Load(FilePath) as TextAsset;
        if (textAsset)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(textAsset.text); // doc.LoadXml("<item><name>wrench</name></item>");
            return xdoc;
        }
        return null;
    }
}