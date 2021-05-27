public class Command
{
    // 设计模式：命令模式
    // 思路：按键绑定命令，命令绑定逻辑。

    // 使用消息降低三个模块的耦合度：
    // 1. Command ：只负责处理输入，不依赖于GUI脚本。
    // 2. GUI ： 只负责接受消息并处理UI显示，不更改游戏状态。
    // 3. StateMachine ： 只负责游戏流程。
    public virtual void OnExcute(Tank tank) { }
    public virtual void OnExcute() { }
}

public class TankMoveUp : Command
{
    public override void OnExcute(Tank tank)
    {
        tank.MoveUp();
        base.OnExcute(tank);
    }
}

public class TankMoveDown : Command
{
    public override void OnExcute(Tank tank)
    {
        tank.MoveDown();
        base.OnExcute(tank);
    }
}

public class TankMoveLeft : Command
{
    public override void OnExcute(Tank tank)
    {
        tank.MoveLeft();
        base.OnExcute(tank);
    }
}

public class TankMoveRight : Command
{
    public override void OnExcute(Tank tank)
    {
        tank.MoveRight();
        base.OnExcute(tank);
    }
}

public class UISelectUp : Command
{
    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.UI_SELECTED_UP, this);
        na.Send();
        base.OnExcute();
    }
}

public class UISelectDown : Command
{
    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.UI_SELECTED_DOWN, this);
        na.Send();
        base.OnExcute();
    }
}

public class UISelectLeft : Command
{
    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.UI_SELECTED_LEFT, this);
        na.Send();
        base.OnExcute();
    }
}

public class UISelectRight : Command
{
    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.UI_SELECTED_RIGHT, this);
        na.Send();
        base.OnExcute();
    }
}

public class UIMouseMove : Command
{
    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.MOUSE_MOVE, this);
        na.Send();
        base.OnExcute();
    }
}

public class UIComfirm : Command
{
    public override void OnExcute()
    {
        Notification na = new Notification(NotificationName.UI_COMFIRM, this);
        na.Send();
        base.OnExcute();
    }
}