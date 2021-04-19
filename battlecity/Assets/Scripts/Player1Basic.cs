using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Basic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));

        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f); // Vector3(x, y, z)
        transform.position = transform.position + horizontal * Time.deltaTime;

        Vector3 vertical = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f); // Vector3(x, y, z)
        transform.position = transform.position + vertical * Time.deltaTime;

        Debug.Log("hello world");
    }
}
