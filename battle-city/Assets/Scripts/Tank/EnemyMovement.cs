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

    public Vector2 m_Goal;
    public Stack<Vector2> m_Path;
    public bool IsPathFinding = false;
    public Dictionary<Vector2, double> m_CostSoFar;

    private void Awake()
    {
        m_AudioDriving.clip = m_EngineIdle;
    }

    public void SetEnemyInfo(int kind)
    {
        m_EnemyInfo = TankConfig.Instance.GetEnemyInfo(kind);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (Vector2.Distance(m_Goal, m_Rigidbody.position) < 0.05) // 两个坐标不会完全相等，大约有0.016左右的误差
        {
            IsPathFinding = false;
        }

        if (null == m_Path || m_Path.Count == 0) { return; }

        IsPathFinding = true;

        Vector2 start = m_Rigidbody.position;

        Vector2 to = m_Path.Peek();

        Vector2 direction = to - start;

        float distance = Vector2.Distance(to, start);

        if (distance > 0.05)
        {
            Vector2 movement = direction * m_EnemyInfo.Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
        else
        {
            m_Path.Pop();
        }

    }

    private void SetAnimation(Vector2 direction)
    {
        if (direction.x > 0) { m_Animator.SetBool("Right", true); }
        if (direction.x < 0) { m_Animator.SetBool("Left", true); }
        if (direction.y < 0) { m_Animator.SetBool("Down", true); }
        if (direction.y > 0) { m_Animator.SetBool("UP", true); }
    }
}
