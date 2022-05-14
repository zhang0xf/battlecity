using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public UIType m_UIType;
    private ObjState m_CurrState;
    private event StateChangedHandler handler = null;

    public ObjState CurrState
    {
        get { return m_CurrState; }
        set
        {
            if (m_CurrState != value)
            {
                ObjState m_LastState = m_CurrState;
                m_CurrState = value;
                if (handler != null)
                {
                    handler(m_UIType, m_CurrState, m_LastState);
                }
            }
        }
    }

    void Awake()
    {
        // Awake is called when the script instance is being loaded.
        // Awake is called either when an active GameObject that contains the script is initialized when a Scene loads,
        // or when a previously inactive GameObject is set to active,
        // or after a GameObject created with Object.Instantiate is initialized.
        // Use Awake to initialize variables or states before the application starts.
        CurrState = ObjState.INITIAL;
        handler += HandleStateChange;
        OnLoad();
    }

    // UI加载
    protected virtual void OnLoad()
    {
        CurrState = ObjState.LOADING;
    }

    // UI暂停
    public virtual void OnPause()
    {
        CurrState = ObjState.INVALID;
    }

    // UI暂停恢复
    public virtual void OnResume()
    {
        CurrState = ObjState.READY;
    }

    // UI卸载
    public virtual void OnRelease()
    {
        CurrState = ObjState.RELEASING;
        handler -= HandleStateChange;
        Destroy(gameObject, 0.0f);  // 延迟0秒销毁
    }

    protected void HandleStateChange(object sender, ObjState newState, ObjState oldState)
    {
        UIManager.Instance.SetUIState(sender, newState);
    }
}
