public class BaseObject : IObject
{
    public ObjState m_CurrState;
    public RegisterState m_RegisterState;

    public void Load()
    {
        m_CurrState = ObjState.LOADING;
        RegisterModule(this);
        OnLoad();
    }

    public void Release()
    {
        m_CurrState = ObjState.RELEASING;
        OnRelease();
    }

    protected virtual void OnLoad()
    {
        m_CurrState = ObjState.READY;
    }

    protected virtual void OnRelease() { }
    
    private void RegisterModule(BaseObject obj)
    {
        if (m_RegisterState == RegisterState.NEED)
        {
            ModuleManager.Instance.RegisterModule(obj);
            m_RegisterState = RegisterState.DONE;
        }
    }
}
