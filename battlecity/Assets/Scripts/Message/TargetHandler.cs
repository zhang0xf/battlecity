using UnityEngine;

public class TargetHandler : BaseHandler
{
    public new event MessageControlHandler EventHandler = null;

    public override void AddSubscriber(MessageControlHandler handler)
    {
        Debug.Log("TargetHandler AddSubscriber");
        EventHandler += handler;
    }

    public override void RemoveSubscriber(MessageControlHandler handler)
    {
        EventHandler -= handler;
    }

    public override void Execute(Notification notify)
    {
        Debug.Log("TargetHandler execute");
        EventHandler(notify);
        base.Execute(notify);
    }
}
