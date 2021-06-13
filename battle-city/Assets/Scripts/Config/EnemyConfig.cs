using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyKind
{ 
    public string Form { set; get; }
    public float Speed { set; get; }
    public int Health { set; get; }
    public float Cooling { set; get; }
}

public class EnemyConfig
{
    private EnemyKind m_Kind = null;
    private static EnemyConfig m_Instance = null;
    private Dictionary<int, EnemyKind> m_Record = null;

    private EnemyConfig()
    {
        m_Record = new Dictionary<int, EnemyKind>();
    }

    public static EnemyConfig Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new EnemyConfig();
            return m_Instance;
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

        m_Record.Clear();

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;
        foreach (XmlElement element in list)
        {
            if (!element.HasChildNodes) continue;
            m_Kind = new EnemyKind();
            AnalyzeEnemyLabel(element);
            int id = GetEnemyID(element);
            if (m_Record.ContainsKey(id))
            {
                m_Record.Remove(id);
                Debug.Log(string.Format("find conflict in xml : {0} repeated", m_Kind.Form));
            }
            m_Record.Add(id, m_Kind);
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
            m_Kind.Form = node.Attributes["value"].Value;
    }

    private void AnalyzeSpeedLabel(XmlElement node)
    {
        if (node.Name.Equals("SPEED") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Kind.Speed = float.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeHealthLabel(XmlElement node)
    {
        if (node.Name.Equals("HEALTH") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("int"))
            m_Kind.Health = int.Parse(node.Attributes["value"].Value);
    }

    private void AnalyzeCoolingLabel(XmlElement node)
    {
        if (node.Name.Equals("COOLING") &&
            !string.IsNullOrEmpty(node.Attributes["value"].Value) &&
            node.Attributes["value_type"].Value.Equals("float"))
            m_Kind.Cooling = float.Parse(node.Attributes["value"].Value);
    }
}