using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController
{
    private static MessageController mInstance = null;
    private Dictionary<string, List<MessageControlHandler>> dict = null;    // event
    // private event MessageControlHandler sample;

    private MessageController() 
    {
        dict = new Dictionary<string, List<MessageControlHandler>>();
    }

    public static MessageController Instance 
    {
        get {
            if (mInstance == null)
            {
                mInstance = new MessageController();
            }
            return mInstance;
        }
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
            handler(notify);
        }
    }
}
