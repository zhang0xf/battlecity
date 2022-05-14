public class StateBase
{
    public GameState m_GameState;

    public virtual void OnEnter() { }

    public virtual void OnExcute() { }

    public virtual void OnLeave() { }
}