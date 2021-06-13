using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f); // Vector3(x, y, z)
        transform.position = transform.position + horizontal * Time.deltaTime;
        Debug.Log("hello world");
    }
}
