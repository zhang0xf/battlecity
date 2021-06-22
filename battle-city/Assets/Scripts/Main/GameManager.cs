using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text m_MessageText;
    [SerializeField] private PlayerManager m_PlayerManager;
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject[] m_EnemyPrefab;
    [SerializeField] private Transform[] m_EnemySpawnPoints;
    [SerializeField] private int m_EnemyCount = 20;
    [SerializeField] private AudioClip m_StartMusic;
    [SerializeField] private AudioSource m_AudioSFX;
    [SerializeField] private Transform m_EnemyParent;
    [SerializeField] private GameObject m_Born;

    private int m_RoundNumber = 0;
    private Queue<EnemyManager> m_Queue;
    private static GameManager m_Instance = null;
    
    public static GameManager Instance
    {
        get { return m_Instance; }
    }

    private void Awake()
    {
        // Unity does not allow you to instantiate anything inheriting(继承) from the MonoBehaviour class using the "new" keyword.
        // MonoBehaviours are scripts that are attached to an object in the scene,
        // and run in the scene as long as the object they are attached to is active.
        // The concept of attaching things to objects is Unity specific, while the keyword new is general to C#,

        if (null == m_Instance)
        {
            m_Instance = this;  // never use new() in MonoBehaviour
        }

        m_Queue = new Queue<EnemyManager>();
    }

    private void Start()
    {
        Debug.Log("游戏启动！");
        Config.LoadConfig();
        UIManager.Instance.PreLoadResources();
        LevelManager.Instance.PreLoadResources();
        SpawnAllTanks();
        ChangeState(GameState.MENU);
        DontDestroyOnLoad(this);
    }

    public void LoadModule(Type type)
    {
        // Creates an instance of the specified(指定的) type using the constructor that best matches the specified parameters.
        BaseObject obj = Activator.CreateInstance(type) as BaseObject;
        obj.Load();
    }

    public IEnumerator GameStarting()
    {
        DisablePlayerControl();

        // display message
        m_RoundNumber++;
        m_MessageText.text = "Round " + m_RoundNumber.ToString();
        m_MessageText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(2.0f);

        // close message
        m_MessageText.gameObject.SetActive(false);

        // load level
        LevelManager.Instance.LoadLevel("level" + m_RoundNumber.ToString());

        // play music 
        m_AudioSFX.clip = m_StartMusic;
        m_AudioSFX.Play();

        // play animation
        m_Born = Instantiate(m_Born, m_PlayerManager.m_Instance.transform.position,
               m_PlayerManager.m_Instance.transform.rotation);
        
        // wait animation complete
        yield return new WaitForSeconds(4.6f);
        
        // close animation
        Destroy(m_Born, 0.0f);

        // display player
        m_PlayerManager.m_Instance.SetActive(true);

        EnablePlayerControl();
    }

    public void SpawnAllTanks()
    {
        m_PlayerManager.m_PlayerID = 0;

        // create player
        m_PlayerManager.m_Instance =
            Instantiate(m_PlayerPrefab, m_PlayerManager.m_SpawnPoint.position, m_PlayerManager.m_SpawnPoint.rotation);

        m_PlayerManager.Setup();
        m_PlayerManager.m_Instance.SetActive(false);

        DisablePlayerControl();

        // create enemy
        StartCoroutine(SpawnEnemys());
        
        DisableEnemyControl();
    }

    public IEnumerator SpawnEnemys()
    {
        for (int i = 0; i <= m_EnemyCount; i++)
        {
            m_Queue.Enqueue(CreateEnemy());
        }
        yield return null;
    }

    public EnemyManager CreateEnemy()
    {
        EnemyManager enemyManager = new EnemyManager();

        int id = UnityEngine.Random.Range(0, 3);

        // random enemy
        EnemyKind enemyKind = EnemyConfig.Instance.GetEnemyKind(id);
        if (null == enemyKind) { return null; }

        enemyManager.m_Form = enemyKind.Form;
        enemyManager.m_Speed = enemyKind.Speed;
        enemyManager.m_Cooling = enemyKind.Cooling;

        // random position
        enemyManager.m_SpawnPoint = m_EnemySpawnPoints[UnityEngine.Random.Range(0, m_EnemySpawnPoints.Length)];

        // random rotation
        Quaternion rotation =
            new Quaternion(enemyManager.m_SpawnPoint.rotation.x,
            enemyManager.m_SpawnPoint.rotation.y,
            enemyManager.m_SpawnPoint.rotation.z + UnityEngine.Random.Range(0, 4) * 90, 1);

        // Instantiate
        enemyManager.m_Instance = Instantiate(m_EnemyPrefab[id], enemyManager.m_SpawnPoint.position, rotation, m_EnemyParent);

        enemyManager.Setup();
        enemyManager.m_Instance.SetActive(false);

        return enemyManager;
    }

    public void EnablePlayerControl()
    {
        m_PlayerManager.EnableControl();
    }

    public void DisablePlayerControl()
    {
        m_PlayerManager.DisableControl();
    }

    public void EnableEnemyControl()
    {
        foreach(EnemyManager enemyManager in m_Queue)
        {
            enemyManager.EnableControl();
        }
    }

    public void DisableEnemyControl()
    {
        foreach (EnemyManager enemyManager in m_Queue)
        {
            enemyManager.DisableControl();
        }
    }

    public PlayerManager GetPlayerManager()
    {
        return m_PlayerManager;
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
                StateMachine.Instance.CurrState = NewGameState.Instance;
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
            case GameState.GAMEOVER:
                // StateMachine.Instance.CurrState = null;
                break;
            default:
                Debug.Log(string.Format("BaseUI.cs : can't chang to {0} state", state.ToString()));
                break;
        }
    }
}
