using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager
{
    private static GameObject UICanvas = null;
    private static UIManager mInstance = null;
    private Dictionary<string, Stack<KeyValuePair<UIType, GameObject>>> UIRecord = null; // 当前场景的UI记录
    private Dictionary<object, ObjectState> UIState = null; // UI和UI的状态记录（UI状态改变时自动更新）
    private UIPathConfig UIPathData = null;

    private UIManager()
    {
        UIRecord = new Dictionary<string, Stack<KeyValuePair<UIType, GameObject>>>();
        UIState = new Dictionary<object, ObjectState>();
        UIPathData = UIPathConfig.Instance;
    }

    public static UIManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new UIManager();
            return mInstance;
        }
    }

    public static void UpdateCanvas()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        UICanvas = GameObject.Find("Canvas");
        if (null == UICanvas)
        {
            UICanvas = new GameObject();
            UICanvas.name = "UIUICanvas";
            UICanvas.AddComponent<Canvas>();
            UICanvas.AddComponent<CanvasScaler>();
            UICanvas.AddComponent<GraphicRaycaster>();

            UICanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = UICanvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;
        }
    }

    public void OpenUI(UIType uIType)
    {
        Game.Instance.StartCoroutine(OpenUIAsynchronous(uIType));
    }

    private IEnumerator OpenUIAsynchronous(UIType uIType)
    {
        yield return new WaitForSeconds(0.0f);  // yield return : stop and continue next frame

        if (IsUIReady(uIType)) { yield break; }

        string path = UIPathData.GetPathByUIType(uIType);
        if (null == path) { yield break; }

        GameObject UIObject = Resources.Load(path) as GameObject;
        if (null == UIObject) { yield break; }

        UpdateCanvas();

        // Problem description : I want to Change UI, but can't change it, function() no work?
        // Object.Instantiate(UIObject, UICanvas.GetComponent<Transform>());
        // Error above: "UIObject" is original Object. Not a Runtime Clone Object!
        // We Should Assign UIObject with "=". Add a Clone Object to the Dictionary<>!
        UIObject = Object.Instantiate(UIObject, UICanvas.GetComponent<Transform>());
        // Debug.Log(string.Format("UIObject name is {0}", UIObject.name));
        // ObjectState must be "LOADING" until now.(Awake())

        BaseUI baseUI = UIObject.GetComponent<BaseUI>(); // 多态
        if (null == baseUI) { yield break; }

        baseUI.UICurrState = ObjectState.READY;

        RecordUI(uIType, UIObject);
    }

    private void RecordUI(UIType uIType, GameObject UIObject)
    {
        if (null == UIObject || uIType == UIType.NONE) { return; }

        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        if (!UIRecord.ContainsKey(scene.name))
        {
            Stack<KeyValuePair<UIType, GameObject>> stack = new Stack<KeyValuePair<UIType, GameObject>>();
            stack.Push(new KeyValuePair<UIType, GameObject>(uIType, UIObject));
            UIRecord.Add(scene.name, stack);
        }
        else
        {
            Stack<KeyValuePair<UIType, GameObject>> stack = UIRecord[scene.name];
            KeyValuePair<UIType, GameObject> kv = new KeyValuePair<UIType, GameObject>(uIType, UIObject);
            if (stack.Contains(kv)) { return; }
            stack.Push(kv);
        }
    }

    public UIType GetPeekUIType()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!UIRecord.ContainsKey(scene.name)) { return UIType.NONE; }

        Stack<KeyValuePair<UIType, GameObject>> stack = UIRecord[scene.name];
        if (null == stack || stack.Count <= 0) { return UIType.NONE; }

        return stack.Peek().Key;
    }

    public GameObject GetPeekUI()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!UIRecord.ContainsKey(scene.name)) { return null; }

        Stack<KeyValuePair<UIType, GameObject>> stack = UIRecord[scene.name];
        if (null == stack || stack.Count <= 0) { return null; }

        return stack.Peek().Value;
    }

    public void SetUIState(object obj, ObjectState state)
    {
        if (UIState.ContainsKey(obj))
            UIState.Remove(obj);
        UIState.Add(obj, state);
    }

    public ObjectState GetUIState(object obj)
    {
        if (UIState.ContainsKey(obj))
            return UIState[obj];
        else
            return ObjectState.NONE;
    }

    public bool IsUIReady(UIType uIType)
    {
        UIType peekType = GetPeekUIType();
        if (peekType != uIType) { return false; }

        GameObject uiObject = GetPeekUI();
        if (null == uiObject) { return false; }

        ObjectState state = GetUIState(uiObject);
        if (state != ObjectState.READY) { return false; }

        return true;
    }
}
