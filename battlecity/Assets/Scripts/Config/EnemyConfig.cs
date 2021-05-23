using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum EnemyType : int
{
    NONE = -1,
    BASIC = 1,
    QUICK = 2,
    ARMOR = 3,
    BASIC_STRENGTHEN = 4,
    QUICK_STRENGTHEN = 5,
    ARMOR_STRENGTHEN = 6
}

public class EnemyConfigData
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float Cooling { set; get; }
    public EnemyType Type { set; get; }
}

public class EnemyConfig
{
    private EnemyConfigData data = null;
    private static EnemyConfig mInstance = null;
    private Dictionary<EnemyType, EnemyConfigData> dict = null;

    private EnemyConfig()
    {
        data = new EnemyConfigData();
        dict = new Dictionary<EnemyType, EnemyConfigData>();
    }

    public static EnemyConfig Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new EnemyConfig();
            return mInstance;
        }
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/EnemyConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/EnemyConfig"));
            return;
        }

        dict.Clear();

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;
        foreach (XmlElement element in list)
        {
            if (!element.HasChildNodes) continue;
            AnalyzeEnemyLabel(element);
            EnemyType type = GetEnemyID(element);
            if (dict.ContainsKey(type))
            {
                dict.Remove(type);
                Debug.Log(string.Format("find conflict in xml : {0} repeated", data.Form));
            }
            dict.Add(type, data);
        }
    }

    private void AnalyzeEnemyLabel(XmlElement node)
    {
        XmlNodeList list = node.ChildNodes;
        foreach (XmlElement element in list)
        {
            AnalyzeFormLabel(element);
            AnalyzeSpeedLabel(element);
            AnalyzeHealthLabel(element);
            AnalyzeCoolingLabel(element);
        }
    }

    private EnemyType GetEnemyID(XmlNode node)
    {
        if (!string.IsNullOrEmpty(node.Attributes["id"].Value))
            return (EnemyType)int.Parse(node.Attributes["id"].Value);
        return EnemyType.NONE;
    }

    private void AnalyzeFormLabel(XmlElement node)
    {
        if (node.Name.Equals("FORM") &&
            node.Attributes["value_type"].Value.Equals("string") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value))
            data.Form = node.Attributes["value"].Value;
    }

    private void AnalyzeSpeedLabel(XmlElement node)
    {
        if (node.Name.Equals("SPEED") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            data.Speed = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeHealthLabel(XmlElement node)
    {
        if (node.Name.Equals("HEALTH") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("int"))
            data.Health = int.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeCoolingLabel(XmlElement node)
    {
        if (node.Name.Equals("COOLING") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            data.Cooling = float.Parse(node.Attributes["value"].Value);
    }
}