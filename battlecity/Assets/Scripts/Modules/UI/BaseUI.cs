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

    // UI����
    protected virtual void OnLoad()
    {
        State = ObjectState.LOADING;
    }

    // UI��ͣ
    public virtual void OnPause()
    {
        State = ObjectState.INVALID;
    }

    // UI��ͣ�ָ�
    public virtual void OnResume()
    {
        State = ObjectState.READY;
    }

    // UIж��
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
