public class BaseObject : IObject
{
    private ObjectState mCurrState = ObjectState.INITIAL;
    private RegisterState mRegState = RegisterState.NONE;
    public event StateChangedHandler Handler;

    public ObjectState CurrState
    {
        get { return mCurrState; }

        set
        {
            if (mCurrState != value)
            {
                ObjectState mLastState = mCurrState;
                mCurrState = value;
                if (Handler != null)
                    Handler(this, mLastState, mCurrState);
            }
        }
    }

    public RegisterState RegState
    {
        get { return mRegState; }
        set { mRegState = value; }
    }

    public void Load()  // 接口 + 虚函数
    {
        if (CurrState != ObjectState.INITIAL) { return; }
        Handler += HandleStateChange;
        CurrState = ObjectState.LOADING;
        RegisterModule(this);   // this : 具体对象
        OnLoad();
    }

    protected virtual void OnLoad()
    {
        CurrState = ObjectState.READY;
    }

    public void Release()
    {
        Handler -= HandleStateChange;
        OnRelease();
    }

    protected virtual void OnRelease()
    { 
        
    }
    
    private void RegisterModule(BaseObject obj)
    {
        if (RegState == RegisterState.NEED)
        {
            ModuleManager.Instance.RegisterModule(obj);
            RegState = RegisterState.DONE;
        }
    }

    private void HandleStateChange(object sender, ObjectState newState, ObjectState oldState) { }
}
