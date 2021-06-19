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
        GameManager.Instance.StartCoroutine(GameStarting());
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

    private IEnumerator GameStarting()
    {
        GameManager.Instance.DisablePlayerControl();
        LevelManager.Instance.LoadLevel("level0");
        GameManager.Instance.DisplayMessageText();
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.PlayStartMusic();
        GameManager.Instance.UnDisplayMessageText();
        PlayerManager playerManager = GameManager.Instance.GetPlayerManager();
        GameManager.Instance.BornAnimation(true);
        yield return new WaitForSeconds(4.6f);
        GameManager.Instance.BornAnimation(false);
        playerManager.m_Instance.SetActive(true);
        GameManager.Instance.EnablePlayerControl();
    }
}
