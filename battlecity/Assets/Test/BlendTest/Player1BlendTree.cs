using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1BlendTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        // set animator params
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magtitude", movement.magnitude);

        transform.position = transform.position + movement * Time.deltaTime;
    }
}
