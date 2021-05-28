using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private GameObject mLastSelectObject = null;
    private GameObject mCurrentSelectObject = null;
    private ObjectState mUILastState = ObjectState.NONE;
    private ObjectState mUICurrState = ObjectState.NONE;
    private event StateChangedHandler handler = null; // UI状态改变自动同步到UIManager

    public GameObject LastSelectObject
    {
        get { return mLastSelectObject; }
        set { mLastSelectObject = value; }
    }

    public GameObject CurrentSelectObject
    {
        get { return mCurrentSelectObject; }
        set { mCurrentSelectObject = value; }
    }

    public ObjectState UICurrState
    {
        get { return mUICurrState; }
        set 
        {
            if (mUICurrState != value)
            {
                mUILastState = mUICurrState;
                mUICurrState = value;
                if (handler != null)
                {
                    handler(gameObject, mUICurrState, mUILastState);
                }
            }
        }
    }

    void Awake()
    {
        handler += HandleStateChange;
        UICurrState = ObjectState.INITIAL;
        OnLoad();
    }

    // UI加载
    protected virtual void OnLoad()
    {
        UICurrState = ObjectState.LOADING;
    }

    // UI暂停
    public virtual void OnPause()
    {
        UICurrState = ObjectState.INVALID;
    }

    // UI暂停恢复
    public virtual void OnResume()
    {
        UICurrState = ObjectState.READY;
    }

    // UI卸载
    public virtual void OnRelease()
    {
        UICurrState = ObjectState.RELEASING;
        handler -= HandleStateChange;
    }

    protected void HandleStateChange(object sender, ObjectState newState, ObjectState oldState)
    {
        // Debug.Log("enter HandleStateChange()");
        UIManager.Instance.SetUIState(sender, newState);
    }
}
