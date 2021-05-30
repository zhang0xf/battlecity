using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private GameObject mLastSelectObject = null;
    private GameObject mCurrentSelectObject = null;
    private ObjectState mUILastState = ObjectState.NONE;
    private ObjectState mUICurrState = ObjectState.NONE;
    private event StateChangedHandler handler = null; // UI״̬�ı��Զ�ͬ����UIManager

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

    // UI����
    protected virtual void OnLoad()
    {
        UICurrState = ObjectState.LOADING;
    }

    // UI��ͣ
    public virtual void OnPause()
    {
        UICurrState = ObjectState.INVALID;
    }

    // UI��ͣ�ָ�
    public virtual void OnResume()
    {
        UICurrState = ObjectState.READY;
    }

    // UIж��
    public virtual void OnRelease()
    {
        UICurrState = ObjectState.RELEASING;
        handler -= HandleStateChange;
    }

    public virtual void RecvSelectUP(Notification notify) { }

    public virtual void RecvSelectDown(Notification notify) { }

    public virtual void RecvSelectLeft(Notification notify) { }

    public virtual void RecvSelectRight(Notification notify) { }

    public virtual void RecvFire(Notification notify) { }

    protected void HandleStateChange(object sender, ObjectState newState, ObjectState oldState)
    {
        UIManager.Instance.SetUIState(sender, newState);
    }
}
