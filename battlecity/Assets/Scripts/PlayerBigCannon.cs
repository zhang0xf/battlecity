using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigCannon : MonoBehaviour
{

    public Animator animator;
    GameObject player1Basic;
    Vector3 localposition;
    Quaternion localrotation;
    float horizontal;
    float vertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0.0f);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        transform.position = transform.position + movement * Time.deltaTime;
    }
}
