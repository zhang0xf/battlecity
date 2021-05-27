using UnityEngine;

// UI普遍行为可以于基类中继承：OnLoad()、OnPause()、OnResume()、OnRelease()。
// UI特征行为使用消息而非多态以降低耦合

public class BaseUI : MonoBehaviour
{
    protected ObjectState mUIState;
    protected GameObject mLastSelectObject = null;
    protected GameObject mCurrentSelectObject = null;

    public ObjectState UIState
    {
        get { return mUIState; }
        set { mUIState = value; }
    }

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

    private void Awake()
    {
        UIState = ObjectState.INITIAL;
        OnLoad();
    }

    // UI加载
    protected virtual void OnLoad()
    {
        UIState = ObjectState.LOADING;
    }

    // UI暂停
    public virtual void OnPause()
    {
        UIState = ObjectState.INVALID;
    }

    // UI暂停恢复
    public virtual void OnResume()
    {
        UIState = ObjectState.READY;
    }

    // UI卸载
    public virtual void OnRelease()
    {
        UIState = ObjectState.RELEASING;
    }
}
