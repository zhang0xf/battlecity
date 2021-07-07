using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_Rigidbody;
    [SerializeField] private AudioClip m_EngineIdle;
    [SerializeField] private AudioClip m_EngineDriving;
    [SerializeField] private AudioSource m_AudioDriving;
    
    public TankInfo m_EnemyInfo;
    public Dictionary<Location, Location> m_ComeFrom;   // 路径
    public Dictionary<Location, double> m_CostSoFar;    // 路径花费

    public void SetEnemyInfo(int kind)
    {
        m_EnemyInfo = TankConfig.Instance.GetEnemyInfo(kind);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    { 
        
    }

    private Direction GetDirection()
    {
        AnimatorStateInfo animStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.IsName("EnemyUp"))
        {
            return Direction.UP;
        }
        else if (animStateInfo.IsName("EnemyLeft"))
        {
            return Direction.LEFT;
        }
        else if (animStateInfo.IsName("EnemyDown"))
        {
            return Direction.DOWN;
        }
        else if (animStateInfo.IsName("EnemyRight"))
        {
            return Direction.RIGHT;
        }

        return Direction.NONE;
    }
}
