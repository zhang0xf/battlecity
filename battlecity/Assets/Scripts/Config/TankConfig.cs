using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TankConfig
{
    private static TankConfig mInstance = null;
    private PlayerData playerData = null;
    private Dictionary<int, EnemyData> dict = null;

    private TankConfig()
    {
        playerData = new PlayerData();
        dict = new Dictionary<int, EnemyData>();
    }

    public static TankConfig Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new TankConfig();
            return mInstance;
        }
    }

    public void LoadConfig(string filePath)
    {
        if (null == filePath)
        {
            Debug.LogError("can not find file!");
            return;
        }

        XmlDocument xdoc = Config.LoadXmlConfig(filePath);
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : filepath[{0}]", filePath));
            return;
        }

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;

        if (!node.HasChildNodes) { return; }
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement element in list)
        {
            if (!element.HasChildNodes) continue;
            string labelName = element.Name;
            if (labelName.Equals("Player"))
                AnalyzePlayerLabel(element);
            else if (labelName.Equals("Enemy"))
                AnalyzeEnemyLabel(element);
        }

    }

    public void AnalyzePlayerLabel(XmlElement node)
    {
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement element in list)
        {
            AnalyzeIDLabel(element, null);  // 有无改进？
            AnalyzeNameLabel(element, null);
            AnalyzeSpeedLabel(element, null);
            AnalyzeHealthLabel(element, null);
            AnalyzeSheildTimeLabel(element, null);
            AnalyzeCoolingLabel(element, null);
        }
    }

    public void AnalyzeEnemyLabel(XmlElement node)
    {
        EnemyData enemyData = new EnemyData();
        XmlNodeList list = node.ChildNodes;

        foreach (XmlElement element in list)
        {
            AnalyzeIDLabel(element, enemyData);
            AnalyzeNameLabel(element, enemyData);
            AnalyzeSpeedLabel(element, enemyData);
            AnalyzeHealthLabel(element, enemyData);
            AnalyzeStrenghtenedLabel(element, enemyData);
            AnalyzeCoolingLabel(element, enemyData);
        }

        int eID = int.Parse(node.Attributes["eid"].Value);  // Visual Assist 提示单词拼写错误，实际没毛病。
        dict.Add(eID, enemyData);
    }

    public void AnalyzeIDLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("ID")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("int"))
        { 
            if(null == data)
                playerData.ID = int.Parse(node.Attributes["value"].Value);
            else
                data.ID = int.Parse(node.Attributes["value"].Value);
        }
    }

    public void AnalyzeNameLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("NAME")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("string"))
        {
            if(null == data)
                playerData.Name = node.Attributes["value"].Value;
            else
                data.Name = node.Attributes["value"].Value;
        }
    }

    public void AnalyzeSpeedLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("SPEED")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("float"))
        { 
            if(null == data)
                playerData.Speed = float.Parse(node.Attributes["value"].Value);
            else
                data.Speed = float.Parse(node.Attributes["value"].Value);
        }
    }

    public void AnalyzeHealthLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("HEALTH")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("int"))
        { 
            if(null == data)
                playerData.Health = int.Parse(node.Attributes["value"].Value);
            else
                data.Health = int.Parse(node.Attributes["value"].Value);
        }
    }

    public void AnalyzeSheildTimeLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("SHEILD_TIME")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("float"))
            playerData.SheildTime = float.Parse(node.Attributes["value"].Value);
    }

    public void AnalyzeCoolingLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("COOLING")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("float"))
        { 
            if(null == data)
                playerData.Cooling = float.Parse(node.Attributes["value"].Value);
            else
                data.Cooling = float.Parse(node.Attributes["value"].Value);
        }
    }

    public void AnalyzeStrenghtenedLabel(XmlElement node, EnemyData data)
    {
        if (!node.Name.Equals("STRENGTHENED")) { return; }
        if (!string.IsNullOrEmpty(node.Attributes["value"].Value) && node.Attributes["value_type"].Value.Equals("bool"))
            data.Strengthened = bool.Parse(node.Attributes["value"].Value);
    }
}
