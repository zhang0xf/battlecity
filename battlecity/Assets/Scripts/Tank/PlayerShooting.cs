using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Transform m_FirePosition;
    [SerializeField] private Rigidbody2D m_BulletUp;
    [SerializeField] private Rigidbody2D m_BulletDown;
    [SerializeField] private Rigidbody2D m_BulletLeft;
    [SerializeField] private Rigidbody2D m_BulletRight;
    [SerializeField] private AudioClip m_FireClip;
    [SerializeField] private AudioSource m_AudioSFX;
    [SerializeField] private float m_FireForce = 24.0f;
    [SerializeField] private int m_BulletCount = 20;

    private Dictionary<Direction, Queue<Rigidbody2D>> m_Bullets;

    private bool IsFiring;  // we have a shoot cooling down
    private bool m_FireButtonPressedThisFrame;
    private bool m_FireButtonReleasedThisFrame;

    private TankInfo m_PlayerInfo;

    private void Awake()
    {
        // m_Bullets = new Dictionary<Direction, Queue<Rigidbody2D>>();
        // StartCoroutine(GenerateBullets());
    }

    private void Update()
    {
        if (m_FireButtonPressedThisFrame && !m_FireButtonReleasedThisFrame && !IsFiring)
        {
            StartCoroutine(Fire());
        }
    }

    private IEnumerator GenerateBullets()
    {
        CreateBullets(Direction.UP, m_BulletCount);
        CreateBullets(Direction.DOWN, m_BulletCount);
        CreateBullets(Direction.LEFT, m_BulletCount);
        CreateBullets(Direction.RIGHT, m_BulletCount);

        yield return null;
    }

    private Queue<Rigidbody2D> CreateBullets(Direction direction, int count)
    {
        if (direction == Direction.NONE) { return null; }

        Queue<Rigidbody2D> queue = new Queue<Rigidbody2D>();

        Rigidbody2D bullet; 

        for (int i = 0; i < count; i++)
        {
            bullet = GetBulletPrefab(direction);
            if (bullet == null) { continue; }
            bullet = Instantiate(bullet, m_FirePosition.position, m_FirePosition.rotation, gameObject.transform);
            bullet.gameObject.SetActive(false);
            queue.Enqueue(bullet);
        }

        if (!m_Bullets.ContainsKey(direction))
        {
            m_Bullets.Add(direction, queue);
        }

        return queue;
    }

    private Rigidbody2D GetBullet(Direction direction)
    {
        if (direction == Direction.NONE) { return null; }

        Rigidbody2D bullet = GetBulletPrefab(direction);
        if (null == bullet) { return null; }
        bullet = Instantiate(bullet, m_FirePosition.position, m_FirePosition.rotation, gameObject.transform);
        return bullet;

#if ABANDON
        if (m_Bullets.ContainsKey(direction))
        {
            Queue<Rigidbody2D> queue = m_Bullets[direction];

            if (queue.Count == 0) { return null; }

            if (!queue.Peek().gameObject.activeSelf)
            {
                Rigidbody2D bullet = queue.Dequeue();
                // existed reset position!
                bullet.gameObject.transform.position = m_FirePosition.position;
                bullet.gameObject.transform.rotation = m_FirePosition.rotation;
                bullet.gameObject.SetActive(true);
                queue.Enqueue(bullet);
                return bullet;
            }
            else
            {
                // all busy and create new one!
                Rigidbody2D bullet = GetBulletPrefab(direction);
                if (null == bullet) { return null; }
                bullet = Instantiate(bullet, m_FirePosition.position, m_FirePosition.rotation, gameObject.transform);
                return bullet;
            }
        }
#endif     
    }

#if ABANDON
    public IEnumerator DestroyBullet(Rigidbody2D bullet, float lifetime)
    {
        if (null == bullet) { yield break; }
        if (!bullet.gameObject.activeSelf) { yield break; }

        yield return new WaitForSeconds(lifetime);

        foreach (var kv in m_Bullets)
        {
            if (kv.Value.Contains(bullet))
            {
                bullet.gameObject.SetActive(false);
                yield break;
            }
        }

        Destroy(bullet);
    }
#endif

    public IEnumerator Fire()
    {
        IsFiring = true;
       
        Direction direction = GetDirection();
        if (direction == Direction.NONE) { yield break; }

        Rigidbody2D bullet = GetBullet(direction);
        if (null == bullet) { yield break; }

        Vector2 movement = GetMovement(direction);

        bullet.velocity = movement * m_FireForce;

        // StartCoroutine(DestroyBullet(bullet, m_LifeTime));

        m_AudioSFX.clip = m_FireClip;
        m_AudioSFX.Play();

        yield return new WaitForSeconds(m_PlayerInfo.Cooling);

        IsFiring = false;
    }

    private Direction GetDirection()
    {
        AnimatorStateInfo animStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        // get direction by animation info

        if (animStateInfo.IsName("PlayerUp"))
        {
            return Direction.UP;
        }
        else if (animStateInfo.IsName("PlayerLeft"))
        {
            return Direction.LEFT;
        }
        else if (animStateInfo.IsName("PlayerDown"))
        {
            return Direction.DOWN;
        }
        else if (animStateInfo.IsName("PlayerRight"))
        {
            return Direction.RIGHT;
        }

        return Direction.NONE;
    }

    private Rigidbody2D GetBulletPrefab(Direction direction)
    {
        if (direction == Direction.NONE) { return null; }
        if (direction == Direction.UP) { return m_BulletUp; }
        if (direction == Direction.DOWN) { return m_BulletDown; }
        if (direction == Direction.LEFT) { return m_BulletLeft; }
        if (direction == Direction.RIGHT) { return m_BulletRight; }
        return null;
    }

    private Vector2 GetMovement(Direction direction)
    {
        if (direction == Direction.NONE) { return new Vector2(0, 0); }
        if (direction == Direction.UP) { return m_FirePosition.transform.up; }
        if (direction == Direction.DOWN) { return -m_FirePosition.transform.up; }
        if (direction == Direction.LEFT) { return -m_FirePosition.transform.right; ; }
        if (direction == Direction.RIGHT) { return m_FirePosition.transform.right; }
        return new Vector2(0, 0);
    }

    public void SetPlayerInfo(int level)
    {
        m_PlayerInfo = TankConfig.Instance.GetPlayerInfo(level);
    }

    private void OnFire(InputValue value)
    {
        // We have setup our button press action to be "Press and Release" trigger behavior in the "Press" "interaction" of the Input Action asset.
        // The isPressed property will be true when OnFire is called during initial button press.
        // It will be false when OnFire is called during button release.

        if (value.isPressed)
        {
            // Debug.Log("fire button is pressed");
            m_FireButtonPressedThisFrame = true;
            m_FireButtonReleasedThisFrame = false;
        }
        else
        {
            // Debug.Log("fire button is not pressed");
            m_FireButtonPressedThisFrame = false;
            m_FireButtonReleasedThisFrame = true;
        }
    }
}
