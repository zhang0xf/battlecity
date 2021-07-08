using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : StateBase
{
    private static GameOverState m_Instance = null;

    private GameOverState()
    {
        m_GameState = GameState.MENU;
    }

    public static GameOverState Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new GameOverState();
            return m_Instance;
        }
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"GameOverState\"."));
        GameManager.Instance.StartCoroutine(GameManager.Instance.GameOvering());
        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        Debug.Log(string.Format("Leave \"GameOverState\"."));
        base.OnLeave();
    }
}
