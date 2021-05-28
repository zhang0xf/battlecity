using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game mInstance = null;
    private StateMachine stateMachine = null;

    private Game() 
    {
        stateMachine = StateMachine.Instance;
    }

    public static Game Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        Debug.Log("游戏启动！");
        Init();
        stateMachine.ChangeState(GameState.MAIN_MENU);
        DontDestroyOnLoad(this);    // this : object that "Game.cs" attached to.
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 状态机自更新
        stateMachine.CurrState.OnUpdate();

        // 对象自更新
    }

    void FixedUpdate()
    {

        
    }


    private void Init()
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
}
