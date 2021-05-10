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

    public void LoadConfig(string filePath)
    {
        if (null == filePath) { return; }

        XmlDocument Xdoc = Config.LoadXmlConfig(filePath);
        if (null == Xdoc)
        {
            Debug.LogError(string.Format("load error : filepath[{0}]", filePath));
            return;
        }

        XmlNode Node = Xdoc.FirstChild;
        Node = Node.NextSibling;
        if (!Node.HasChildNodes) { return; }

        XmlNodeList list = Node.ChildNodes;
        foreach (XmlNode element in list)
        {
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
