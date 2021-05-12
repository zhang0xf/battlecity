using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject
{
    private ObjectState mState;
    public event StateChangedHandler StateHandler;
    private ObjectRegisterState mRegisterState;  // private : 可被子类继承但不可被子类访问。访问方法：RegisterState(public)

    public BaseObject()
    {
        mState = ObjectState.INITIAL;
        mRegisterState = ObjectRegisterState.NONE;
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

    public ObjectRegisterState RegisterState
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
        Loading();
    }

    protected virtual void Loading()
    {
        // 多态
        State = ObjectState.READY;
    }

    public void Release()
    { 
        
    }

    public void Releasing()
    { 
        
    }
    
    private void RegisterModule(BaseObject obj)
    {
        if (RegisterState == ObjectRegisterState.NEED)
        {
            ModuleManager.Instance.RegisterModule(obj);    // The this keyword refers to the current instance of the class.
            RegisterState = ObjectRegisterState.DONE;
        }
    }
}
