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

    protected virtual void OnLoad()
    {
        State = ObjectState.LOADING;
    }

    public virtual void SelectUP() { }
    
    public virtual void SelectDown() { }
    
    public virtual void SelectLeft() { }
    
    public virtual void SelectRight() { }
}
