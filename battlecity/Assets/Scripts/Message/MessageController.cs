using System.Collections.Generic;

public class MessageController
{
    // 将委托 ~= 函数指针（简化）
    private static MessageController m_Instance = null;
    private Dictionary<string, List<MessageControlHandler>> m_Record = null;    // 消息管理
    private Queue<KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>> m_Queue = null;    // 消息添加和删除队列
    
    private static bool isSending = false;
    private const string ADD_NOTIFICATION = "add_notification";
    private const string REMOVE_NOTIFICATION = "remove_notification";

    private MessageController() 
    {
        m_Record = new Dictionary<string, List<MessageControlHandler>>();
        m_Queue = new Queue<KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>>();
    }

    public static MessageController Instance 
    {
        get 
        {
            if (m_Instance == null)
                m_Instance = new MessageController();
            return m_Instance;
        }
    }

    public void AddNotification(string notifyName, MessageControlHandler handler)
    {
        if (isSending)
        {
            KeyValuePair<string, MessageControlHandler> kv = new KeyValuePair<string, MessageControlHandler>(notifyName, handler);
            m_Queue.Enqueue(new KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>(ADD_NOTIFICATION, kv));
        }
        else
        {
            if (!m_Record.ContainsKey(notifyName))
            {
                List<MessageControlHandler> list = new List<MessageControlHandler>();
                list.Add(handler);
                m_Record.Add(notifyName, list);
            }
            else
            {
                List<MessageControlHandler> list = m_Record[notifyName];
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
            m_Queue.Enqueue(new KeyValuePair<string, KeyValuePair<string, MessageControlHandler>>(REMOVE_NOTIFICATION, kv));
        }
        else
        {
            if (m_Record.ContainsKey(notifyName))
            {
                List<MessageControlHandler> list = m_Record[notifyName];
                if (list.Contains(handler))
                    list.Remove(handler);

                if (list.Count == 0)
                    m_Record.Remove(notifyName);
            }
        }
    }

    public void SendNotification(Notification notify)
    {
        if (null == notify || !m_Record.ContainsKey(notify.Name))
            return;

        List<MessageControlHandler> list = m_Record[notify.Name];
       
        // when foreach list, add or remove is forbidden.
        foreach (MessageControlHandler handler in list)
        {
            isSending = true;
            if (handler != null)
                handler(notify);    // 将委托用作函数指针。
        }

        isSending = false;

        if (m_Queue.Count == 0) { return; }

        SyncMessageList();
    }

    private void SyncMessageList()
    {
        KeyValuePair<string, KeyValuePair<string, MessageControlHandler>> element = m_Queue.Dequeue();

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