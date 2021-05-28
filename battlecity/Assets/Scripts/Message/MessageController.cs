using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageController
{
    // 将委托 ~= 函数指针（简化）
    private static MessageController mInstance = null;
    private Dictionary<string, List<MessageControlHandler>> dict = null;

    private MessageController() 
    {
        dict = new Dictionary<string, List<MessageControlHandler>>();
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

    public void RemoveNotification(string notifyName, MessageControlHandler handler)
    {
        List<MessageControlHandler> list = null;
        if (dict.ContainsKey(notifyName)) 
        {
            list = dict[notifyName];
            if(list.Contains(handler))
                list.Remove(handler);
        }

        if (list.Count <= 0)
        {
            // 不删除list。SendNotification在执行foreach()时list不能为空！
            // dict.Remove(notifyName);
        }
    }

    public void SendNotification(Notification notify)
    {
        if (null == notify || !dict.ContainsKey(notify.Name))
            return;

        List<MessageControlHandler> list = dict[notify.Name];

        foreach (MessageControlHandler handler in list)
        {
            handler(notify);    // 将委托用作函数指针。
        }
    }
}