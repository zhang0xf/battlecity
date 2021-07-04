using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TankInfo
{
    public int ID { set; get; }
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float SheildTime { set; get; }
    public float Cooling { set; get; }
}

public class TankConfig
{
    private TankInfo m_Info = null;
    private Dictionary<int, TankInfo> m_Player = null;
    private Dictionary<int, TankInfo> m_Enemy = null;
    private static TankConfig m_Instance = null;

    private TankConfig()
    {
        m_Player = new Dictionary<int, TankInfo>();
        m_Enemy = new Dictionary<int, TankInfo>();
    }

    public static TankConfig Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new TankConfig();
            return m_Instance;
        }
    }

    public TankInfo GetPlayerInfo(int level)
    {
        if (m_Player.ContainsKey(level))
            return m_Player[level];
        return null;
    }

    public TankInfo GetEnemyInfo(int kind)
    {
        if (m_Enemy.ContainsKey(kind))
            return m_Enemy[kind];
        return null;
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/TankConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/TankConfig"));
            return;
        }

        m_Player.Clear();
        m_Enemy.Clear();

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement element in list)
        {
            if (!element.HasChildNodes) continue;

            m_Info = new TankInfo();

            if (element.Name.Equals("player"))
            {
                AnalyzePlayerLabel(element);
                int id = GetPlayerID(element);
                if (m_Player.ContainsKey(id))
                {
                    m_Player.Remove(id);
                    Debug.Log(string.Format("find conflict in xml : {0} repeated", m_Info.Form));
                }
                m_Player.Add(id, m_Info);
            }
            else if(element.Name.Equals("enemy"))
            {
                AnalyzeEnemyLabel(element);
                int id = GetEnemyID(element);
                if (m_Enemy.ContainsKey(id))
                {
                    m_Enemy.Remove(id);
                    Debug.Log(string.Format("find conflict in xml : {0} repeated", m_Info.Form));
                }
                m_Enemy.Add(id, m_Info);
            }
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

    private int GetEnemyID(XmlNode node)
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
            m_Info.Form = node.Attributes["value"].Value;
    }

    private void AnalyzeSpeedLabel(XmlElement node)
    {
        if (node.Name.Equals("SPEED") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Info.Speed = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeHealthLabel(XmlElement node)
    {
        if (node.Name.Equals("HEALTH") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("int"))
            m_Info.Health = int.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeSheildTimeLabel(XmlElement node)
    {
        if (node.Name.Equals("SHEILD_TIME") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Info.SheildTime = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeCoolingLabel(XmlElement node)
    {
        if (node.Name.Equals("COOLING") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Info.Cooling = float.Parse(node.Attributes["value"].Value);
    }
}