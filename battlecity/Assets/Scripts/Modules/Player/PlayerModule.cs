using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModule : BaseObject
{
    public PlayerModule()
    {
        RegisterState = ObjectRegisterState.NEED;   // ���� ModuleManager
    }

    protected override void Loading()
    {


        base.Loading(); // ��ʾ���û���
    }
}
