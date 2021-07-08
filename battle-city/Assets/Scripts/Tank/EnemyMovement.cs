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
    public Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>> m_MergePath;

    private Vector2 m_From;
    private Vector2 m_Check;
    private Vector2 m_To;
    private Vector2 m_Direction;
    private float m_Distance;

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
        // 允许和目标之间的误差(浮点计算一定有误差)
        if (Vector2.Distance(m_Goal, m_Rigidbody.position) < 0.05)
        {
            IsPathFinding = false;
        }

        m_From = m_Rigidbody.position;

        if (!IsPathFinding)
        {
            // 首次执行函数
            if (null == m_MergePath || m_MergePath.Count == 0) { return; }
            IsPathFinding = true;
            var kv = m_MergePath.Peek();
            m_To = kv.Value.Key;
            m_Direction = (m_To - m_From).normalized;
        }

        // 每帧更新
        m_Distance = Vector2.Distance(m_To, m_From);

        // 确保不会越过目标
        if (m_Distance > 0.03 && (Vector2.Dot(m_From, m_Direction) < Vector2.Dot(m_To, m_Direction)))
        {
            Vector2 movement = m_Direction * m_EnemyInfo.Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }
        else
        {
            if (null == m_MergePath || m_MergePath.Count == 0) { return; }
            var kv = m_MergePath.Dequeue();
            m_Check = kv.Value.Key;

            // 校准误差（由于存在大量正方形的碰撞体，若不校准误差会被边边角角卡住）
            if (Vector2.Distance(m_Rigidbody.position, m_Check) > 0.03)
            {
                m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Check - m_Rigidbody.position));
            }
            m_To = kv.Value.Value;
            m_Direction = kv.Key;
        }
    }

    // 合并方向相同的路径点：解决移动停顿的问题，使只在转向时才停顿。（有无更好的解决方案？）
    public Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>> MergePath(Stack<Vector2> path)
    {
        if (null == path) { return null; }

        Queue<Vector2> pathClone = new Queue<Vector2>(path.ToArray());

        Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>> queue
            = new Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>>();

        Vector2 from = pathClone.Dequeue();
        Vector2 directRefer = (pathClone.Peek() - from).normalized;  // normalized:单位向量
        Stack<Vector2> record = new Stack<Vector2>();

        while (pathClone.Count != 0)
        {
            Vector2 to = pathClone.Peek();
            Vector2 needCheck = (to - from).normalized;

            if (Vector3.Dot(directRefer, needCheck) == 1) // 判断向量是否方向相同
            {
                record.Push(pathClone.Dequeue()); 
            }
            else
            {
                Vector2 last = record.Pop();
                KeyValuePair<Vector2, Vector2> endPoints = new KeyValuePair<Vector2, Vector2>(from, last);
                queue.Enqueue(new KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>((last - from).normalized, endPoints));
                from = last;
                directRefer = (pathClone.Peek() - from).normalized;
            }
        }

        // 最后一道转折路线需要单独添加
        if (record.Count != 0)
        {
            Vector2 last = record.Pop();
            KeyValuePair<Vector2, Vector2> endPoints = new KeyValuePair<Vector2, Vector2>(from, last);
            queue.Enqueue(new KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>((last - from).normalized, endPoints));
        }

        return queue;
    }

    private void SetAnimation(Vector2 direction)
    {
        if (direction.x > 0) { m_Animator.SetBool("Right", true); }
        if (direction.x < 0) { m_Animator.SetBool("Left", true); }
        if (direction.y < 0) { m_Animator.SetBool("Down", true); }
        if (direction.y > 0) { m_Animator.SetBool("UP", true); }
    }
}
