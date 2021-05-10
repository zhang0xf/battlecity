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

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Config.LoadConfig();

        Notification notify = new Notification(NotificationName.LOAD_ENEMY, this);
        MessageController.Instance.AddNotification(notify, LoadEnemy);
        // notify.Send();

        MessageController.Instance.AddSubscriber(notify, LoadEnemy);
        notify.Send(true);
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
            Debug.LogError("null pointer");
            return;
        }

        animator = player1Basic.GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("null pointer");
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
            if (null == player1BigCannon)
            {
                Debug.LogError("null pointer");
                return;
            }
            player1BigCannon = Instantiate(player1BigCannon);

            player1BigCannon.GetComponent<Transform>().SetPositionAndRotation(localposition, localrotation);

            animator.SetBool("Exit", true);
            Destroy(player1Basic);
        }

    }

    public void LoadEnemy(Notification notify) 
    {
        GameObject player = GameObject.Find("Player1Basic");
        if (null == player) { Debug.Log("can not find a player"); }
        // Transform transform = player.GetComponent<Transform>();
        localposition = player.GetComponent<Transform>().position;
        localrotation = player.GetComponent<Transform>().rotation;

        GameObject obj = Resources.Load("Prefabs/Enemy/Enemy1") as GameObject;
        obj = Instantiate(obj, localposition, localrotation);
    }

}
