using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameState : StateBase
{
    private static NewGameState m_Instance = null;

    private NewGameState()
    {
        m_GameState = GameState.MENU;
    }

    public static NewGameState Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new NewGameState();
            return m_Instance;
        }
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"NewGameState\"."));
        GameManager.Instance.StartCoroutine(GameManager.Instance.GameStarting());
        base.OnEnter();
    }

    public override void OnExcute()
    {
        base.OnExcute();
    }

    public override void OnLeave()
    {
        Debug.Log(string.Format("Leave \"NewGameState\"."));
        base.OnLeave();
    }
}
