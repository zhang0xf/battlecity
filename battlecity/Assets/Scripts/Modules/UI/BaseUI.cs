using UnityEngine;

// UI�ձ���Ϊ�����ڻ����м̳У�OnLoad()��OnPause()��OnResume()��OnRelease()��
// UI������Ϊʹ����Ϣ���Ƕ�̬�Խ������

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

    // UI����
    protected virtual void OnLoad()
    {
        UIState = ObjectState.LOADING;
    }

    // UI��ͣ
    public virtual void OnPause()
    {
        UIState = ObjectState.INVALID;
    }

    // UI��ͣ�ָ�
    public virtual void OnResume()
    {
        UIState = ObjectState.READY;
    }

    // UIж��
    public virtual void OnRelease()
    {
        UIState = ObjectState.RELEASING;
    }
}
