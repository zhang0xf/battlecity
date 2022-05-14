using UnityEngine;

public class MenuState : StateBase
{
    private static MenuState m_Instance = null;

    private MenuState()
    {
        m_GameState = GameState.MENU;
    }

    public static MenuState Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new MenuState();
            return m_Instance;
        }
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"MenuState\"."));
        UIManager.Instance.Clear();
        UIManager.Instance.OpenUI(UIType.MAIN_MENU_UI, false);
        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        Debug.Log(string.Format("Leave \"MenuState\"."));
        UIManager.Instance.Clear();
        base.OnLeave();
    }
}
