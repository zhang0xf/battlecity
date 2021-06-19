using System;
using UnityEngine;

[Serializable]
public class EnemyManager
{
    public Transform m_SpawnPoint;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public string m_Form;
    [HideInInspector] public float m_Speed;
    [HideInInspector] public float m_Cooling;

   /* private TankMovement m_Movement;
    private TankShooting m_Shooting;
*/
    public void Setup()
    {
       /* m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();*/
    }

    public void DisableControl()
    {
       /* m_Movement.enabled = false;
        m_Shooting.enabled = false;*/
    }

    public void EnableControl()
    {
       /* m_Movement.enabled = true;
        m_Shooting.enabled = true;*/
    }

    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
