public class StateMachine : IUpdate
{
    private StateBase m_LastState = null;
    private StateBase m_CurrState = null;
    private static StateMachine m_Instance = null;

    private StateMachine() { }

    public static StateMachine Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new StateMachine();
            return m_Instance;
        }
    }

    public StateBase CurrState
    {
        get { return m_CurrState; }

        set 
        {
            if (m_CurrState != value)
            {
                if (m_LastState != null)
                    m_LastState.OnLeave();
                m_LastState = m_CurrState;
                m_CurrState = value;
                m_CurrState.OnEnter();
            }
        }
    }

    public void Update()
    {
        CurrState.OnExcute();
    }
}
