using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    public Player()
    {
        RegState = RegisterState.NEED;   // 加入 ModuleManager
    }

    public override void MoveDown()
    {
        base.MoveDown();
    }

    public override void MoveLeft()
    {
        base.MoveLeft();
    }

    public override void MoveUp()
    {
        base.MoveUp();
    }

    public override void MoveRight()
    {
        base.MoveRight();
    }

    protected override void OnLoad()
    {
        // 监听消息

        base.OnLoad();
    }



}
