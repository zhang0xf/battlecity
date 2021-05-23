using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum PlayerState : int
{
    NONE = -1,
    BASIC = 1,
    LONG_CANON = 2,
    BIG_CANNON = 3,
    ULTIMATE = 4
}

public class PlayerConfigData
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float SheildTime { set; get; }
    public float Cooling { set; get; }
    public PlayerState State { set; get; }
}

public class PlayerConfig
{
    private PlayerConfigData data = null;
    private Dictionary<PlayerState, PlayerConfigData> dict = null;
    private static PlayerConfig mInstance = null;

    private PlayerConfig()
    {
        data = new PlayerConfigData();
        dict = new Dictionary<PlayerState, PlayerConfigData>();
    }

    public static PlayerConfig Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new PlayerConfig();
            return mInstance;
        }
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/PlayerConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/PlayerConfig"));
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
            AnalyzePlayerLabel(element);
            PlayerState state = GetPlayerID(element);
            if (dict.ContainsKey(state))
            {
                dict.Remove(state);
                Debug.Log(string.Format("find conflict in xml : {0} repeated", data.Form));
            }
            dict.Add(state, data);
        }
    }

    private void AnalyzePlayerLabel(XmlElement node)
    {
        XmlNodeList list = node.ChildNodes;
        foreach (XmlElement element in list)
        {
            AnalyzeFormLabel(element);
            AnalyzeSpeedLabel(element);
            AnalyzeHealthLabel(element);
            AnalyzeSheildTimeLabel(element);
            AnalyzeCoolingLabel(element);
        }
    }

    private PlayerState GetPlayerID(XmlNode node)
    {
        if (!string.IsNullOrEmpty(node.Attributes["id"].Value))
            return (PlayerState)int.Parse(node.Attributes["id"].Value);
        return PlayerState.NONE;
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

    private void AnalyzeSheildTimeLabel(XmlElement node)
    {
        if (node.Name.Equals("SHEILD_TIME") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            data.SheildTime = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeCoolingLabel(XmlElement node)
    {
        if (node.Name.Equals("COOLING") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            data.Cooling = float.Parse(node.Attributes["value"].Value);
    }
}