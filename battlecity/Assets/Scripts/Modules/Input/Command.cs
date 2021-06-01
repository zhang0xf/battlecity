using UnityEngine;

public class Command 
{
    private KeyCode keyCode;

    public Command(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }

    public virtual void OnExcute() { }

    public KeyCode GetBindKey() { return keyCode; }
}

public class FireCommand : Command 
{
    public FireCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.FIRE, this);
        na.Send();

        base.OnExcute();
    }
}

public class BackCommand : Command 
{
    public BackCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.BACK, this);
        na.Send();

        base.OnExcute();
    }
}

public class UpCommand : Command 
{
    public UpCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.SELECT_UP, this);
        na.Send();

        base.OnExcute();
    }
}

public class DownCommand : Command
{
    public DownCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.SELECT_DOWN, this);
        na.Send();

        base.OnExcute();
    }
}

public class LeftCommand : Command 
{
    public LeftCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.SELECT_LEFT, this);
        na.Send();

        base.OnExcute();
    }
}

public class RightCommand : Command
{
    public RightCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.SELECT_RIGHT, this);
        na.Send();

        base.OnExcute();
    }
}

public class MouseLeftCommand : Command
{
    public MouseLeftCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.MOUSE_LEFT, this);
        na.Send();

        base.OnExcute();
    }
}

public class MouseMoveCommand : Command
{
    public MouseMoveCommand(KeyCode keyCode) : base(keyCode) { }

    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.MOUSE_MOVE, this);
        na.Send();

        base.OnExcute();
    }
}