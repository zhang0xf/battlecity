using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
    public Toggle mAudio = null;
    public Toggle keyboard = null;
    public Toggle controller = null;
    public Slider slider = null;
    public GameObject mAudioBind = null;
    public GameObject keyboardBind = null;
    public GameObject controllerBind = null;
    public GameObject[] mAudioBindArray = null;
    public GameObject[] keyBoardBindArray = null;
    public GameObject[] controllerBindArray = null;

    private UIType uIType;
    private BaseScene settingScene = null;  // 场景
   
    private float audioValue = ResetValue;
    private Color deepRed = new Color(100 / 255f, 20 / 255f, 20 / 255f, 1f);    // 需要"f"指明是浮点运算，否则结果为0（整除）！
    private Color lightRed = new Color(200 / 255f, 20 / 255f, 20 / 255f, 1f);
    private Color selectedColor = new Color(1f, 1f, 1f, 40 / 255f);
    private Color unSelectedColor = new Color(1f, 1f, 1f, 0);

    private int index = -1;
    private Stack<GameObject> layer = null; // 菜单栈（记录选择）
    private static float ResetValue = 0.75f;
    private static float unitValue = 0.05f;

    protected override void OnLoad()
    {
        init();
        Game.Instance.CreateCurrScene(SceneName.MENU, typeof(SettingScene));

        MessageController.Instance.AddNotification(NotificationName.SELECT_UP, RecvSelectUP);
        MessageController.Instance.AddNotification(NotificationName.SELECT_DOWN, RecvSelectDown);
        MessageController.Instance.AddNotification(NotificationName.SELECT_LEFT, RecvSelectLeft);
        MessageController.Instance.AddNotification(NotificationName.SELECT_RIGHT, RecvSelectRight);
        MessageController.Instance.AddNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.AddNotification(NotificationName.MOUSE_LEFT, RecvMouseLeft);
        MessageController.Instance.AddNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        MessageController.Instance.AddNotification(NotificationName.FIRE, RecvFire);
        MessageController.Instance.AddNotification(NotificationName.BACK, RecvBack);
        

        ResetUI(true);

        base.OnLoad();
    }

    private void init()
    {
        uIType = UIType.SETTING_UI;

        dictUp = new Dictionary<GameObject, GameObject>();
        dictDown = new Dictionary<GameObject, GameObject>();

        layer = new Stack<GameObject>();

        mAudio.onValueChanged.AddListener(delegate { SetToggleContent(); });
        keyboard.onValueChanged.AddListener(delegate { SetToggleContent(); });
        controller.onValueChanged.AddListener(delegate { SetToggleContent(); });

        MakeLogicConnections();
    }

    private void MakeLogicConnections()
    {
        MakeUILogicConnections(mAudio.gameObject, keyboard.gameObject);
        MakeUILogicConnections(keyboard.gameObject, controller.gameObject);
        MakeUILogicConnections(keyBoardBindArray);
        MakeUILogicConnections(controllerBindArray);
    }

    public override void OnPause()
    {
        SetInvalid();
        base.OnPause();
    }

    private void SetInvalid()
    {
        mAudio.interactable = false;
        keyboard.interactable = false;
        controller.interactable = false;
        slider.interactable = false;

        foreach (GameObject obj in keyBoardBindArray)
        {
            obj.transform.Find("BindBackgroud/Fill").GetComponent<Button>().interactable = false;
            obj.transform.Find("BindBackgroud").GetComponent<Image>().color = unSelectedColor;
           
        }
        foreach (GameObject obj in controllerBindArray)
        {
            obj.transform.Find("BindBackgroud/Fill").GetComponent<Button>().interactable = false;
            obj.transform.Find("BindBackgroud").GetComponent<Image>().color = unSelectedColor;
        }
    }

    public override void OnResume()
    {
        Game.Instance.CreateCurrScene(SceneName.MENU, typeof(SettingScene));
        SetValid();
        ResetUI(false);
        base.OnResume();
    }

    private void SetValid()
    {
        mAudio.interactable = true;
        keyboard.interactable = true;
        controller.interactable = true;
        slider.interactable = true;

        foreach (GameObject obj in keyBoardBindArray)
        {
            obj.transform.Find("BindBackgroud/Fill").GetComponent<Button>().interactable = true;
            obj.transform.Find("BindBackgroud").GetComponent<Image>().color = Color.white;
        }

        foreach (GameObject obj in controllerBindArray)
        {
            obj.transform.Find("BindBackgroud/Fill").GetComponent<Button>().interactable = true;
            obj.transform.Find("BindBackgroud").GetComponent<Image>().color = Color.white;
        }
    }

    public override void OnRelease()
    {
        MessageController.Instance.RemoveNotification(NotificationName.SELECT_UP, RecvSelectUP);
        MessageController.Instance.RemoveNotification(NotificationName.SELECT_DOWN, RecvSelectDown);
        MessageController.Instance.RemoveNotification(NotificationName.SELECT_LEFT, RecvSelectLeft);
        MessageController.Instance.RemoveNotification(NotificationName.SELECT_RIGHT, RecvSelectRight);
        MessageController.Instance.RemoveNotification(NotificationName.MOUSE_MOVE, RecvMouseMove);
        MessageController.Instance.RemoveNotification(NotificationName.MOUSE_LEFT, RecvMouseLeft);
        MessageController.Instance.RemoveNotification(NotificationName.POINTER_ENTER, RecvPointerEnter);
        MessageController.Instance.RemoveNotification(NotificationName.FIRE, RecvFire);
        MessageController.Instance.RemoveNotification(NotificationName.BACK, RecvBack);

        base.OnRelease();
    }

    private void ResetUI( bool isReset)
    {
        layer.Clear();
        LastSelectObject = null;
        mAudio.isOn = true;
        keyboard.isOn = false;
        controller.isOn = false;
        CurrentSelectObject = mAudio.gameObject;
        SelectAudio(isReset);
        SetToggleContent();
    }

    private void SetToggleContent()
    {
        mAudioBind.gameObject.SetActive(mAudio.isOn);
        keyboardBind.gameObject.SetActive(keyboard.isOn);
        controllerBind.gameObject.SetActive(controller.isOn);
    }

    public override void RecvFire(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        settingScene = Game.Instance.CurrScene as SettingScene;
        if (null == settingScene) { return; }

        if (settingScene.CurrState != ObjectState.READY) { return; }

        if (layer.Peek() == mAudio.gameObject)
        {
            ChangeSelect(mAudioBindArray[0], false, true);
        }
        else if (layer.Peek() == keyboard.gameObject)
        {
            ChangeSelect(keyBoardBindArray[0], false, true);
        }
        else if (layer.Peek() == controller.gameObject)
        {
            ChangeSelect(controllerBindArray[0], false, true);
        }
        else if ((index = Array.IndexOf(keyBoardBindArray, layer.Peek())) != -1 ||
            (index = Array.IndexOf(controllerBindArray, layer.Peek())) != -1)
        {
            OnPause();
            settingScene.ChangeState(BindKeyState.Instance);
        }

    }

    public override void RecvBack(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        settingScene = Game.Instance.CurrScene as SettingScene;
        if (null == settingScene) { return; }

        if (settingScene.CurrState != ObjectState.READY) { return; }

        if (layer.Count == 0) { return; }

        layer.Pop();

        if (layer.Count == 0)
        {
            UIManager.Instance.PopUI();
        }
        else
        {
            ChangeSelect(layer.Peek(), true, false);
        }
    }

    public override void RecvSelectUP(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        settingScene = Game.Instance.CurrScene as SettingScene;
        if (null == settingScene) { return; }

        if (settingScene.CurrState != ObjectState.READY) { return; }

        GameObject up = GetUpConnection(CurrentSelectObject);
        if (null == up) { return; }

        ChangeSelect(up);
    }

    public override void RecvSelectDown(Notification notify)
    {
        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        settingScene = Game.Instance.CurrScene as SettingScene;
        if (null == settingScene) { return; }

        if (settingScene.CurrState != ObjectState.READY) { return; }

        GameObject down = GetDownConnection(CurrentSelectObject);
        if (null == down) { return; }

        ChangeSelect(down);
    }

    public override void RecvSelectLeft(Notification notify)
    {
        if ((index = Array.IndexOf(mAudioBindArray, CurrentSelectObject)) == -1) { return; }
        slider.value -= unitValue;
        // AudioManager
    }

    public override void RecvSelectRight(Notification notify)
    {
        if ((index = Array.IndexOf(mAudioBindArray, CurrentSelectObject)) == -1) { return; }
        slider.value += unitValue;
        // AudioManager
    }

    public override void RecvMouseMove(Notification notify)
    {
        Cursor.visible = true;
    }

    public override void RecvMouseLeft(Notification notify)
    {
        RecvFire(notify);

        base.RecvMouseLeft(notify);
    }

    public override void RecvPointerEnter(Notification notify)
    {
        if (UICurrState != ObjectState.READY || null == notify) { return; }

        GameObject obj = (GameObject)notify.Content;

        GameObject pointerObject = GetPointerObject(obj);

        if (pointerObject == CurrentSelectObject) { return; }

        ChangeSelect(pointerObject);

        Cursor.visible = true;
    }

    private void ChangeSelect(GameObject obj, bool isUnSelect = true, bool isSelect = true)
    {
        Cursor.visible = false;
        LastSelectObject = CurrentSelectObject;
        CurrentSelectObject = obj;
        if (isUnSelect) { UnSelectObject(LastSelectObject); }
        if (isSelect) { SelectObject(CurrentSelectObject); }
    }

    private void SelectObject(GameObject obj)
    {
        if (obj == mAudio.gameObject) { SelectAudio(false); }
        else if (obj == keyboard.gameObject) { SelectKeyboard(); }
        else if (obj == controller.gameObject) { SelectController(); }
        else if ((index = Array.IndexOf(mAudioBindArray, obj)) != -1) { SelectAudioArray(index); }
        else if ((index = Array.IndexOf(keyBoardBindArray, obj)) != -1) { SelectKeyboardArray(index); }
        else if ((index = Array.IndexOf(controllerBindArray, obj)) != -1) { SelectControllerArray(index); }
    }

    private void UnSelectObject(GameObject obj)
    {
        if (obj == mAudio.gameObject) { UnSelectAudio(); }
        else if (obj == keyboard.gameObject) { UnSelectKeyboard(); }
        else if (obj == controller.gameObject) { UnSelectController(); }
        else if ((index = Array.IndexOf(mAudioBindArray, obj)) != -1) { UnSelectAudioArray(index); }
        else if ((index = Array.IndexOf(keyBoardBindArray, obj)) != -1) { UnSelectKeyboardArray(index); }
        else if ((index = Array.IndexOf(controllerBindArray, obj)) != -1) { UnSelectControllerArray(index); }
    }

    private void SelectAudio(bool isReset)
    {
        if (isReset)
            slider.value = ResetValue;
        else
            slider.value = audioValue;

        mAudio.isOn = true;
        mAudio.GetComponent<Image>().color = selectedColor;
        slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = deepRed;  // 相对路径查找

        layer.Push(mAudio.gameObject);
    }

    private void UnSelectAudio()
    {
        mAudio.isOn = false;
        mAudio.GetComponent<Image>().color = unSelectedColor;
        slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = deepRed;

        if (layer.Peek() == mAudio.gameObject)
            layer.Pop();
    }

    private void SelectKeyboard()
    {
        keyboard.isOn = true;
        keyboard.GetComponent<Image>().color = selectedColor;

        layer.Push(keyboard.gameObject);
    }

    private void UnSelectKeyboard()
    {
        keyboard.isOn = false;
        keyboard.GetComponent<Image>().color = unSelectedColor;

        if (layer.Peek() == keyboard.gameObject)
            layer.Pop();
    }

    private void SelectController()
    {
        controller.isOn = true;
        controller.GetComponent<Image>().color = selectedColor;

        layer.Push(controller.gameObject);
    }

    private void UnSelectController()
    {
        controller.isOn = false;
        controller.GetComponent<Image>().color = unSelectedColor;

        if (layer.Peek() == controller.gameObject)
            layer.Pop();
    }

    private void SelectAudioArray(int index)
    {
        slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = lightRed;

        layer.Push(mAudioBindArray[index]);
    }

    private void SelectKeyboardArray(int index)
    {
        GameObject bindBackgroud = keyBoardBindArray[index].transform.Find("BindBackgroud").gameObject;
        bindBackgroud.GetComponent<Image>().color = lightRed;

        layer.Push(keyBoardBindArray[index]);
    }

    private void SelectControllerArray(int index)
    {
        GameObject bindBackgroud = controllerBindArray[index].transform.Find("BindBackgroud").gameObject;
        bindBackgroud.GetComponent<Image>().color = lightRed;

        layer.Push(controllerBindArray[index]);
    }

    private void UnSelectAudioArray(int index)
    {
        slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = deepRed;

        if (layer.Peek() == mAudioBindArray[index])
            layer.Pop();
    }

    private void UnSelectKeyboardArray(int index)
    {
        GameObject bindBackgroud = keyBoardBindArray[index].transform.Find("BindBackgroud").gameObject;
        bindBackgroud.GetComponent<Image>().color = Color.white;

        if (layer.Peek() == keyBoardBindArray[index])
            layer.Pop();
    }

    private void UnSelectControllerArray(int index)
    {
        GameObject bindBackgroud = controllerBindArray[index].transform.Find("BindBackgroud").gameObject;
        bindBackgroud.GetComponent<Image>().color = Color.white;

        if (layer.Peek() == controllerBindArray[index])
            layer.Pop();
    }
}
