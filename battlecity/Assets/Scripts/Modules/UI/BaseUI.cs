using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected ObjectState mState;

    public  ObjectState State
    {
        get { return mState; }
        set { mState = value; }
    }

    private void Awake()
    {
        State = ObjectState.INITIAL;
        OnLoad();
    }

    // UIº”‘ÿ
    protected virtual void OnLoad()
    {
        State = ObjectState.LOADING;
    }

    // UI‘›Õ£
    public virtual void OnPause()
    {
        State = ObjectState.INVALID;
    }

    // UI‘›Õ£ª÷∏¥
    public virtual void OnResume()
    {
        State = ObjectState.READY;
    }

    // UI–∂‘ÿ
    public virtual void OnRelease()
    {
        State = ObjectState.RELEASING;
    }

    public virtual void SelectUP() { }
    
    public virtual void SelectDown() { }
    
    public virtual void SelectLeft() { }
    
    public virtual void SelectRight() { }

    public virtual void MouseMove() { }

    public virtual void Comfirm() { }
}
