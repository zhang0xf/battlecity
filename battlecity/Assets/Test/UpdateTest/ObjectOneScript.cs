using UnityEngine;

public class ObjectOneScript : MonoBehaviour
{
    GameObject obj = null;
    BaseScript script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj = GameObject.Find("Buttons");
        if (obj != null)
        {
            script = obj.GetComponent<BaseScript>();
            if (script == null)
            {
                Debug.Log("NULL");
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            script.SetButton();
        }
    }
}
