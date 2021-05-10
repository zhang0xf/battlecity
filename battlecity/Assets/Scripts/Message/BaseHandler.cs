using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHandler
{
    private string name = string.Empty;
   
#pragma warning disable 414
    public event MessageControlHandler eventHandler = null;
#pragma warning disable 414 // CS0414

    //warning CS0414: The field 'BaseHandler.eventHandler' is assigned but its value is never used

    public string Name
    {
        set { name = value; }
        get { return name; }
    }

    public virtual void AddSubscriber(MessageControlHandler handler)
    {
        this.eventHandler += handler;
    }

    public virtual void RemoveSubscriber(MessageControlHandler handler)
    {
        this.eventHandler -= handler;
    }

    public virtual void Execute(Notification notify)
    {

    }
}
