using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : BaseHandler
{
    public new event MessageControlHandler EventHandler = null;

    public override void AddSubscriber(MessageControlHandler handler)
    {
        Debug.Log("TargetHandler");
        this.EventHandler += handler;
    }

    public override void RemoveSubscriber(MessageControlHandler handler)
    {
        this.EventHandler -= handler;
    }

    public override void Execute(Notification notify)
    {
        Debug.Log("TargetHandler execute");
        this.EventHandler(notify);
        base.Execute(notify);
    }

}
