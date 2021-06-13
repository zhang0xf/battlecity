using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Unity does not allow you to instantiate anything inheriting(继承) from the MonoBehaviour class using the "new" keyword.
    // MonoBehaviours are scripts that are attached to an object in the scene, and run in the scene as long as the object they are attached to is active.
    // The concept of attaching things to objects is Unity specific, while the keyword new is general to C#,

    public Text m_MessageText;
    public PlayerManager[] m_Players;

    private static GameManager m_Instance = null;
    
    public static GameManager Instance
    {
        get { return m_Instance; }
    }

    private void Awake()
    {
        if (null == m_Instance)
        {
            m_Instance = this;  // never use new() in MonoBehaviour
        }
    }

    private void Start()
    {
        Debug.Log("游戏启动！");
        Config.LoadConfig();
        UIManager.Instance.PreLoadResources();
        LevelManager.Instance.PreLoadResources();
        ChangeState(GameState.MENU);
        DontDestroyOnLoad(this);
    }

    public void LoadModule(Type type)
    {
        //Creates an instance of the specified(指定的) type using the constructor that best matches the specified parameters.
        BaseObject obj = Activator.CreateInstance(type) as BaseObject;
        obj.Load();
    }

    public void ChangeState(GameState state)
    {
        if (state == GameState.NONE) { return; }

        switch (state)
        {
            case GameState.MENU:
                StateMachine.Instance.CurrState = MenuState.Instance;
                break;
            case GameState.NEWGAME:
                // StateMachine.Instance.CurrState = null;
                break;
            case GameState.CONTINUE:
                // StateMachine.Instance.CurrState = null;
                break;
            case GameState.CUSTOMIZE:
                // StateMachine.Instance.CurrState = null;
                break;
            case GameState.ONLINE:
                // StateMachine.Instance.CurrState = null;
                break;
            case GameState.EXIT:
                // StateMachine.Instance.CurrState = null;
                break;
            default:
                Debug.Log(string.Format("BaseUI.cs : can't chang to {0} state", state.ToString()));
                break;
        }
    }
}
