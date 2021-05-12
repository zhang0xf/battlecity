using UnityEngine;

public class TargetHandler : BaseHandler
{
    public new event MessageControlHandler EventHandler = null;

    public override void AddSubscriber(MessageControlHandler handler)
    {
        EventHandler += handler;
        Debug.Log("TargetHandler AddSubscriber");
    }

    public override void RemoveSubscriber(MessageControlHandler handler)
    {
        EventHandler -= handler;
    }

    public override void Execute(Notification notify)
    {
        EventHandler(notify);
        base.Execute(notify);
        Debug.Log("TargetHandler execute");
    }

}
