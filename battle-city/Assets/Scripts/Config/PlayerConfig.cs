using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerStatus
{
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float SheildTime { set; get; }
    public float Cooling { set; get; }
}

public class PlayerConfig
{
    private PlayerStatus m_Status = null;
    private Dictionary<int, PlayerStatus> m_Record = null;
    private static PlayerConfig m_Instance = null;

    private PlayerConfig()
    {
        m_Record = new Dictionary<int, PlayerStatus>();
    }

    public static PlayerConfig Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new PlayerConfig();
            return m_Instance;
        }
    }

    public PlayerStatus GetPlayerStatus(int id)
    {
        if (m_Record.ContainsKey(id))
            return m_Record[id];
        else
            return null;
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/PlayerConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/PlayerConfig"));
            return;
        }

        m_Record.Clear();

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement element in list)
        {
            if (!element.HasChildNodes) continue;
            m_Status = new PlayerStatus();
            AnalyzePlayerLabel(element);
            int id = GetPlayerID(element);
            if (m_Record.ContainsKey(id))
            {
                m_Record.Remove(id);
                Debug.Log(string.Format("find conflict in xml : {0} repeated", m_Status.Form));
            }
            m_Record.Add(id, m_Status);
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

    private int GetPlayerID(XmlNode node)
    {
        if (!string.IsNullOrEmpty(node.Attributes["id"].Value))
            return int.Parse(node.Attributes["id"].Value);
        return -1;
    }

    private void AnalyzeFormLabel(XmlElement node)
    {
        if (node.Name.Equals("FORM") &&
            node.Attributes["value_type"].Value.Equals("string") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value))
            m_Status.Form = node.Attributes["value"].Value;
    }

    private void AnalyzeSpeedLabel(XmlElement node)
    {
        if (node.Name.Equals("SPEED") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Status.Speed = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeHealthLabel(XmlElement node)
    {
        if (node.Name.Equals("HEALTH") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("int"))
            m_Status.Health = int.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeSheildTimeLabel(XmlElement node)
    {
        if (node.Name.Equals("SHEILD_TIME") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Status.SheildTime = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeCoolingLabel(XmlElement node)
    {
        if (node.Name.Equals("COOLING") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Status.Cooling = float.Parse(node.Attributes["value"].Value);
    }
}