using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 使用消息降低三个模块的耦合度：
// 1. Command ：只负责处理输入，不依赖于GUI脚本。
// 2. GUI ： 只负责接受消息并处理UI显示，不更改游戏状态。
// 3. StateMachine ： 只负责游戏流程。

public class MainMenuUI : BaseUI
{
    private static int defaultIndex = 0;
    private int index = defaultIndex;
    public Button[] buttons = null; // set from inspector panel
    private EventSystem mEventSystem = null;

    // BaseUI : Awake()
    // Awake is called when the script instance is being loaded.
    // Awake is called either when an active GameObject that contains the script is initialized when a Scene loads,
    // or when a previously inactive GameObject is set to active,
    // or after a GameObject created with Object.Instantiate is initialized.
    // Use Awake to initialize variables or states before the application starts.
    protected override void OnLoad()
    {
        // Debug.Log("MianMenuUI Awake()");
        MessageController.Instance.AddNotification(NotificationName.UI_SELECTED_UP, RecvSelectUP);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECTED_DOWN, RecvSelectDown);
        MessageController.Instance.AddNotification(NotificationName.UI_COMFIRM, RecvComfirm);
        MessageController.Instance.AddNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECTED_BY_MOUSE, RecvUIButtonSelectedByMouse);

        base.OnLoad();
    }

    // This function is called when the object(script) becomes enabled and active.
    private void OnEnable()
    {
        if (buttons.Length <= 0) { Debug.LogError("buttons is null"); }

        // Fetch the current EventSystem. Make sure your Scene has one.
        mEventSystem = EventSystem.current;

        // Add Scripts for Button
        foreach (Button button in buttons)
        {
            if (null == button) continue;
            PointerData pointerData = button.gameObject.AddComponent<PointerData>();
            if (null == pointerData) { }
        }
    }

    private void Start()
    {
        if (null == mEventSystem) { return; }
        if (null == buttons[defaultIndex]) { return; }

        SetDefaultSelected();
    }

    public override void OnPause()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
            button.gameObject.transform.Find("Icon").gameObject.SetActive(false);
        }

        base.OnPause();
    }

    public override void OnResume()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }

        SetDefaultSelected();

        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECTED_UP, RecvSelectUP);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECTED_DOWN, RecvSelectDown);
        MessageController.Instance.RemoveNotification(NotificationName.UI_COMFIRM, RecvComfirm);
        MessageController.Instance.RemoveNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECTED_BY_MOUSE, RecvUIButtonSelectedByMouse);

        Debug.Log(string.Format("releasing gameobject : {0}", gameObject.name));
        Destroy(gameObject, 0.0f);  // 延迟0秒

        base.OnRelease();
    }

    private void RecvMouseMove(Notification notify)
    {
        Cursor.visible = true;
    }

    private void RecvSelectUP(Notification notify)
    {
        if (UIState == ObjectState.INVALID) { return; }
        if (UIState != ObjectState.READY) { return; }

        Cursor.visible = false;

        if (buttons.Length <= 0) { return; }
        if (index == 0) { return; }

        LastSelectObject = CurrentSelectObject;
        CurrentSelectObject = buttons[--index].gameObject;

        Game.Instance.StartCoroutine(SetButton());
    }

    private void RecvSelectDown(Notification notify)
    {
        if (UIState == ObjectState.INVALID) { return; }
        if (UIState != ObjectState.READY) { return; }

        Cursor.visible = false;

        if (buttons.Length <= 0) { return; }
        if (index == buttons.Length - 1) { return; }

        LastSelectObject = CurrentSelectObject;
        CurrentSelectObject = buttons[++index].gameObject;
        
        Game.Instance.StartCoroutine(SetButton());
    }

    private IEnumerator SetButton()
    {
        yield return new WaitForSeconds(0.0f);
        mEventSystem.SetSelectedGameObject(CurrentSelectObject);
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        LastSelectObject.transform.Find("Icon").gameObject.SetActive(false);
    }

    private void RecvUIButtonSelectedByMouse(Notification notify)
    {
        if (UIState == ObjectState.INVALID) { return; }
        if (UIState != ObjectState.READY) { return; }

        if (null == notify) { return; }

        GameObject mousePoint = null;

        GameObject obj = (GameObject)notify.Content;

        PointerData pointerData = obj.gameObject.GetComponent<PointerData>();
        if (null == pointerData)
            mousePoint = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(obj);
        else
            mousePoint = obj;

        // Debug.Log(string.Format("mousePoint object name is {0}", mousePoint.name));

        if (mousePoint == CurrentSelectObject) { return; }

        LastSelectObject = CurrentSelectObject;
        CurrentSelectObject = mousePoint;

        mEventSystem.SetSelectedGameObject(CurrentSelectObject);
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        LastSelectObject.transform.Find("Icon").gameObject.SetActive(false); // lastSelectObject 不可能为null
        
        SnycIndex(CurrentSelectObject); // 将鼠标的跳跃性选择同步给 => 键盘和手柄

    }

    private void SetDefaultSelected()
    {
        LastSelectObject = null;
        CurrentSelectObject = buttons[defaultIndex].gameObject;
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        mEventSystem.SetSelectedGameObject(CurrentSelectObject.gameObject);
    }

    private void SnycIndex(GameObject obj)
    {
        int idx = 0;
        foreach (Button button in buttons)
        {
            if (button.gameObject == obj)
            {
                index = idx;
                break;
            }
            ++idx;
        }
    }

    public bool IsOptionPlayerSelected()
    {
        if (CurrentSelectObject == buttons[0].gameObject) { return true; }
        return false;
    }

    public bool IsOptionPlayersSelected()
    {
        if (CurrentSelectObject == buttons[1].gameObject) { return true; }
        return false;
    }

    public bool IsOptionSettingSelected()
    {
        if (CurrentSelectObject == buttons[2].gameObject) { return true; }
        return false;
    }

    public bool IsOptionCustomizeSelected()
    {
        if (CurrentSelectObject == buttons[3].gameObject) { return true; }
        return false;
    }

    public bool IsOptionOnlineSelected()
    {
        if (CurrentSelectObject == buttons[4].gameObject) { return true; }
        return false;
    }

    public bool IsOptionExitSelected()
    {
        if (CurrentSelectObject == buttons[5].gameObject) { return true; }
        return false;
    }

    private void RecvComfirm(Notification notify)
    {
        OnPause();
    }
}
