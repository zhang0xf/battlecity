using UnityEngine;

public class Command
{
    // 设计模式：命令模式 + 多态
    public virtual void OnExcute(Tank tank) { }
    public virtual void OnExcute(BaseUI baseUI) { }
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
    public override void OnExcute(BaseUI baseUI)
    {
        Debug.Log(string.Format("enter UISelectUp : OnExcute()"));
        baseUI.SelectUP();
        base.OnExcute(baseUI);
    }
}

public class UISelectDown : Command
{
    public override void OnExcute(BaseUI baseUI)
    {
        baseUI.SelectDown();
        base.OnExcute(baseUI);
    }
}

public class UISelectLeft : Command
{
    public override void OnExcute(BaseUI baseUI)
    {
        baseUI.SelectLeft();
        base.OnExcute(baseUI);
    }
}

public class UISelectRight : Command
{
    public override void OnExcute(BaseUI baseUI)
    {
        baseUI.SelectRight();
        base.OnExcute(baseUI);
    }
}

public class UIMouseMove : Command
{
    public override void OnExcute(BaseUI baseUI)
    {
        baseUI.MouseMove();
        base.OnExcute(baseUI);
    }
}