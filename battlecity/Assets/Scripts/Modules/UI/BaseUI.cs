using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUI : MonoBehaviour
{
    private GameObject mLastSelectObject = null;
    private GameObject mCurrentSelectObject = null;
    private ObjectState mUILastState = ObjectState.NONE;
    private ObjectState mUICurrState = ObjectState.NONE;
    private event StateChangedHandler handler = null; // UI状态改变自动同步到UIManager

    protected Dictionary<GameObject, GameObject> dictUp = null;  // <当前对象, 上面位置的对象>
    protected Dictionary<GameObject, GameObject> dictDown = null;// <当前对象，下面位置的对象>

    public BaseUI()
    {
        dictUp = new Dictionary<GameObject, GameObject>();
        dictDown = new Dictionary<GameObject, GameObject>();
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
        Destroy(gameObject, 0.0f);  // 延迟0秒
    }

    public virtual void RecvFire(Notification notify) { }

    public virtual void RecvBack(Notification notify) { }

    public virtual void RecvSelectUP(Notification notify) { }

    public virtual void RecvSelectDown(Notification notify) { }

    public virtual void RecvSelectLeft(Notification notify) { }

    public virtual void RecvSelectRight(Notification notify) { }

    public virtual void RecvMouseMove(Notification notify) { }

    public virtual void RecvMouseLeft(Notification notify) { }

    public virtual void RecvPointerEnter(Notification notify) { }

    protected void HandleStateChange(object sender, ObjectState newState, ObjectState oldState)
    {
        UIManager.Instance.SetUIState(sender, newState);
    }

    public void MakeUILogicConnections(GameObject up, GameObject down)
    {
        if (null == up || null == down) { return; }

        if (!dictDown.ContainsKey(up))
            dictDown.Add(up, down);
        if (!dictUp.ContainsKey(down))
            dictUp.Add(down, up);
    }

    public void MakeUILogicConnections(GameObject[] array)
    {
        for (int idx = 0; idx < array.Length - 1; idx++)
        {
            GameObject up = array[idx];
            GameObject down = array[idx + 1];

            MakeUILogicConnections(up, down);
        }
    }

    public GameObject GetUpConnection(GameObject obj)
    {
        if (dictUp.ContainsKey(obj.gameObject))
            return dictUp[obj.gameObject];
        return null;
    }

    public GameObject GetDownConnection(GameObject obj)
    {
        if (dictDown.ContainsKey(obj.gameObject))
            return dictDown[obj.gameObject];
        return null;
    }

    public GameObject GetPointerObject(GameObject obj)
    {
        PointerData pointerData = obj.GetComponent<PointerData>();
        if (null == pointerData)
            return ExecuteEvents.GetEventHandler<IPointerEnterHandler>(obj);

        return obj;
    }
}
