using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

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

    [SerializeField] private AudioClip m_Hit;
    [SerializeField] private AudioClip m_Fire;
    [SerializeField] private AudioSource m_AudioSFX;
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private string m_MusicVolume = "MusicVolume";
    
    private float m_Multiplier = 30f;
    private float m_RecordMusicVolume;
    private float m_RecordSliderValue;
    private EventSystem m_EventSystem;
    private InputMaster m_InputyManager;

    // call by BaseUI:Awake()
    protected override void OnLoad()
    {
        // make sure scene has an EventSystem!
        m_EventSystem = EventSystem.current;

        // To use the controls, we need to instantiate them.
        m_InputyManager = new InputMaster();

        // get slider value between game session.
        // onValueChanged has not added!
        m_Slider.value = PlayerPrefs.GetFloat(m_Slider.name);

        // set callback
        // lambda expression(all lambdas can convert to an Action<> or Func<> or Predicate<>)
        // "ctx" is parameter which type is "CallbackContext"
        // in runtime lambda expression will convert to Action<> or Func<> or Predicate<> automatically,
        // to match the Action<> or Func<> or Predicate<>
        m_InputyManager.UI.Navigate.performed += ctx => HandleNavigatePerformedEvent(ctx);

        m_InputyManager.UI.Submit.performed += ctx => HandleSubmitPerformedEvent(ctx);

        m_InputyManager.UI.Cancel.performed += ctx => HandleCancelPerformedEvent(ctx);

        StartCoroutine(SetSelect(m_Audio.gameObject));

        m_Audio.onValueChanged.AddListener(delegate { SetToggleContent(m_AudioBind, m_Audio.isOn); });
        m_Keyboard.onValueChanged.AddListener(delegate { SetToggleContent(m_KeyboardBind, m_Keyboard.isOn); });
        m_Controller.onValueChanged.AddListener(delegate { SetToggleContent(m_ControllerBind, m_Controller.isOn); });
        m_Music.onValueChanged.AddListener(delegate {HandleMusicToggleValueChange(m_Music.isOn); });
        m_Slider.onValueChanged.AddListener(delegate {HandleAudioSliderValueChange(m_Slider.value); });

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
        //`PlayerPrefs` is a class that stores Player preferences between game sessions.
        //It can store string, float and integer values into the user's platform registry.
        PlayerPrefs.SetFloat(m_MusicVolume, m_RecordMusicVolume);
        PlayerPrefs.SetFloat(m_Slider.name, m_RecordSliderValue);
        m_InputyManager.Disable();
    }

    public override void OnPause()
    {
        m_InputyManager.UI.Navigate.performed -= ctx => HandleNavigatePerformedEvent(ctx);
        m_InputyManager.Disable();
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnPause();
    }

    public override void OnResume()
    {
        m_InputyManager.UI.Navigate.performed += ctx => HandleNavigatePerformedEvent(ctx);
        m_InputyManager.Enable();
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnResume();
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        base.OnRelease();
    }

    private void SetToggleContent(GameObject obj, bool isOn)
    {
        if (null == obj) { return; }
        obj.SetActive(isOn);
    }

    private void HandleMusicToggleValueChange(bool isOn)
    {
        if (isOn)
            m_Slider.value = m_Slider.maxValue * 0.75f;
        else
            m_Slider.value = m_Slider.minValue;
    }

    private void HandleAudioSliderValueChange(float value)
    {
        // Debug.LogFormat("value is {0}", value);
        m_RecordSliderValue = value;
        // log10(0) is -infinity(¸ºÎÞÇî), will cause SetFloat() failure. and sound still play.
        m_RecordMusicVolume = Mathf.Log10(value) * m_Multiplier;
        m_AudioMixer.SetFloat(m_MusicVolume, m_RecordMusicVolume);
        m_Music.isOn = value > m_Slider.minValue;
    }

    private void HandleNavigatePerformedEvent(CallbackContext context)
    {
        AudioPlay(m_Hit);
    }

    private void HandleSubmitPerformedEvent(CallbackContext context)
    {
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

        AudioPlay(m_Fire);
    }

    private void HandleCancelPerformedEvent(CallbackContext context)
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

        AudioPlay(m_Hit);
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

    private void AudioPlay(AudioClip clip)
    {
        if (null == clip) { return; }

        m_AudioSFX.clip = clip;
        m_AudioSFX.Play();
    }
}
