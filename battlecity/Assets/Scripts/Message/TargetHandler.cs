using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : BaseHandler
{
    public new event MessageControlHandler eventHandler = null;

    public override void AddSubscriber(MessageControlHandler handler)
    {
        this.eventHandler += handler;
        Debug.Log("TargetHandler add");
    }

    public override void RemoveSubscriber(MessageControlHandler handler)
    {
        this.eventHandler -= handler;
    }

    public override void Execute(Notification notify)
    {
        Debug.Log("TargetHandler execute");
        this.eventHandler(notify);
        base.Execute(notify);
    }

}
