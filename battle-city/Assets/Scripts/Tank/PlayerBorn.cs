using System.Collections;
using UnityEngine;

public class PlayerBorn : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    public void PlayBorn()
    {
        m_Animator.SetBool("Born", true);
    }
}
