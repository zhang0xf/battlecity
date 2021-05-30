using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// 使用MessageController降低四个模块的耦合度：
// 1. 输入模块：Command
// 2. GUI模块：BaseUI
// 3. 状态机模块：StateBase
// 4. 场景模块：BaseScene

public class Game : MonoBehaviour
{
    private static Game mInstance = null;
    private BaseScene mCurrScene = null;

    public static Game Instance
    {
        get { return mInstance; }
    }

    public BaseScene CurrScene
    {
        set { mCurrScene = value; }
        get { return mCurrScene; }
    }

    void Awake()
    {
        Debug.Log("游戏启动！");
        Load();
        StartCoroutine(StartGame());
        DontDestroyOnLoad(this);// object which "Game.cs" attached to.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 场景更新
        if (CurrScene != null)
        {
            CurrScene.Update();
        }
    }

    void FixedUpdate()
    {
        
    }

    private void Load()
    {
        if (null == mInstance)
        {
            mInstance = this;
            // Unity does not allow you to instantiate anything inheriting(继承) from the MonoBehaviour class using the "new" keyword.
            // MonoBehaviours are scripts that are attached to an object in the scene, and run in the scene as long as the object they are attached to is active.
            // The concept of attaching things to objects is Unity specific, while the keyword new is general to C#,
            // How to specify what to attach it to?
            // Method : GameObject.AddComponent(). What this does is create a new script, of type T, and add it to the specified GameObject.
            // MyScript script = new Script();
            // MyScript script = obj.AddComponent<MyScript>();
        }
        Config.LoadConfig();
        LoadModule(typeof(Player));
    }

    private void LoadModule(Type type)
    {
        // CreateInstance() : Creates an instance of the specified type using the constructor that best matches the specified parameters.
        BaseObject obj = Activator.CreateInstance(type) as BaseObject;
        obj.Load();
    }

    private IEnumerator StartGame()
    {
        SceneManager.LoadScene(SceneName.MENU);
        UIManager.Instance.OpenUI(UIType.MAIN_MENU_UI);
        yield return null;
    }

    public void CreateCurrScene(string sceneName, Type type)
    {
        StartCoroutine(CreateCurrSceneAsynchronous(sceneName, type));    
    }

    private IEnumerator CreateCurrSceneAsynchronous(string sceneName, Type type)
    {
        string activeName = SceneManager.GetActiveScene().name;

        if (!activeName.Equals(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }

        if (CurrScene != null)
        {
            if (CurrScene.GetType() == type) { yield break; }
            CurrScene.Release();
            CurrScene = null;
        }
        
        CurrScene = Activator.CreateInstance(type) as BaseScene;
        CurrScene.Load();   // CurrState is READY
    }
}
