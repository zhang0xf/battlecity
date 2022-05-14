using System;
using UnityEngine;

[Serializable]
public class EnemyManager
{
    [HideInInspector] public Transform m_SpawnPoint;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public int m_EnemyKind;

    private EnemyMovement m_Movement;
    private EnemyShooting m_Shooting;
    private EnemyAI m_EnemyAI;

    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<EnemyMovement>();
        m_Shooting = m_Instance.GetComponent<EnemyShooting>();
        m_EnemyAI = m_Instance.GetComponent<EnemyAI>();
        m_Movement.SetEnemyInfo(m_EnemyKind);
        m_Shooting.SetEnemyInfo(m_EnemyKind);
    }

    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
        m_EnemyAI.enabled = false;
    }

    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;
        m_EnemyAI.enabled = true;
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
