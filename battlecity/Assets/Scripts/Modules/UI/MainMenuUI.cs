using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    public Button newGame = null;
    public Button continueGame = null;
    public Button setting = null;
    public Button customize = null;
    public Button online = null;
    public Button exit = null;
    public GameObject Backgroud = null;

    private EventSystem mEventSystem = null;

    protected override void OnLoad()
    {
        Debug.Log("MianMenuUI Awake()");

        // BaseUI : Awake()
        // Awake is called when the script instance is being loaded.
        // Awake is called either when an active GameObject that contains the script is initialized when a Scene loads,
        // or when a previously inactive GameObject is set to active,
        // or after a GameObject created with Object.Instantiate is initialized.
        // Use Awake to initialize variables or states before the application starts.

        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_NEWGAME_OPTION, RecvSelectNewGameOption);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_CONTINUE_OPTION, RecvSelectContinueOption);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_SETTING_OPTION, RecvSelectSettingOption);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_CUSTOMIZE_OPTION, RecvSelectCustomizeOption);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_ONLINE_OPTION, RecvSelectOnlineOption);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_EXIT_OPTION, RecvSelectExitOption);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_OPTION_BY_MOUSE, RecvSelectOptionByMouse);

        base.OnLoad();
    }

    void Start()
    {
        mEventSystem = EventSystem.current; // Fetch the current EventSystem. Make sure your Scene has one.
        ResetUI();
    }

    public override void OnPause()
    {

        base.OnPause();
    }

    public override void OnResume()
    {
        ResetUI();

        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_NEWGAME_OPTION, RecvSelectNewGameOption);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_CONTINUE_OPTION, RecvSelectContinueOption);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_SETTING_OPTION, RecvSelectSettingOption);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_CUSTOMIZE_OPTION, RecvSelectCustomizeOption);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_ONLINE_OPTION, RecvSelectOnlineOption);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_EXIT_OPTION, RecvSelectExitOption);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_OPTION_BY_MOUSE, RecvSelectOptionByMouse);

        Destroy(gameObject, 0.0f);  // 延迟0秒
        base.OnRelease();
    }

    private void ResetUI()
    {
        newGame.interactable = true;
        continueGame.interactable = true;
        setting.interactable = true;
        customize.interactable = true;
        online.interactable = true;
        exit.interactable = true;

        Cursor.visible = false;

        LastSelectObject = null;
        CurrentSelectObject = newGame.gameObject;
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        mEventSystem.SetSelectedGameObject(CurrentSelectObject);
    }

    private void RecvSelectNewGameOption(Notification notify)
    {
        SelectObject(newGame.gameObject);
    }

    private void RecvSelectContinueOption(Notification notify)
    {
        SelectObject(continueGame.gameObject);
    }

    private void RecvSelectSettingOption(Notification notify)
    {
        SelectObject(setting.gameObject);
    }

    private void RecvSelectCustomizeOption(Notification notify)
    {
        SelectObject(customize.gameObject);
    }

    private void RecvSelectOnlineOption(Notification notify)
    {
        SelectObject(online.gameObject);
    }

    private void RecvSelectExitOption(Notification notify)
    {
        SelectObject(exit.gameObject);
    }

    private void SelectObject(GameObject obj)
    {
        Cursor.visible = false;

        LastSelectObject = CurrentSelectObject;
        CurrentSelectObject = obj;
        mEventSystem.SetSelectedGameObject(CurrentSelectObject);
        LastSelectObject.transform.Find("Icon").gameObject.SetActive(false);
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
    }

    private void RecvSelectOptionByMouse(Notification notify)
    {
        if (UICurrState != ObjectState.READY || null == notify) { return; }
        
        GameObject obj = (GameObject)notify.Content;

        if (obj == Backgroud) { Cursor.visible = true; return; }

        GameObject pointerObject = GetPointerObject(obj);

        // Debug.Log(string.Format("pointerObject name : {0}", pointerObject.name));

        if (pointerObject == CurrentSelectObject) { return; }
        if (pointerObject == newGame.gameObject) { StateMachine.Instance.ChangeState(GameState.MAIN_MENU_NEW_GAME); }
        if (pointerObject == continueGame.gameObject) { StateMachine.Instance.ChangeState(GameState.MAIN_MENU_CONTINUE); }
        if (pointerObject == setting.gameObject) { StateMachine.Instance.ChangeState(GameState.MAIN_MENU_SETTING); }
        if (pointerObject == customize.gameObject) { StateMachine.Instance.ChangeState(GameState.MAIN_MENU_CUSTOMIZE); }
        if (pointerObject == online.gameObject) { StateMachine.Instance.ChangeState(GameState.MAIN_MENU_ONLINE); }
        if (pointerObject == exit.gameObject) { StateMachine.Instance.ChangeState(GameState.MAIN_MENU_EXIT); }
    }

    private GameObject GetPointerObject(GameObject obj)
    {
        PointerData pointerData = obj.gameObject.GetComponent<PointerData>();
        if (null == pointerData)
            return ExecuteEvents.GetEventHandler<IPointerEnterHandler>(obj);

        return obj;
    }
}
