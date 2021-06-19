using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    
    private float m_Speed;
    private Rigidbody2D m_Rigidbody;
    private InputManager m_InputManager;
    private Vector2 m_MovementInputValue;
    private Vector2 m_DefaultMovement = new Vector2(0, 0);
    private PlayerStatus m_PlayerStatus;

    void Awake()
    {
        m_InputManager = new InputManager();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_InputManager.Player.Up.performed += 
            ctx => { m_Animator.SetBool("Up", true); m_MovementInputValue = new Vector2(0, 1); };

        m_InputManager.Player.Up.canceled += 
            ctx => { m_Animator.SetBool("Up", false); m_MovementInputValue = m_DefaultMovement; };

        m_InputManager.Player.Down.performed += 
            ctx => { m_Animator.SetBool("Down", true); m_MovementInputValue = new Vector2(0, -1); };

        m_InputManager.Player.Down.canceled += 
            ctx => { m_Animator.SetBool("Down", false); m_MovementInputValue = m_DefaultMovement; };

        m_InputManager.Player.Left.performed += 
            ctx => { m_Animator.SetBool("Left", true); m_MovementInputValue = new Vector2(-1, 0); };

        m_InputManager.Player.Left.canceled += 
            ctx => { m_Animator.SetBool("Left", false); m_MovementInputValue = m_DefaultMovement; };

        m_InputManager.Player.Right.performed += 
            ctx => { m_Animator.SetBool("Right", true); m_MovementInputValue = new Vector2(1, 0); };

        m_InputManager.Player.Right.canceled += 
            ctx => { m_Animator.SetBool("Right", false); m_MovementInputValue = m_DefaultMovement; };
    }

    void OnEnable()
    {
        m_InputManager.Enable();
    }

    void OnDisable()
    {
        m_InputManager.Disable();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movement = m_MovementInputValue * m_PlayerStatus.Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    public void SetPlayerStatus(int id)
    {
        m_PlayerStatus = PlayerConfig.Instance.GetPlayerStatus(id);
    }
}
