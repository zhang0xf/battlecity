using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
      /*  player1Basic = Resources.Load("Prefabs/Player/Player1Basic") as GameObject;
        if (player1Basic != null)
        {
            player1Basic = Instantiate(player1Basic);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void FixedUpdate()
    {
        player1Basic = GameObject.Find("Player1Basic");
        if (player1Basic == null)
        {
            Debug.LogError("nullpointer");
            return;
        }

        animator = player1Basic.GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("nullpointer");
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0.0f);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        transform.position = transform.position + movement * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            localposition = player1Basic.GetComponent<Transform>().position;
            localrotation = player1Basic.GetComponent<Transform>().rotation;

            GameObject player1BigCannon = Resources.Load("Prefabs/Player/Player1BigCannon") as GameObject;
            if (null == player1BigCannon) {
                Debug.LogError("nullpointer");
                return;
            }
            player1BigCannon = Instantiate(player1BigCannon);

            player1BigCannon.GetComponent<Transform>().SetPositionAndRotation(localposition, localrotation);

            animator.SetBool("Exit", true);
            Destroy(player1Basic);
        }

    }
}
