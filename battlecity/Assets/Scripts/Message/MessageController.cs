using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageController
{
    /* -简化方法- */
    // 1. 将委托~=函数指针。
    private static MessageController mInstance = null;
    private Dictionary<string, List<MessageControlHandler>> dict = null;

    /* -复杂方法- */
    // 1. 支持配置文件的层叠化。
    // 2. 基于一条消息进行多内容处理。
    private Dictionary<string, Type> dictType = null;
    private Dictionary<string, BaseHandler> dictHandler = null;

    private MessageController() 
    {
        dict = new Dictionary<string, List<MessageControlHandler>>();
        dictType = new Dictionary<string, Type>();
        dictHandler = new Dictionary<string, BaseHandler>();

        MessageHandlerCollection(new TargetHandler());
    }

    public static MessageController Instance 
    {
        get 
        {
            if (mInstance == null)
                mInstance = new MessageController();
            return mInstance;
        }
    }

    public void MessageHandlerCollection<T>(T t)
    {
        RegisterType(t.GetType().Name, t.GetType());
    }

    public void AddNotification(Notification notify, MessageControlHandler handler)
    {
        List<MessageControlHandler> list = null;
        if (!dict.ContainsKey(notify.Name))
        {
            list = new List<MessageControlHandler>();
            list.Add(handler);
            dict.Add(notify.Name, list);
        }
        else
        {
            list.Add(handler);
        }
    }

    public void RemoveNotification(Notification notify, MessageControlHandler handler)
    {
        List<MessageControlHandler> list = null;
        if (dict.ContainsKey(notify.Name)) 
        {
            list = dict[notify.Name];
            if(list.Contains(handler))
                list.Remove(handler);
        }

        if (list.Count <= 0)
        {
            dict.Remove(notify.Name);
        }
    }

    public void SendNotification(Notification notify)
    {
        if (null == notify || !dict.ContainsKey(notify.Name))
            return;

        List<MessageControlHandler> list = dict[notify.Name];

        foreach (MessageControlHandler handler in list)
        {
            handler(notify);    // 直接调用委托，而非通过触发事件的订阅。
        }
    }

    public void RegisterType(string typeName, Type type)
    {
        if (!dictType.ContainsKey(typeName))
            dictType.Add(typeName, type);
    }

    public Type GetTypeByName(string typeName)
    {
        if (dictType == null || !dictType.ContainsKey(typeName)) return null;
        return dictType[typeName];
    }

    public void AddHandler(string notifyName, BaseHandler handler)
    {
        if (dictHandler.ContainsKey(notifyName))
        {
            dictHandler.Remove(notifyName);
            dictHandler.Add(notifyName, handler);
        }
        else
        {
            dictHandler.Add(notifyName, handler);
        }
    }

    public BaseHandler GetHandler(string notifyName)
    {
        if (null == dictHandler || !dictHandler.ContainsKey(notifyName)) return null;
        return dictHandler[notifyName];
    }

    public void PrintDictionary(Dictionary<string, BaseHandler> dict)
    {
        foreach (string key in dict.Keys)
            Debug.Log(string.Format("key : {0}, value : {1}", key, dict[key]));
    }

    public void AddSubscriber(Notification notify, MessageControlHandler handler)
    {
        BaseHandler baseHandler = GetHandler(notify.Name);
        if (null == baseHandler) { return; }
        baseHandler.AddSubscriber(handler);
    }

    public void RemoveSubscriber(Notification notify, MessageControlHandler handler)
    {
        BaseHandler baseHandler = GetHandler(notify.Name);
        if (null == baseHandler) { return;  }
        baseHandler.RemoveSubscriber(handler);
    }

    public void ActiveEventHandler(Notification notify)
    {
        BaseHandler baseHandler = GetHandler(notify.Name);
        if (null == baseHandler) { return; }
        baseHandler.Execute(notify);
    }
}
