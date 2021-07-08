using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text m_MessageText;
    [SerializeField] private Image m_GameOverImage;
    [SerializeField] private PlayerManager m_PlayerManager;
    [SerializeField] private Transform[] m_SpawnPoints;
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private int m_EnemyCount;
    [SerializeField] private AudioClip m_StartMusic;
    [SerializeField] private AudioSource m_AudioSFX;
    [SerializeField] private Transform m_EnemyPool;
    [SerializeField] private GameObject m_Born;

    private GameObject m_CurrLevel;
    private event GameObjectChangeHandler m_Handler;
    private int m_RoundNumber = 0;
    private List<EnemyManager> m_List;  // 活跃着的坦克
    private Queue<EnemyManager> m_Queue;    // 预备坦克
    private static GameManager m_Instance = null;
    
    public static GameManager Instance
    {
        get { return m_Instance; }
    }

    public GameObject CurrLevel
    {
        set 
        {
            if (m_CurrLevel != value)
            {
                m_CurrLevel = value;
                m_Handler(this, m_CurrLevel);
            }
        }
        get { return m_CurrLevel; }
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
        m_List = new List<EnemyManager>();
        m_Handler += HandleCurrLevelChange;
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
        // Creates an instance of the specified(指定的) type using the constructor that best matches the specified parameters.
        BaseObject obj = Activator.CreateInstance(type) as BaseObject;
        obj.Load();
    }

    public IEnumerator GameStarting()
    {
        // 禁用操作
        DisablePlayerControl();
        DisableEnemyControl();

        // 关闭GameOver图片
        if (m_GameOverImage.IsActive())
        {
            m_GameOverImage.gameObject.SetActive(false);
        }

        // 显示回合信息
        if (StateMachine.Instance.CurrState == NewGameState.Instance)
        {
            m_RoundNumber = 0;
        }
        m_RoundNumber++;
        m_MessageText.text = "Round " + m_RoundNumber.ToString();
        m_MessageText.gameObject.SetActive(true);

        // 2秒之后关闭信息
        yield return new WaitForSeconds(2.0f);
        m_MessageText.gameObject.SetActive(false);

        SpawnAllTanks();

        // 加载关卡（只有一关）
        CurrLevel = LevelManager.Instance.LoadLevel("level1");

        // 播放开始音乐
        m_AudioSFX.clip = m_StartMusic;
        m_AudioSFX.Play();

        // 生成敌人
        StartCoroutine(GenerateEnemy());

        // 生成玩家
        GameObject playerBorn = Instantiate(m_Born, m_PlayerManager.m_Instance.transform.position,
               m_PlayerManager.m_Instance.transform.rotation);
        yield return new WaitForSeconds(4.6f);
        Destroy(playerBorn, 0.0f);
        m_PlayerManager.m_Instance.SetActive(true);

        // 允许操作
        EnablePlayerControl();
        EnableEnemyControl();
    }

    public IEnumerator GameOvering()
    {
        DisablePlayerControl();
        DisableEnemyControl();

        // 显示GameOver图片
        m_GameOverImage.gameObject.SetActive(true);

        // 等待3秒
        yield return new WaitForSeconds(3.0f);

        // 销毁坦克
        if (m_PlayerManager.m_Instance != null)
            Destroy(m_PlayerManager.m_Instance, 0.0f);

        foreach (var enemyManager in m_Queue)
        {
            if (enemyManager.m_Instance != null)
                Destroy(enemyManager.m_Instance, 0.0f);
        }
        m_Queue.Clear();

        foreach (var enemyManager in m_List)
        {
            if (enemyManager.m_Instance != null)
                Destroy(enemyManager.m_Instance, 0.0f);
        }
        m_List.Clear();

        // 销毁关卡
        Destroy(m_CurrLevel, 0.0f);

        // 切换到主界面
        ChangeState(GameState.MENU);

        yield return null;
    }

    public IEnumerator GenerateEnemy()
    {
        if (m_Queue.Count == 0)
        {
            // 赢了也走输掉的流程，这块不重要！
            StartCoroutine(GameOvering());
            ChangeState(GameState.NEWGAME);
            yield break;
        }

        EnemyManager enemyManager = m_Queue.Dequeue();
        if (null == enemyManager) { yield break; }

        m_List.Add(enemyManager);

        GameObject enemyBorn = Instantiate(m_Born, enemyManager.m_Instance.transform.position,
              enemyManager.m_Instance.transform.rotation);
        yield return new WaitForSeconds(4.6f);
        Destroy(enemyBorn, 0.0f);
        enemyManager.m_Instance.SetActive(true);

        yield return null;
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
            EnemyManager enemyManager = new EnemyManager();

            // 随机敌人
            enemyManager.m_EnemyKind = UnityEngine.Random.Range(0, 3);

            // 随机位置
            enemyManager.m_SpawnPoint = m_SpawnPoints[UnityEngine.Random.Range(0, m_SpawnPoints.Length)];

            // 生成敌人
            enemyManager.m_Instance = 
                Instantiate(m_EnemyPrefab, enemyManager.m_SpawnPoint.position, enemyManager.m_SpawnPoint.rotation, m_EnemyPool);

            // 设置敌人
            enemyManager.Setup();
            enemyManager.m_Instance.SetActive(false);

            m_Queue.Enqueue(enemyManager);
        }

        yield return null;
    }

    private void HandleCurrLevelChange(object sender, GameObject level)
    {
        if (null == sender || null == level) { return; }

        Notification na = new Notification(NotificationName.LEVEL_CHANGE, this);
        na.Content = level;
        na.Send();
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
                StateMachine.Instance.CurrState = GameOverState.Instance;
                break;
            default:
                Debug.Log(string.Format("BaseUI.cs : can't chang to {0} state", state.ToString()));
                break;
        }
    }

    public void EnablePlayerControl()
    {
        if (m_PlayerManager.m_Instance != null)
            m_PlayerManager.EnableControl();
    }

    public void DisablePlayerControl()
    {
        if (m_PlayerManager.m_Instance != null)
            m_PlayerManager.DisableControl();
    }

    public void EnableEnemyControl()
    {
        foreach (EnemyManager enemyManager in m_Queue)
        {
            if(enemyManager.m_Instance != null && enemyManager.m_Instance.activeSelf)
                enemyManager.EnableControl();
        }
    }

    public void DisableEnemyControl()
    {
        foreach (EnemyManager enemyManager in m_Queue)
        {
            if (enemyManager.m_Instance != null && enemyManager.m_Instance.activeSelf)
                enemyManager.DisableControl();
        }
    }
}
