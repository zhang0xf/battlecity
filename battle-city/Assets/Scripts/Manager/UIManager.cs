using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager
{
    private static GameObject m_Canvas = null;
    private static UIManager m_Instance = null;
    private Dictionary<UIType, string> m_Path = null;   // 路径
    private Dictionary<UIType, GameObject> m_Resource = null;   // 加载完成,但未实例化的UI
    private Dictionary<UIType, ObjState> m_State = null;    // 状态（状态改变会自动更新，见:BaseUI.cs）
    private Dictionary<string, Stack<KeyValuePair<UIType, GameObject>>> m_Record = null; // 栈记录

    private UIManager()
    {
        m_Resource = new Dictionary<UIType, GameObject>();
        m_State = new Dictionary<UIType, ObjState>();
        m_Record = new Dictionary<string, Stack<KeyValuePair<UIType, GameObject>>>();
        
        m_Resource.Clear();
        m_State.Clear();
        m_Record.Clear();
        
        m_Path = UIConfig.Instance.GetRecord();
    }

    public static UIManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new UIManager();
            return m_Instance;
        }
    }

    public static void UpdateCanvas()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        m_Canvas = GameObject.Find("Canvas");
        if (null == m_Canvas)
        {
            m_Canvas = new GameObject();
            m_Canvas.name = "UIUICanvas";
            m_Canvas.AddComponent<Canvas>();
            m_Canvas.AddComponent<CanvasScaler>();
            m_Canvas.AddComponent<GraphicRaycaster>();

            m_Canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler canvasScaler = m_Canvas.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;
        }
    }

    public void PreLoadResources()
    {
        if (m_Path != null && m_Resource != null)
        {
            foreach (KeyValuePair<UIType, string> kv in m_Path)
            {
                GameObject obj = Resources.Load(kv.Value) as GameObject;    // 尚未Instantiate()
                if (null == obj) { continue; }
                if (!m_Resource.ContainsKey(kv.Key))
                {
                    m_Resource.Add(kv.Key, obj);
                }
            }
        }
    }

    public void OpenUI(UIType uIType, bool isPause)
    {
        if (IsUIReady(uIType)) { return; }

        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        if (isPause && m_Record[scene.name].Count == 0) { return; }

        if (isPause)
        {
            PauseUI(GetPeekUI());   // 暂停UI
        }
        else
        {
           Clear();   // 清空所有UI
        }

        UpdateCanvas();

        GameObject obj = null;

        if (!m_Resource.ContainsKey(uIType))    // 若资源尚未加载，则加载资源
        {
            obj = Resources.Load(m_Path[uIType]) as GameObject;
            if (null == obj)
            {
                Debug.LogError("can not find prefab");
                return;
            }
        }
        else 
        {
            obj = m_Resource[uIType];
        }

        // Problem description : I want to Change UI, but can't change it, function() no work?
        // Object.Instantiate(obj, UICanvas.GetComponent<Transform>());
        // Error above: "obj" is original Object. Not a Runtime Clone Object!
        // We Should Assign obj with "=". Add a Clone Object to the Dictionary<>!
        // Debug.Log(string.Format("obj name is {0}", obj.name));
        obj = Object.Instantiate(obj, m_Canvas.GetComponent<Transform>());

        BaseUI baseUI = obj.GetComponent<BaseUI>(); // 多态
        if (null == baseUI) { return; }

        baseUI.CurrState = ObjState.READY;

        PushUI(uIType, obj);
    }

    private void PushUI(UIType uIType, GameObject obj)
    {
        if (null == obj || uIType == UIType.NONE) { return; }

        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        if (!m_Record.ContainsKey(scene.name))
        {
            Stack<KeyValuePair<UIType, GameObject>> stack = new Stack<KeyValuePair<UIType, GameObject>>();
            stack.Push(new KeyValuePair<UIType, GameObject>(uIType, obj));
            m_Record.Add(scene.name, stack);
        }
        else
        {
            Stack<KeyValuePair<UIType, GameObject>> stack = m_Record[scene.name];
            KeyValuePair<UIType, GameObject> kv = new KeyValuePair<UIType, GameObject>(uIType, obj);
            if (stack.Contains(kv)) { return; }
            stack.Push(kv);
        }
    }

    public void PopUI(UIType uIType)
    {
        if (uIType == UIType.NONE) { return; }

        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        if (m_Record.ContainsKey(scene.name) && 
            m_Record[scene.name].Count != 0 &&
            uIType == GetPeekUIType())
        {
            GameObject peek = GetPeekUI();
            ReleaseUI(peek);
            m_Record[scene.name].Pop();
        }
    }

    public UIType GetPeekUIType()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!m_Record.ContainsKey(scene.name)) { return UIType.NONE; }

        Stack<KeyValuePair<UIType, GameObject>> stack = m_Record[scene.name];
        if (null == stack || stack.Count <= 0) { return UIType.NONE; }

        return stack.Peek().Key;
    }

    public GameObject GetPeekUI()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return null; }
        if (!m_Record.ContainsKey(scene.name)) { return null; }

        Stack<KeyValuePair<UIType, GameObject>> stack = m_Record[scene.name];
        if (null == stack || stack.Count <= 0) { return null; }

        return stack.Peek().Value;
    }

    public void SetUIState(object obj, ObjState state)
    {
        if (null == obj) { return; }

        UIType uIType = (UIType)obj;

        if (m_State.ContainsKey(uIType))
            m_State.Remove(uIType);
        m_State.Add(uIType, state);
    }

    public ObjState GetUIState(UIType uIType)
    {
        if (uIType == UIType.NONE) { return ObjState.NONE; }

        if (m_State.ContainsKey(uIType))
            return m_State[uIType];
        else
            return ObjState.NONE;
    }

    public bool IsUIReady(UIType uIType)
    {
        if (uIType == UIType.NONE) { return false; }

        if (m_State.ContainsKey(uIType))
            return m_State[uIType] == ObjState.READY;
        else
            return false;
    }

    public void PauseUI(GameObject obj)
    {
        if (null == obj) { return; }

        BaseUI ui = obj.GetComponent<BaseUI>(); // 多态
        if (null == ui) { return; }

        ui.OnPause();
    }

    public void ResumeUI(GameObject obj)
    {
        if (null == obj) { return; }

        BaseUI ui = obj.GetComponent<BaseUI>(); // 多态
        if (null == ui) { return; }

        ui.OnResume();
    }

    public void ReleaseUI(GameObject obj)
    {
        if (null == obj) { return; }

        BaseUI ui = obj.GetComponent<BaseUI>(); // 多态
        if (null == ui) { return; }

        ui.OnRelease();
    }

    public bool IsResume()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return false; }

        if (m_Record[scene.name].Count == 0) { return false; }

        UIType uIType = GetPeekUIType();

        ObjState uIState = GetUIState(uIType);

        if (uIState != ObjState.INVALID)
        {
            Debug.Log(string.Format("UI state error : UIType is {0}, UI State is {1}", uIType.ToString(), uIState.ToString()));
            return false;
        }
        return true;
    }

    public void Clear()
    {
        m_State.Clear();

        Scene scene = SceneManager.GetActiveScene();
        if (null == scene) { return; }

        if (m_Record.ContainsKey(scene.name))
        {
            while (m_Record[scene.name].Count != 0)
            {
                GameObject obj = m_Record[scene.name].Pop().Value;
                if (null == obj) { continue; }

                BaseUI ui = obj.GetComponent<BaseUI>(); // 多态
                if (null == ui) { continue; }

                ui.OnRelease();
            }
        }

        m_Record.Clear();
    }
}
