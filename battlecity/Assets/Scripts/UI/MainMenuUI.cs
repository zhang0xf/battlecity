using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MainMenuUI : BaseUI
{
    [SerializeField] private Button m_NewGame;
    [SerializeField] private Button m_Continue;
    [SerializeField] private Button m_Setting;
    [SerializeField] private Button m_Customize;
    [SerializeField] private Button m_Online;
    [SerializeField] private Button m_Exit;
    [SerializeField] private GameObject m_ExitOverlay;
    [SerializeField] private Button m_Yes;
    [SerializeField] private Button m_No;
    [SerializeField] private AudioClip m_Hit;
    [SerializeField] private AudioClip m_Fire;
    [SerializeField] private AudioSource m_AudioSFX;
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private string m_MusicVolume = "MusicVolume";

    private InputMaster m_InputManager;
    private InputDevice m_Device;
    private EventSystem m_EventSystem;

    // call by BaseUI:Awake()
    protected override void OnLoad()
    {
        // make sure scene has an EventSystem!
        m_EventSystem = EventSystem.current;

        m_InputManager = new InputMaster();

        // init Start Music Volume
        m_AudioMixer.SetFloat(m_MusicVolume, PlayerPrefs.GetFloat(m_MusicVolume));

        // hide
        m_ExitOverlay.SetActive(false);

        // add listener
        m_NewGame.onClick.AddListener(delegate { NewGameButtonOnClick(); });
        m_Continue.onClick.AddListener(delegate { ContinueButtonOnClick(); });
        m_Setting.onClick.AddListener(delegate { SettingButtonOnClick(); });
        m_Customize.onClick.AddListener(delegate { CustomizeButtonOnClick(); });
        m_Online.onClick.AddListener(delegate { OnlineButtonOnClick(); });
        m_Exit.onClick.AddListener(delegate { ExitButtonOnClick(); });
        m_Yes.onClick.AddListener(delegate { YesButtonOnClick(); });
        m_No.onClick.AddListener(delegate { NoButtonOnClick(); });

        // lambda ??????????????????performed??????Action??????ctx????????
        m_InputManager.UI.Navigate.performed += ctx => HandleNavigatePerformedEvent(ctx);

        // set callback
        m_InputManager.UI.Cancel.performed += ctx => HandleCancelPerformedEvent(ctx);

        // callback to get device info
        InputSystem.onActionChange +=
            (obj, change) =>
            {
                if (change == InputActionChange.ActionPerformed)
                {
                    InputAction m_InputAction = (InputAction)obj;
                    m_Device = m_InputAction.activeControl.device;
                    // Debug.Log($"device: {m_Device.displayName}");
                }
            };

        StartCoroutine(SetSelect(m_NewGame.gameObject));

        m_UIType = UIType.MAIN_MENU_UI;
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnLoad();
    }

    private void OnEnable()
    {
        m_InputManager.Enable();
    }

    private void OnDisable()
    {
        m_InputManager.Disable();
    }

    public override void OnPause()
    {
        ButtonDisable();
        m_Yes.interactable = false;
        m_No.interactable = false;
        m_EventSystem.SetSelectedGameObject(null);
        m_InputManager.Disable();
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnPause();
    }

    public override void OnResume()
    {
        ButtonEnable();
        m_Yes.interactable = true;
        m_No.interactable = true;
        m_EventSystem.SetSelectedGameObject(null);
        m_EventSystem.SetSelectedGameObject(m_NewGame.gameObject);
        m_InputManager.Enable();
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnRelease();
    }

    private void ButtonEnable()
    {
        m_NewGame.interactable = true;
        m_Continue.interactable = true;
        m_Setting.interactable = true;
        m_Customize.interactable = true;
        m_Online.interactable = true;
        m_Exit.interactable = true;
    }

    private void ButtonDisable()
    {
        m_NewGame.interactable = false;
        m_Continue.interactable = false;
        m_Setting.interactable = false;
        m_Customize.interactable = false;
        m_Online.interactable = false;
        m_Exit.interactable = false;
    }

    private void HandleNavigatePerformedEvent(CallbackContext context)
    {
        AudioPlay(m_Hit);
    }

    private void HandleCancelPerformedEvent(CallbackContext context)
    {
        if (m_EventSystem.currentSelectedGameObject == m_Yes.gameObject ||
            m_EventSystem.currentSelectedGameObject == m_No.gameObject)
        {
            ButtonEnable();
            m_ExitOverlay.SetActive(false);
            StartCoroutine(SetSelect(m_Exit.gameObject));
        }
        else {
            ButtonDisable();
            m_ExitOverlay.SetActive(true);
            StartCoroutine(SetSelect(m_Yes.gameObject));
        }
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

        StartCoroutine(SetSelect(obj));

        AudioPlay(m_Hit);
    }

    private IEnumerator SetSelect(GameObject obj)
    {
        m_EventSystem.SetSelectedGameObject(null);
        // wait for the end of this frame, otherwise the "UI" will not be highlighted.
        yield return null;
        m_EventSystem.SetSelectedGameObject(obj);
    }

    private void AudioPlay(AudioClip clip)
    {
        if (null == clip) { return; }
        m_AudioSFX.clip = clip;
        m_AudioSFX.Play();
    }

    private void NewGameButtonOnClick()
    {
        AudioPlay(m_Fire);
        GameManager.Instance.ChangeState(GameState.NEWGAME);
    }

    private void ContinueButtonOnClick()
    {
        AudioPlay(m_Fire);
        GameManager.Instance.ChangeState(GameState.CONTINUE);
    }

    private void SettingButtonOnClick()
    {
        AudioPlay(m_Fire);
        UIManager.Instance.OpenUI(UIType.SETTING_UI, true);
    }

    private void CustomizeButtonOnClick()
    {
        AudioPlay(m_Fire);
        GameManager.Instance.ChangeState(GameState.CUSTOMIZE);
    }

    private void OnlineButtonOnClick()
    {
        AudioPlay(m_Fire);
        GameManager.Instance.ChangeState(GameState.ONLINE);
    }

    private void ExitButtonOnClick()
    {
        AudioPlay(m_Fire);
        ButtonDisable();
        m_ExitOverlay.SetActive(true);
        StartCoroutine(SetSelect(m_Yes.gameObject));
    }

    private void YesButtonOnClick()
    {
        Application.Quit();
    }

    private void NoButtonOnClick()
    {
        ButtonEnable();
        m_ExitOverlay.SetActive(false);
        StartCoroutine(SetSelect(m_Exit.gameObject));
    }
}
