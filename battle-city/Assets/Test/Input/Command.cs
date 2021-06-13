using System.Collections.Generic;
using UnityEngine;

// 命令模式：按键绑定命令、命令绑定逻辑
// 废弃原因：unity的输入系统很完善！

public class Command 
{
    private List<KeyCode> list = null;
    private KeyCode keyCode;

    public Command()
    {
        list = new List<KeyCode>();
    }

    public virtual void OnExcute(KeyCode keyCode) { }

    public void BindKey(KeyCode keyCode)
    {
        if (list != null && !list.Contains(keyCode))
            list.Add(keyCode);
    }

    public KeyCode GetKeyCode()
    {
        return keyCode;
    }

    public bool IsMatch(KeyCode keyCode)
    {
        this.keyCode = keyCode;
        return list.Contains(keyCode);
    }
}

public class FireCommand : Command 
{
    public override void OnExcute(KeyCode keyCode)
    {
        Notification na = new Notification(NotificationName.FIRE, this);
        na.Content = keyCode;
        na.Send();
    }
}

public class BackCommand : Command 
{
    public override void OnExcute(KeyCode keyCode)
    {
        Notification na = new Notification(NotificationName.BACK, this);
        na.Content = keyCode;
        na.Send();
    }
}

public class UpCommand : Command 
{
    public override void OnExcute(KeyCode keyCode)
    {
        Notification na = new Notification(NotificationName.SELECT_UP, this);
        na.Content = keyCode;
        na.Send();
    }
}

public class DownCommand : Command
{
    public override void OnExcute(KeyCode keyCode)
    {
        Notification na = new Notification(NotificationName.SELECT_DOWN, this);
        na.Content = keyCode;
        na.Send();
    }
}

public class LeftCommand : Command 
{
    public override void OnExcute(KeyCode keyCode)
    {
        Notification na = new Notification(NotificationName.SELECT_LEFT, this);
        na.Content = keyCode;
        na.Send();
    }
}

public class RightCommand : Command
{
    public override void OnExcute(KeyCode keyCode)
    {
        Notification na = new Notification(NotificationName.SELECT_RIGHT, this);
        na.Content = keyCode;
        na.Send();
    }
}