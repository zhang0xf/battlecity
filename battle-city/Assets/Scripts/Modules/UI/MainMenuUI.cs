using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [SerializeField] private Button m_NewGame;
    [SerializeField] private Button m_Continue;
    [SerializeField] private Button m_Setting;
    [SerializeField] private Button m_Customize;
    [SerializeField] private Button m_Online;
    [SerializeField] private Button m_Exit;

    private EventSystem m_EventSystem;

    // call by BaseUI:Awake()
    protected override void OnLoad()
    {
        // make sure scene has an EventSystem!
        m_EventSystem = EventSystem.current;

        // add listener
        m_NewGame.onClick.AddListener(delegate { NewGameButtonOnClick(); });
        m_Continue.onClick.AddListener(delegate { ContinueButtonOnClick(); });
        m_Setting.onClick.AddListener(delegate { SettingButtonOnClick(); });
        m_Customize.onClick.AddListener(delegate { CustomizeButtonOnClick(); });
        m_Online.onClick.AddListener(delegate { OnlineButtonOnClick(); });
        m_Exit.onClick.AddListener(delegate { ExitButtonOnClick(); });

        StartCoroutine(SetSelect(m_NewGame.gameObject));

        m_UIType = UIType.MAIN_MENU_UI;
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnLoad();
    }

    public override void OnPause()
    {
        m_NewGame.interactable = false;
        m_Continue.interactable = false;
        m_Setting.interactable = false;
        m_Customize.interactable = false;
        m_Online.interactable = false;
        m_Exit.interactable = false;
        m_EventSystem.SetSelectedGameObject(null);
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnPause();
    }

    public override void OnResume()
    {
        m_NewGame.interactable = true;
        m_Continue.interactable = true;
        m_Setting.interactable = true;
        m_Customize.interactable = true;
        m_Online.interactable = true;
        m_Exit.interactable = true;
        m_EventSystem.SetSelectedGameObject(null);
        m_EventSystem.SetSelectedGameObject(m_NewGame.gameObject);
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnRelease();
    }

    private void RecvPointerEnter(Notification notify)
    {
        if (CurrState != ObjState.READY || null == notify) { return; }
        
        GameObject obj = null;
        GameObject pointer = (GameObject)notify.Content;

        // if child has no PointerEnterEvent.cs,find in parent.
        PointerEnterEvent pointerEnterEvent = pointer.GetComponent<PointerEnterEvent>();
        if (null == pointerEnterEvent)
            obj = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(pointer);
        else
            obj = pointer;

        if (null == obj || obj == m_EventSystem.currentSelectedGameObject) { return; }

        m_EventSystem.SetSelectedGameObject(null);
        m_EventSystem.SetSelectedGameObject(obj);
    }

    private IEnumerator SetSelect(GameObject obj)
    {
        m_EventSystem.SetSelectedGameObject(null);
        // wait for the end of this frame, otherwise the "UI" will not be highlighted.
        yield return null;
        m_EventSystem.SetSelectedGameObject(obj);
    }

    private void NewGameButtonOnClick()
    {
        GameManager.Instance.ChangeState(GameState.NEWGAME);
    }

    private void ContinueButtonOnClick()
    {
        GameManager.Instance.ChangeState(GameState.CONTINUE);
    }

    private void SettingButtonOnClick()
    {
        UIManager.Instance.OpenUI(UIType.SETTING_UI, true);
    }

    private void CustomizeButtonOnClick()
    {
        GameManager.Instance.ChangeState(GameState.CUSTOMIZE);
    }

    private void OnlineButtonOnClick()
    {
        GameManager.Instance.ChangeState(GameState.ONLINE);
    }

    private void ExitButtonOnClick()
    {
        // UIManager.Instance.OpenUI(UIType.EXIT_UI, true);
    }
}
