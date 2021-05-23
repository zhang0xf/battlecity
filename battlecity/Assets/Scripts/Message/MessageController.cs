using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageController
{
    // 方案1：将委托 ~= 函数指针（简化）
    private static MessageController mInstance = null;
    private Dictionary<string, List<MessageControlHandler>> dict = null;

    // 方案2：事件触发（比较麻烦）
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

    public void AddNotification(string notifyName, MessageControlHandler handler)
    {
        List<MessageControlHandler> list = null;
        if (!dict.ContainsKey(notifyName))
        {
            list = new List<MessageControlHandler>();
            list.Add(handler);
            dict.Add(notifyName, list);
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
            handler(notify);    // 方案1：将委托用作函数指针。
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
            dictHandler.Remove(notifyName);
        dictHandler.Add(notifyName, handler);
    }

    public BaseHandler GetHandler(string notifyName)
    {
        if (null == dictHandler || !dictHandler.ContainsKey(notifyName)) return null;
        return dictHandler[notifyName];
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