using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerManager
{
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerID;
    [HideInInspector] public GameObject m_Instance;

    private PlayerMovement m_Movement;
    private PlayerShooting m_Shooting;

    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<PlayerMovement>();
        m_Shooting = m_Instance.GetComponent<PlayerShooting>();
        m_Movement.SetPlayerStatus(m_PlayerID);
        m_Shooting.SetPlayerStatus(m_PlayerID);
    }

    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
    }

    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
