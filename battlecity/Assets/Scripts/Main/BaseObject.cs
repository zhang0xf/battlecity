using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject
{
    private ObjectState mState;
    public event StateChangedHandler StateHandler;
    private RegisterState mRegisterState;  // private : 不可被子类访问，通过转换为父类引用访问！

    public BaseObject()
    {
        mState = ObjectState.INITIAL;
        mRegisterState = RegisterState.NONE;
    }

    public ObjectState State
    {
        get { return mState; }

        set
        {
            if (mState == value)
            {
                ObjectState oldState = mState;
                mState = value;
                if (StateHandler != null)
                    StateHandler(this, oldState, mState);
            }
        }
    }

    public RegisterState RegState
    {
        get { return mRegisterState; }
        set
        {
            if (mRegisterState.GetType() == value.GetType())
                mRegisterState = value;
            else
                Debug.LogError("Type Error : RegisterState = value");
        }
    }

    public void Load()
    {
        if (State != ObjectState.INITIAL) { return; }
        State = ObjectState.LOADING;
        RegisterModule(this);
        OnLoad();
    }

    protected virtual void OnLoad()
    {
        State = ObjectState.READY;  // 多态
    }

    public void Release()
    { 
        
    }
    
    private void RegisterModule(BaseObject obj)
    {
        if (RegState == RegisterState.NEED)
        {
            ModuleManager.Instance.RegisterModule(obj);    // The this keyword refers to the current instance of the class.
            RegState = RegisterState.DONE;
        }
    }
}
