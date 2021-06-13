using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    [SerializeField] private Toggle m_Audio;
    [SerializeField] private Toggle m_Keyboard;
    [SerializeField] private Toggle m_Controller;
    [SerializeField] private Toggle m_Music;
    [SerializeField] private Slider m_Slider;
    [SerializeField] private GameObject m_AudioBind;
    [SerializeField] private GameObject m_KeyboardBind;
    [SerializeField] private GameObject m_ControllerBind;
    [SerializeField] private Button[] m_KeyboardBindArray;
    [SerializeField] private Button[] m_ControllerBindArray;

    private EventSystem m_EventSystem;
    private InputManager m_InputyManager;

    // call by BaseUI:Awake()
    protected override void OnLoad()
    {
        // make sure scene has an EventSystem!
        m_EventSystem = EventSystem.current;

        // To use the controls, we need to instantiate them.
        m_InputyManager = new InputManager();

        // set callback
        m_InputyManager.UI.Submit.performed +=
            ctx =>
            {
                // callback
                // lambda expression
                // "ctx" is parameter which type is "CallbackContext"
                // in runtime lambda expression will convert to Action<> or Func<> automatically, to match the Action's or Func's Parameter type.
                // all lambdas can convert to an Action<> or Func<>.
                if (m_EventSystem.currentSelectedGameObject == m_Audio.gameObject &&
                m_AudioBind.gameObject.activeSelf)
                {
                    // select right content(slider)
                    StartCoroutine(SetSelect(m_Slider.gameObject));
                }
                else if (m_EventSystem.currentSelectedGameObject == m_Keyboard.gameObject &&
                m_KeyboardBind.gameObject.activeSelf)
                {
                    // select right content(button)
                    StartCoroutine(SetSelect(m_KeyboardBindArray[0].gameObject));
                }
                else if (m_EventSystem.currentSelectedGameObject == m_Controller.gameObject &&
                m_ControllerBind.gameObject.activeSelf)
                {
                    // select right content(button)
                    StartCoroutine(SetSelect(m_ControllerBindArray[0].gameObject));
                }
            };

        // set callback
        m_InputyManager.UI.Cancel.performed +=
            ctx =>
            {
                if (m_EventSystem.currentSelectedGameObject == m_Slider.gameObject ||
                m_EventSystem.currentSelectedGameObject == m_Music.gameObject)
                {
                    StartCoroutine(SetSelect(m_Audio.gameObject));
                }
                else if (IsInArray(m_KeyboardBindArray, m_EventSystem.currentSelectedGameObject))
                {
                    StartCoroutine(SetSelect(m_Keyboard.gameObject));
                }
                else if (IsInArray(m_ControllerBindArray, m_EventSystem.currentSelectedGameObject))
                {
                    StartCoroutine(SetSelect(m_Controller.gameObject));
                }
                else if (m_EventSystem.currentSelectedGameObject == m_Audio.gameObject ||
                m_EventSystem.currentSelectedGameObject == m_Keyboard.gameObject ||
                m_EventSystem.currentSelectedGameObject == m_Controller.gameObject)
                {
                    UIManager.Instance.PopUI(UIType.SETTING_UI);
                    if (UIManager.Instance.IsResume())
                    {
                        UIManager.Instance.ResumeUI(UIManager.Instance.GetPeekUI());
                    }
                }
            };

        StartCoroutine(SetSelect(m_Audio.gameObject));

        m_Audio.onValueChanged.AddListener(delegate { SetToggleContent(); });
        m_Keyboard.onValueChanged.AddListener(delegate { SetToggleContent(); });
        m_Controller.onValueChanged.AddListener(delegate { SetToggleContent(); });

        m_UIType = UIType.SETTING_UI;
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnLoad();
    }

    private void OnEnable()
    {
        m_InputyManager.Enable();
    }

    private void OnDisable()
    {
        m_InputyManager.Disable();
    }

    public override void OnPause()
    {
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnPause();
    }

    public override void OnResume()
    {
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnRelease();
    }

    private void SetToggleContent()
    {
        m_AudioBind.gameObject.SetActive(m_Audio.isOn);
        m_KeyboardBind.gameObject.SetActive(m_Keyboard.isOn);
        m_ControllerBind.gameObject.SetActive(m_Controller.isOn);
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

    private bool IsInArray(Button[] array, GameObject obj)
    {
        if (array.Length == 0 || null == obj) { return false; }

        foreach (Button button in array)
        {
            if (button.gameObject == obj)
            {
                return true;
            }
        }
        return false;
    }
}
