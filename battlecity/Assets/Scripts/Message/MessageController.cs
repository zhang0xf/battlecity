using System.Collections.Generic;

public class MessageController
{
    // 将委托 ~= 函数指针（简化）
    private static MessageController mInstance = null;
    private Dictionary<string, List<MessageControlHandler>> dict = null;    // 消息管理
    private Queue<KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>> queue = null;    // 消息添加和删除队列
    
    private static bool isSending = false;
    private const string ADD_NOTIFICATION = "add_notification";
    private const string REMOVE_NOTIFICATION = "remove_notification";

    private MessageController() 
    {
        dict = new Dictionary<string, List<MessageControlHandler>>();
        queue = new Queue<KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>>();
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
        if (isSending)
        {
            KeyValuePair<string, MessageControlHandler> kv = new KeyValuePair<string, MessageControlHandler>(notifyName, handler);
            queue.Enqueue(new KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>(ADD_NOTIFICATION, kv));
        }
        else
        {
            if (!dict.ContainsKey(notifyName))
            {
                List<MessageControlHandler> list = new List<MessageControlHandler>();
                list.Add(handler);
                dict.Add(notifyName, list);
            }
            else
            {
                List<MessageControlHandler> list = dict[notifyName];
                if (!list.Contains(handler))
                    list.Add(handler);
            }
        }
    }

    public void RemoveNotification(string notifyName, MessageControlHandler handler)
    {
        if (isSending)
        {
            KeyValuePair<string, MessageControlHandler> kv = new KeyValuePair<string, MessageControlHandler>(notifyName, handler);
            queue.Enqueue(new KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>(REMOVE_NOTIFICATION, kv));
        }
        else
        {
            if (dict.ContainsKey(notifyName))
            {
                List<MessageControlHandler> list = dict[notifyName];
                if (list.Contains(handler))
                    list.Remove(handler);

                if (list.Count == 0)
                    dict.Remove(notifyName);
            }
        }
    }

    public void SendNotification(Notification notify)
    {
        if (null == notify || !dict.ContainsKey(notify.Name))
            return;

        List<MessageControlHandler> list = dict[notify.Name];
       
        // when foreach list, add or remove is forbidden.
        foreach (MessageControlHandler handler in list)
        {
            isSending = true;
            if (handler != null)
                handler(notify);    // 将委托用作函数指针。
        }

        isSending = false;

        if (queue.Count == 0) { return; }

        SyncMessageList();
    }

    private void SyncMessageList()
    {
        KeyValuePair<string, KeyValuePair<string, MessageControlHandler>> element = queue.Dequeue();

        KeyValuePair<string, MessageControlHandler> message = element.Value;

        if (element.Key.Equals(ADD_NOTIFICATION))
        {
            AddNotification(message.Key, message.Value);
        }
        else if (element.Key.Equals(REMOVE_NOTIFICATION))
        {
            RemoveNotification(message.Key, message.Value);
        }
    }
}