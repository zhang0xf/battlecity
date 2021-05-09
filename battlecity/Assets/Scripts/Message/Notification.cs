using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : IEnumerable<KeyValuePair<string, object>>
{
    private object content;    // 属性
    public object Sender { private set; get; }  // 自动属性
    public string Name { private set; get; }
    
    public Notification(string name, object obj)
    {
        Name = name;
        Sender = obj;
    }

    public object Content
    {
        get {
            return content;
        }
        set {
            content = value;
        }
    }

    public void Send()
    {
        MessageController.Instance.SendNotification(this);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
