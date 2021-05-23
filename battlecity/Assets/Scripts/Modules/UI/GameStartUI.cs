using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStartUI : BaseUI
{
    private int index = 0;
    public GameObject[] buttons = null; // 通过界面绑定
    private EventSystem mEventSystem = null;

    protected override void OnLoad()
    {

        base.OnLoad();
    }

    // This function is called when the object(script) becomes enabled and active.
    private void OnEnable()
    {
        // Debug.Log(string.Format("GameStartUI : OnEnable()"));

        // Fetch the current EventSystem. Make sure your Scene has one.
        mEventSystem = EventSystem.current;

        // default setting
        if (null == mEventSystem)
        {
            Debug.Log(string.Format("Error : mEventSystem == null"));
            return;
        }
        mEventSystem.SetSelectedGameObject(buttons[0]);
    }

    public override void SelectDown()
    {
        if (buttons.Length <= 0) { return; }

        if (index != buttons.Length - 1)
            ++index;

        Game.Instance.StartCoroutine(SetButton(index));

        base.SelectDown();
    }

    public override void SelectLeft()
    {
        base.SelectLeft();
    }

    public override void SelectRight()
    {
        base.SelectRight();
    }

    public override void SelectUP()
    {
        if (buttons.Length <= 0) { return; }

        if (index != 0)
            --index;

        Game.Instance.StartCoroutine(SetButton(index));

        base.SelectUP();
    }

    private IEnumerator SetButton(int index)
    {
        yield return new WaitForSeconds(0.0f);
        
        // Debug.Log(string.Format("index = {0}", index));
        
        if (index < 0 || index >= buttons.Length) { yield break; }

        if (null == mEventSystem)
        {
            Debug.Log(string.Format("Error : mEventSystem == null"));
            yield break;
        }

        mEventSystem.SetSelectedGameObject(buttons[index]);
    }

}
