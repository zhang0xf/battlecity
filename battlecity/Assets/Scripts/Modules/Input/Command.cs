public class Command
{
    // 设计模式：命令模式
    // 思路：按键绑定命令，命令绑定逻辑。

    public virtual void OnExcute(Tank tank) { }
    public virtual void OnExcute(GameState state) 
    {
        StateMachine.Instance.ChangeState(state);
    }
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

public class UIComfirm : Command { }

public class UIBack : Command { }

public class UISelectUp : Command { }

public class UISelectDown : Command { }

public class UISelectLeft : Command { }

public class UISelectRight : Command { }