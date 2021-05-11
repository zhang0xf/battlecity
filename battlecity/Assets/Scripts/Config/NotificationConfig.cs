using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class NotificationConfig
{
    // data > MessageController

    private static NotificationConfig mInstance = null;

    private NotificationConfig()
    {
        
    }

    public static NotificationConfig Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new NotificationConfig();
            return mInstance;
        }
    }

    public void LoadConfig()
    {
        XmlDocument xdoc = Config.LoadXmlConfig("Config/NotificationConfig");
        if (null == xdoc)
        {
            Debug.LogError(string.Format("load error : Config/NotificationConfig"));
            return;
        }

        XmlNode node = xdoc.FirstChild;
        node = node.NextSibling;
        if (!node.HasChildNodes) { return; }

        XmlNodeList list = node.ChildNodes;
        foreach (XmlNode element in list)
        {
            if (!element.HasChildNodes) continue;
            AnalyzeMsgLabel(element);
        }
    }

    public void AnalyzeMsgLabel(XmlNode node)
    {
        string msgName = node.Attributes["name"].Value;
        if (!node.HasChildNodes) { return; }

        XmlNodeList list = node.ChildNodes;
        foreach (XmlNode element in list)
        {
            BaseHandler handler =  AnalyzeHandlerLabel(element);
            MessageController.Instance.AddHandler(msgName, handler);
        }
    }

    public BaseHandler AnalyzeHandlerLabel(XmlNode node)
    {
        string className = node.Attributes["class"].Value;
        Type classType = MessageController.Instance.GetTypeByName(className);
        if (null == classType)
        {
            Debug.LogError(string.Format("class : {0}, not register yet !", className));
            return null;
        }
        return StaticFunction.CreateObjectByType<BaseHandler>(classType);
    }
}
