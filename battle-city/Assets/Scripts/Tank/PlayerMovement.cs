using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_Rigidbody;

    private bool IsMoving;

    private bool IsUpPressedThisFrame;
    private bool IsUpReleasedThisFrame = true;

    private bool IsDownPressedThisFrame;
    private bool IsDownReleasedThisFrame = true;

    private bool IsLeftPressedThisFrame;
    private bool IsLeftReleasedThisFrame = true;

    private bool IsRightPressedThisFrame;
    private bool IsRightReleasedThisFrame = true;

    private PlayerStatus m_PlayerStatus;
    private Vector2 m_MovementInputValue;

    private enum Direction
    { 
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
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

    private void SetMove(bool move, Direction direction)
    {
        if (direction == Direction.NONE) { return; }

        IsMoving = move;

        if (!IsMoving)
        {
            if (direction == Direction.UP)
            {
                m_Animator.SetBool("Up", false);
            }
            else if (direction == Direction.DOWN)
            {
                m_Animator.SetBool("Down", false);
            }
            else if (direction == Direction.LEFT)
            {
                m_Animator.SetBool("Left", false);
            }
            else if (direction == Direction.RIGHT)
            {
                m_Animator.SetBool("Right", false);
            }

            m_MovementInputValue = new Vector2(0, 0);
        }
        else
        {
            if (direction == Direction.UP)
            {
                m_Animator.SetBool("Up", move);
                m_MovementInputValue = new Vector2(0, 1);
            }
            else if (direction == Direction.DOWN)
            {
                m_Animator.SetBool("Down", move);
                m_MovementInputValue = new Vector2(0, -1);
            }
            else if (direction == Direction.LEFT)
            {
                m_Animator.SetBool("Left", move);
                m_MovementInputValue = new Vector2(-1, 0);
            }
            else if (direction == Direction.RIGHT)
            {
                m_Animator.SetBool("Right", move);
                m_MovementInputValue = new Vector2(1, 0);
            }
        }
    }

    private void SetMove()
    {
        if (IsUpPressedThisFrame && IsDownReleasedThisFrame && IsLeftReleasedThisFrame && IsRightReleasedThisFrame)
        {
            SetMove(true, Direction.UP);
        }

        if (IsDownPressedThisFrame && IsUpReleasedThisFrame && IsLeftReleasedThisFrame && IsRightReleasedThisFrame)
        {
            SetMove(true, Direction.DOWN);
        }

        if (IsLeftPressedThisFrame && IsUpReleasedThisFrame && IsDownReleasedThisFrame && IsRightReleasedThisFrame)
        {
            SetMove(true, Direction.LEFT);
        }

        if (IsRightPressedThisFrame && IsUpReleasedThisFrame && IsDownReleasedThisFrame && IsLeftReleasedThisFrame)
        {
            SetMove(true, Direction.RIGHT);
        }
    }

    private void OnUp(InputValue value)
    {
        if (value.isPressed)
        {
            IsUpPressedThisFrame = true;
            IsUpReleasedThisFrame = false;
            if (!IsMoving)
            {
                SetMove(true, Direction.UP); 
            }
        }
        else
        {
            IsUpPressedThisFrame = false;
            IsUpReleasedThisFrame = true;
            if (IsMoving && m_Animator.GetBool("Up"))
            {
                SetMove(false, Direction.UP);
            }
            SetMove();
        }
    }

    private void OnDown(InputValue value)
    {
        if (value.isPressed)
        {
            IsDownPressedThisFrame = true;
            IsDownReleasedThisFrame = false;
            if (!IsMoving) 
            {
                SetMove(true, Direction.DOWN); 
            }
        }
        else
        {
            IsDownPressedThisFrame = false;
            IsDownReleasedThisFrame = true;
            if (IsMoving && m_Animator.GetBool("Down"))
            {
                SetMove(false, Direction.DOWN);
            }
            SetMove();
        }
    }

    private void OnLeft(InputValue value)
    {
        if (value.isPressed)
        {
            IsLeftPressedThisFrame = true;
            IsLeftReleasedThisFrame = false;
            if (!IsMoving)
            {
                SetMove(true, Direction.LEFT);
            }
        }
        else
        {
            IsLeftPressedThisFrame = false;
            IsLeftReleasedThisFrame = true;
            if (IsMoving && m_Animator.GetBool("Left"))
            {
                SetMove(false, Direction.LEFT);
            }
            SetMove();
        }
    }

    private void OnRight(InputValue value)
    {
        if (value.isPressed)
        {
            IsRightPressedThisFrame = true;
            IsRightReleasedThisFrame = false;
            if (!IsMoving) 
            {
                SetMove(true, Direction.RIGHT);    ;
            }
        }
        else
        {
            IsRightPressedThisFrame = false;
            IsRightReleasedThisFrame = true;
            if (IsMoving && m_Animator.GetBool("Right"))
            {
                SetMove(false, Direction.RIGHT);
            }
            SetMove();
        }
    }
}
