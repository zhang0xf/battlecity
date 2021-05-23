using UnityEngine;
using UnityEngine.UI;

public class ObjectTwoScript : BaseScript
{
    public Button button1 = null;
    public Button button2 = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetButton()
    {
        Debug.Log("enter ObjectTwoScript : SetButton() ");
        button1.Select();
        button2.OnSelect(null);
    }

}
