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
        // Ѱ·�����������Ŀ��֮������(�������һ�������)
        if (Vector2.Distance(m_Goal, m_Rigidbody.position) < 0.07)
        {
            IsPathFinding = false;
        }

        // ÿ֡����
        m_From = m_Rigidbody.position;

        if (!IsPathFinding)
        {
            // �״�ִ�к���
            if (null == m_MergePath || m_MergePath.Count == 0) { return; }
            IsPathFinding = true;
            var kv = m_MergePath.Peek();
            m_To = kv.Value.Key;
            m_Direction = (m_To - m_From).normalized;
        }

        // ÿ֡����
        m_Distance = Vector2.Distance(m_To, m_From);

        // �����С��Ҫȷ������Խ��Ŀ��
        if (m_Distance > 0.03 && (Vector2.Dot(m_From, m_Direction) < Vector2.Dot(m_To, m_Direction)))
        {
            Vector2 movement = m_Direction * m_EnemyInfo.Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
            SetAnimation(m_Direction);
            // m_AudioDriving.clip = m_EngineDriving;
            // m_AudioDriving.Play();   // �е㳳
        }
        else
        {
            if (null == m_MergePath || m_MergePath.Count == 0) { return; }
            var kv = m_MergePath.Dequeue();
            m_Check = kv.Value.Key;

            // У׼�����ڴ��ڴ��������ε���ײ�壬����У׼���ᱻ�߽߱ǽǿ�ס��
            if (Vector2.Distance(m_Rigidbody.position, m_Check) > 0.03)
            {
                m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Check - m_Rigidbody.position));
            }
            m_To = kv.Value.Value;
            m_Direction = kv.Key;
        }
    }

    // �ϲ�������ͬ��·���㣺����ƶ�ͣ�ٵ����⣬ʹֻ��ת��ʱ��ͣ�١������޸��õĽ����������
    public Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>> MergePath(Stack<Vector2> path)
    {
        if (null == path) { return null; }

        Queue<Vector2> pathClone = new Queue<Vector2>(path.ToArray());

        Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>> queue
            = new Queue<KeyValuePair<Vector2, KeyValuePair<Vector2, Vector2>>>();

        if (pathClone.Count == 0) { return null; }

        Vector2 from = pathClone.Dequeue();
        Vector2 directRefer = (pathClone.Peek() - from).normalized;  // normalized:��λ����
        Stack<Vector2> record = new Stack<Vector2>();

        while (pathClone.Count != 0)
        {
            Vector2 to = pathClone.Peek();
            Vector2 needCheck = (to - from).normalized;

            if (Vector3.Dot(directRefer, needCheck) == 1) // �ж������Ƿ�����ͬ
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

        // ���һ��ת��·����Ҫ�������
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
        if (direction.x > 0 && direction.y == 0)
        {
            m_Animator.SetBool("Right", true);
            m_Animator.SetBool("Left", false);
            m_Animator.SetBool("Down", false);
            m_Animator.SetBool("Up", false);
        }
        
        if (direction.x < 0 && direction.y == 0) 
        {
            m_Animator.SetBool("Left", true);
            m_Animator.SetBool("Right", false);
            m_Animator.SetBool("Down", false);
            m_Animator.SetBool("Up", false);
        }
        
        if (direction.y < 0 && direction.x == 0) 
        { 
            m_Animator.SetBool("Down", true);
            m_Animator.SetBool("Right", false);
            m_Animator.SetBool("Left", false);
            m_Animator.SetBool("Up", false);
        }
        
        if (direction.y > 0 && direction.x == 0)
        {
            m_Animator.SetBool("Up", true);
            m_Animator.SetBool("Right", false);
            m_Animator.SetBool("Left", false);
            m_Animator.SetBool("Down", false);
        }
    }
}
