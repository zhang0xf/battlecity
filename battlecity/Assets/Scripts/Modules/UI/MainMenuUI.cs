using System.Collections.Generic;
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

    private UIType uIType;
    private EventSystem mEventSystem = null;
    private MainMenuScene mainMenuScene = null; // 场景
    private Dictionary<GameObject, GameObject> dictUp = null;   // <对象, 上面位置的对象>
    private Dictionary<GameObject, GameObject> dictDown = null; // <对象，下面位置的对象>

    protected override void OnLoad()
    {
        // BaseUI : Awake()
        // Awake is called when the script instance is being loaded.
        // Awake is called either when an active GameObject that contains the script is initialized when a Scene loads,
        // or when a previously inactive GameObject is set to active,
        // or after a GameObject created with Object.Instantiate is initialized.
        // Use Awake to initialize variables or states before the application starts.

        init();
        Game.Instance.CreateCurrScene(SceneName.MENU, typeof(MainMenuScene));

        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_UP, RecvSelectUP);
        MessageController.Instance.AddNotification(NotificationName.UI_SELECT_DOWN, RecvSelectDown);
        MessageController.Instance.AddNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.AddNotification(NotificationName.UI_POINTER_ENTER, RecvPointerEnter);
        MessageController.Instance.AddNotification(NotificationName.FIRE, RecvFire);

        ResetUI();

        base.OnLoad();
    }

    public override void OnPause()
    {
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(false);
        newGame.interactable = false;
        continueGame.interactable = false;
        setting.interactable = false;
        customize.interactable = false;
        online.interactable = false;
        exit.interactable = false;

        base.OnPause();
    }

    public override void OnResume()
    {
        ResetUI();

        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_UP, RecvSelectUP);
        MessageController.Instance.RemoveNotification(NotificationName.UI_SELECT_DOWN, RecvSelectDown);
        MessageController.Instance.RemoveNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.RemoveNotification(NotificationName.UI_POINTER_ENTER, RecvPointerEnter);
        MessageController.Instance.RemoveNotification(NotificationName.FIRE, RecvFire);

        Destroy(gameObject, 0.0f);  // 延迟0秒
        base.OnRelease();
    }

    private void init()
    {
        uIType = UIType.MAIN_MENU_UI;
        mEventSystem = EventSystem.current; // Fetch the current EventSystem. Make sure your Scene has one.
        dictUp = new Dictionary<GameObject, GameObject>();
        dictDown = new Dictionary<GameObject, GameObject>();
        MappingButton();
    }

    private void ResetUI()
    {
        Cursor.visible = false;
        newGame.interactable = true;
        continueGame.interactable = true;
        setting.interactable = true;
        customize.interactable = true;
        online.interactable = true;
        exit.interactable = true;
        LastSelectObject = null;
        CurrentSelectObject = newGame.gameObject;
        CurrentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        mEventSystem.SetSelectedGameObject(CurrentSelectObject);
    }

    private void MappingButton()
    {
        List<Button> list = new List<Button>();
        list.Add(newGame);
        list.Add(continueGame);
        list.Add(setting);
        list.Add(customize);
        list.Add(online);
        list.Add(exit);
        Button[] buttons = list.ToArray();

        for (int idx = 0; idx < buttons.Length; idx++)
        {
            Button next = null;
            Button prev = null;
            Button current = buttons[idx];

            if (idx != 0)
                prev = buttons[idx - 1];
            if (idx != buttons.Length - 1)
                next = buttons[idx + 1];

            if(prev != null)
                dictUp.Add(current.gameObject, prev.gameObject);
            if(next != null)
                dictDown.Add(current.gameObject, next.gameObject);
        }
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

    private void RecvMouseMove(Notification notify)
    {
        Cursor.visible = true;
    }

    private void RecvPointerEnter(Notification notify)
    {
        if (UICurrState != ObjectState.READY || null == notify) { return; }
        
        GameObject obj = (GameObject)notify.Content;

        if (obj == Backgroud) { Cursor.visible = true; return; }

        GameObject pointerObject = GetPointerObject(obj);

        if (pointerObject == CurrentSelectObject) { return; }

        SelectObject(pointerObject);

        Cursor.visible = true;
    }

    private GameObject GetPointerObject(GameObject obj)
    {
        PointerData pointerData = obj.gameObject.GetComponent<PointerData>();
        if (null == pointerData)
            return ExecuteEvents.GetEventHandler<IPointerEnterHandler>(obj);

        return obj;
    }

    public override void RecvSelectUP(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        mainMenuScene = Game.Instance.CurrScene as MainMenuScene;
        if (null == mainMenuScene) { return; }
        
        if (mainMenuScene.CurrState != ObjectState.READY) { return; }

        GameObject above = GetAboveObject(CurrentSelectObject);
        if (null == above) { return; }

        SelectObject(above);
    }

    public override void RecvSelectDown(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }
        
        mainMenuScene = Game.Instance.CurrScene as MainMenuScene;
        if (null == mainMenuScene) { return; }
        
        if (mainMenuScene.CurrState != ObjectState.READY) { return; }

        GameObject below = GetBelowObject(CurrentSelectObject);
        if (null == below) { return; }

        SelectObject(below);
    }

    public override void RecvFire(Notification notify)
    {
        mainMenuScene = Game.Instance.CurrScene as MainMenuScene;
        if (null == mainMenuScene) { return; }

        if (CurrentSelectObject == newGame.gameObject) 
        {
            OnRelease();
            // mainMenuScene.ChangeState();
        }
        if (CurrentSelectObject == continueGame.gameObject)
        {
            OnRelease();
            // mainMenuScene.ChangeState(); 
        }
        if (CurrentSelectObject == customize.gameObject)
        {
            OnRelease();
            // mainMenuScene.ChangeState(); 
        }
        if (CurrentSelectObject == setting.gameObject)
        {
            OnPause();
            mainMenuScene.ChangeState(SettingUIState.Instance);
        }
        if (CurrentSelectObject == online.gameObject)
        {
            OnRelease();
            // mainMenuScene.ChangeState();
        }
        if (CurrentSelectObject == exit.gameObject)
        {
            OnPause();
            // mainMenuScene.ChangeState();
        }
        base.RecvFire(notify);
    }

    private GameObject GetAboveObject(GameObject button)
    {
        if (dictUp.ContainsKey(button.gameObject))
            return dictUp[button.gameObject];
        return null;
    }

    private GameObject GetBelowObject(GameObject button)
    {
        if (dictDown.ContainsKey(button.gameObject))
            return dictDown[button.gameObject];
        return null;
    }

}
