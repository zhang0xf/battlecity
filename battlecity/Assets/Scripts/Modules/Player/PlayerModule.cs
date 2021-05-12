using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : BaseObject
{
    public PlayerModule()
    {
        RegisterState = ObjectRegisterState.NEED;   // 加入 ModuleManager
    }

    protected override void Loading()
    {


        base.Loading(); // 显示调用基类
    }
}
