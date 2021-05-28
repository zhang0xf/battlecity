using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    private Toggle keyboard = null;
    private Toggle controller = null;
    private Slider slider = null;
    private EventSystem mEventSystem = null;
    private GameObject mAudio = null;
    private GameObject keyboardBind = null;
    private GameObject controllerBind = null;
    private Color defaultSelectedColor = Color.clear;
    private Color selectedColor = Color.clear;
    private Color sliderDefaultColor = Color.clear;
    private Color sliderSelectedColor = Color.clear;
    private float sliderCurrValue = 0.0f;
    private readonly float sliderDefaultValue = 0.75f;

    protected override void OnLoad()
    {
        Debug.Log("SettingUI Awake()");


        base.OnLoad();
    }

    private void OnEnable()
    {
        mEventSystem = EventSystem.current;

        PointerData pointerData = gameObject.AddComponent<PointerData>();
        if (null == pointerData) { Debug.LogError(string.Format("pointerData is null")); }

        keyboard = gameObject.transform.Find("DynamicBindButton/Keyboard").gameObject.GetComponent<Toggle>();
        if (null == keyboard) { Debug.LogError(string.Format("gameObject is null : keyboard")); }
        keyboard.onValueChanged.AddListener(delegate { SetToggleContent(); } );

        controller = gameObject.transform.Find("DynamicBindButton/Controller").gameObject.GetComponent<Toggle>();
        if (null == controller) { Debug.LogError(string.Format("gameObject is null : controller")); }
        controller.onValueChanged.AddListener(delegate { SetToggleContent(); });

        mAudio = gameObject.transform.Find("Audio").gameObject;
        if (null == mAudio) { Debug.LogError(string.Format("gameObject is null : mAudio")); }

        slider = gameObject.transform.Find("Audio/Slider").gameObject.GetComponent<Slider>();
        if (null == slider) { Debug.LogError(string.Format("gameObject is null : slider")); }

        keyboardBind = gameObject.transform.Find("DynamicBindButton/KeyBoardBind").gameObject;
        if (null == keyboardBind) { Debug.LogError(string.Format("gameObject is null : keyboardBind")); }

        controllerBind = gameObject.transform.Find("DynamicBindButton/ControllerBind").gameObject;
        if (null == controllerBind) { Debug.LogError(string.Format("gameObject is null : controllerBind")); }
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultSelectedColor = new Color(1, 1, 1, 0);
        selectedColor = new Color(1, 1, 1, 20 / 255f);
        sliderDefaultColor = new Color(100 / 255, 20 / 255, 20 / 255, 1);
        sliderSelectedColor = new Color(200 / 255, 20 / 255, 20 / 255, 1);
        SetDefaultSelected();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    private void SetDefaultSelected()
    {
        keyboard.isOn = true;
        controller.isOn = false;
        slider.value = sliderDefaultValue;
        LastSelectObject = null;
        CurrentSelectObject = mAudio;
        sliderCurrValue = sliderDefaultValue;
        SetToggleContent();
    }

    private void SetToggleContent()
    {
        keyboardBind.gameObject.SetActive(keyboard.isOn);
        controllerBind.gameObject.SetActive(controller.isOn);
    }

    private void RecvSetAudioSelected(Notification notify)
    {
        CurrentSelectObject = mAudio;
        mEventSystem.SetSelectedGameObject(CurrentSelectObject);
        mAudio.GetComponent<Image>().color = selectedColor;

        GameObject fill = slider.transform.Find("Fill").gameObject;
        if (null == fill) { return; }

        fill.GetComponent<Image>().color = sliderSelectedColor;
    }

    private void RecvReduceSliderValue(Notification notify)
    {
        sliderCurrValue -= 0.1f;
    }

    private void RecvAddSliderValue(Notification notify)
    {
        sliderCurrValue += 0.1f;
    }
}
