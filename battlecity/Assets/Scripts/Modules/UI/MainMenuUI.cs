using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    public Button newGame = null;
    public Button continueGame = null;
    public Button setting = null;
    public Button customize = null;
    public Button online = null;
    public Button exit = null;

    private UIType uIType;
    private MainMenuScene mainMenuScene = null; // 场景

    private Color selectedColor = new Color(1f, 1f, 1f, 40 / 255f);
    private Color unSelectedColor = new Color(1f, 1f, 1f, 0);

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

        MessageController.Instance.AddNotification(NotificationName.SELECT_UP, RecvSelectUP);
        MessageController.Instance.AddNotification(NotificationName.SELECT_DOWN, RecvSelectDown);
        MessageController.Instance.AddNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.AddNotification(NotificationName.MOUSE_LEFT, RecvMouseLeft);
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        MessageController.Instance.AddNotification(NotificationName.FIRE, RecvFire);
        

        ResetUI();

        base.OnLoad();
    }

    public override void OnPause()
    {
        UnSelectObject(CurrentSelectObject);
        
        newGame.interactable = false;
        continueGame.interactable = false;
        setting.interactable = false;
        customize.interactable = false;
        online.interactable = false;
        exit.interactable = false;

        RemoveScripts();

        base.OnPause();
    }

    public override void OnResume()
    {
        Game.Instance.CreateCurrScene(SceneName.MENU, typeof(MainMenuScene));
        
        ResetUI();

        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.SELECT_UP, RecvSelectUP);
        MessageController.Instance.RemoveNotification(NotificationName.SELECT_DOWN, RecvSelectDown);
        MessageController.Instance.RemoveNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.RemoveNotification(NotificationName.MOUSE_LEFT, RecvMouseLeft);
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        MessageController.Instance.RemoveNotification(NotificationName.FIRE, RecvFire);

        base.OnRelease();
    }

    private void init()
    {
        uIType = UIType.MAIN_MENU_UI;
        MakeLogicConnections();
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

        SelectObject(newGame.gameObject);
        UnSelectObject(continueGame.gameObject);
        UnSelectObject(setting.gameObject);
        UnSelectObject(customize.gameObject);
        UnSelectObject(online.gameObject);
        UnSelectObject(exit.gameObject);
    }

    private void MakeLogicConnections()
    {
        MakeUILogicConnections(newGame.gameObject, continueGame.gameObject);
        MakeUILogicConnections(continueGame.gameObject, setting.gameObject);
        MakeUILogicConnections(setting.gameObject, customize.gameObject);
        MakeUILogicConnections(customize.gameObject, online.gameObject);
        MakeUILogicConnections(online.gameObject, exit.gameObject);
    }

    private void ChangeSelect(GameObject obj)
    {
        LastSelectObject = CurrentSelectObject;
        CurrentSelectObject = obj;
        UnSelectObject(LastSelectObject);
        SelectObject(CurrentSelectObject);
    }

    private void SelectObject(GameObject obj)
    {
        obj.GetComponent<Image>().color = selectedColor;
        obj.transform.Find("Icon").gameObject.SetActive(true);
    }

    private void UnSelectObject(GameObject obj)
    {
        obj.GetComponent<Image>().color = unSelectedColor;
        obj.transform.Find("Icon").gameObject.SetActive(false);
    }

    public override void RecvMouseMove(Notification notify)
    {
        Cursor.visible = true;
        AddScripts();
    }

    private void AddScripts()
    {
        newGame.gameObject.AddComponent<PointerData>();
        continueGame.gameObject.AddComponent<PointerData>();
        setting.gameObject.AddComponent<PointerData>();
        customize.gameObject.AddComponent<PointerData>();
        online.gameObject.AddComponent<PointerData>();
        exit.gameObject.AddComponent<PointerData>();
    }

    private void RemoveScripts()
    {
        PointerData pointerData = null;

        pointerData = newGame.gameObject.GetComponent<PointerData>();
        if (pointerData != null) { Destroy(pointerData); }

        pointerData = continueGame.gameObject.GetComponent<PointerData>();
        if (pointerData != null) { Destroy(pointerData); }

        pointerData = setting.gameObject.GetComponent<PointerData>();
        if (pointerData != null) { Destroy(pointerData); }

        pointerData = customize.gameObject.GetComponent<PointerData>();
        if (pointerData != null) { Destroy(pointerData); }

        pointerData = online.gameObject.GetComponent<PointerData>();
        if (pointerData != null) { Destroy(pointerData); }

        pointerData = exit.gameObject.GetComponent<PointerData>();
        if (pointerData != null) { Destroy(pointerData); }
    }

    public override void RecvPointerEnter(Notification notify)
    {
        if (UICurrState != ObjectState.READY || null == notify) { return; }
        
        GameObject obj = (GameObject)notify.Content;

        GameObject pointerObject = GetPointerObject(obj);

        if (pointerObject == CurrentSelectObject) { return; }

        ChangeSelect(pointerObject);
    }

    public override void RecvSelectUP(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        mainMenuScene = Game.Instance.CurrScene as MainMenuScene;
        if (null == mainMenuScene) { return; }
        
        if (mainMenuScene.CurrState != ObjectState.READY) { return; }

        RemoveScripts();

        Cursor.visible = false;

        GameObject up = GetUpConnection(CurrentSelectObject);
        if (null == up) { return; }

        ChangeSelect(up);
    }

    public override void RecvSelectDown(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }
        
        mainMenuScene = Game.Instance.CurrScene as MainMenuScene;
        if (null == mainMenuScene) { return; }
        
        if (mainMenuScene.CurrState != ObjectState.READY) { return; }

        RemoveScripts();

        Cursor.visible = false;

        GameObject below = GetDownConnection(CurrentSelectObject);
        if (null == below) { return; }

        ChangeSelect(below);
    }

    public override void RecvFire(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        mainMenuScene = Game.Instance.CurrScene as MainMenuScene;
        if (null == mainMenuScene) { return; }

        if (mainMenuScene.CurrState != ObjectState.READY) { return; }

        // RemoveScripts();

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
            mainMenuScene.ChangeState(SettingState.Instance);
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
    }

    public override void RecvMouseLeft(Notification notify)
    {
        RecvFire(notify);
    }
}
