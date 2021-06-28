using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text m_MessageText;
    [SerializeField] private PlayerManager m_PlayerManager;
    [SerializeField] private EnemyManager m_EnemyManager;
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private int m_EnemyCount = 20;
    [SerializeField] private AudioClip m_StartMusic;
    [SerializeField] private AudioSource m_AudioSFX;
    [SerializeField] private Transform m_EnemyPool;
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
        // 禁用操作
        DisablePlayerControl();
        DisableEnemyControl();

        // 显示回合信息
        m_RoundNumber++;
        m_MessageText.text = "Round " + m_RoundNumber.ToString();
        m_MessageText.gameObject.SetActive(true);

        // 2秒之后关闭信息
        yield return new WaitForSeconds(2.0f);
        m_MessageText.gameObject.SetActive(false);

        // 加载关卡
        LevelManager.Instance.LoadLevel("level" + m_RoundNumber.ToString());

        // 播放开始音乐
        m_AudioSFX.clip = m_StartMusic;
        m_AudioSFX.Play();

        // 获取一个敌人
        EnemyManager enemyManager = m_Queue.Dequeue();
        if (null == enemyManager) { yield break; }

        // 播放出生动画
        GameObject playerBorn = Instantiate(m_Born, m_PlayerManager.m_Instance.transform.position,
               m_PlayerManager.m_Instance.transform.rotation);
        GameObject enemyBorn = Instantiate(m_Born, enemyManager.m_Instance.transform.position,
              enemyManager.m_Instance.transform.rotation);

        // 等待动画播放完成
        yield return new WaitForSeconds(4.6f);
        Destroy(playerBorn, 0.0f);
        Destroy(enemyBorn, 0.0f);

        // 玩家和敌人设置为可见
        m_PlayerManager.m_Instance.SetActive(true);
        enemyManager.m_Instance.SetActive(true);

        // 允许操作
        EnablePlayerControl();
        EnableEnemyControl();
    }

    public void SpawnAllTanks()
    {
        // 创建玩家
        m_PlayerManager.m_PlayerLevel = 0;
        m_PlayerManager.m_Instance =
            Instantiate(m_PlayerPrefab, m_PlayerManager.m_SpawnPoint.position, m_PlayerManager.m_SpawnPoint.rotation);

        // 设置玩家
        m_PlayerManager.Setup();
        m_PlayerManager.m_Instance.SetActive(false);

        // 禁用操作
        DisablePlayerControl();

        // 创建敌人
        StartCoroutine(SpawnEnemys());

        // 禁用操作
        DisableEnemyControl();
    }

    public IEnumerator SpawnEnemys()
    {
        for (int i = 0; i <= m_EnemyCount; i++)
        {
            // 随机敌人
            m_EnemyManager.m_EnemyKind = UnityEngine.Random.Range(0, 3);

            // 随机位置
            m_EnemyManager.m_SpawnPoint = 
                m_EnemyManager.m_EnemySpawnPoints[UnityEngine.Random.Range(0, m_EnemyManager.m_EnemySpawnPoints.Length - 1)];

            // 生成敌人
            m_EnemyManager.m_Instance = 
                Instantiate(m_EnemyPrefab, m_EnemyManager.m_SpawnPoint.position, m_EnemyManager.m_SpawnPoint.rotation, m_EnemyPool);

            // 设置敌人
            m_EnemyManager.Setup();
            m_EnemyManager.m_Instance.SetActive(false);

            m_Queue.Enqueue(m_EnemyManager);
        }

        yield return null;
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
        foreach (EnemyManager enemyManager in m_Queue)
            enemyManager.EnableControl();
    }

    public void DisableEnemyControl()
    {
        foreach (EnemyManager enemyManager in m_Queue)
            enemyManager.DisableControl();
    }
}
