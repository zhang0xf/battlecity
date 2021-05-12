public class BaseHandler
{
    private string name = string.Empty;
   
#pragma warning disable 414
    public event MessageControlHandler EventHandler = null;
    // warning CS0414: The field 'BaseHandler.eventHandler' is assigned but its value is never used
#pragma warning disable 414

    public string Name
    {
        set { name = value; }
        get { return name; }
    }

    public virtual void AddSubscriber(MessageControlHandler handler)
    {
        EventHandler += handler;
    }

    public virtual void RemoveSubscriber(MessageControlHandler handler)
    {
        EventHandler -= handler;
    }

    public virtual void Execute(Notification notify)
    {
        // Base Class Do Nothing !
    }
}
