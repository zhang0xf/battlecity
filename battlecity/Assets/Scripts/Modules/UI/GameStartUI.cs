using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStartUI : BaseUI
{
    private static int defaultIndex = 0;
    private int index = defaultIndex;
    private GameObject lastSelectObject = null;
    private GameObject currentSelectObject = null;
    public Button[] buttons = null; // set from inspector panel
    private EventSystem mEventSystem = null;

    protected override void OnLoad()
    {
        MessageController.Instance.AddNotification(NotificationName.UI_BUTTON_SELECTED_BY_MOUSE, RecvUIButtonSelectedByMouse);

        base.OnLoad();
    }

    // This function is called when the object(script) becomes enabled and active.
    private void OnEnable()
    {
        if (buttons.Length <= 0)
        {
            Debug.LogError("buttons is null");
        }

        // Fetch the current EventSystem. Make sure your Scene has one.
        mEventSystem = EventSystem.current;

        // Add Scripts for Button
        foreach (Button button in buttons)
        {
            if (null == button) continue;

            PointerData pointerData = button.gameObject.AddComponent<PointerData>();
            if (null == pointerData)
            {
                Debug.LogError(string.Format("button name : {0}, AddComponent<Script> Error", button.gameObject.name));
            }
        }
    }

    private void Start()
    {
        // default setting
        if (null == mEventSystem) { return; }
        if (null == buttons[defaultIndex]) { return; }
        
        lastSelectObject = null;
        currentSelectObject = buttons[defaultIndex].gameObject;
        currentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        mEventSystem.SetSelectedGameObject(currentSelectObject.gameObject);
    }

    public override void MouseMove()
    {
        Cursor.visible = true;

        base.MouseMove();
    }

    public override void SelectDown()
    {
        if (buttons.Length <= 0) { return; }
        if (index == buttons.Length - 1) { return; }

        lastSelectObject = currentSelectObject;
        currentSelectObject = buttons[++index].gameObject;
        
        Game.Instance.StartCoroutine(SetButton());
        
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
        if (index == 0) { return; }

        lastSelectObject = currentSelectObject;
        currentSelectObject = buttons[--index].gameObject;

        Game.Instance.StartCoroutine(SetButton());
        
        base.SelectUP();
    }

    private IEnumerator SetButton()
    {
        yield return new WaitForSeconds(0.0f);
        mEventSystem.SetSelectedGameObject(currentSelectObject);
        currentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        lastSelectObject.transform.Find("Icon").gameObject.SetActive(false);
    }

    private void RecvUIButtonSelectedByMouse(Notification notify)
    {
        if (null == notify) { return; }

        GameObject mousePoint = null;

        GameObject obj = (GameObject)notify.Content;

        PointerData pointerData = obj.gameObject.GetComponent<PointerData>();
        if (null == pointerData)
            mousePoint = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(obj);
        else
            mousePoint = obj;

        // Debug.Log(string.Format("mousePoint object name is {0}", mousePoint.name));

        if (mousePoint == currentSelectObject) { return; }

        lastSelectObject = currentSelectObject;
        currentSelectObject = mousePoint;

        mEventSystem.SetSelectedGameObject(currentSelectObject);
        currentSelectObject.transform.Find("Icon").gameObject.SetActive(true);
        lastSelectObject.transform.Find("Icon").gameObject.SetActive(false); // lastSelectObject 不可能为null
        
        SnycIndex(currentSelectObject); // 将鼠标的跳跃性选择 同步=> 键盘和手柄

    }

    private void SnycIndex(GameObject currentSelectObject)
    {
        int idx = 0;
        foreach (Button button in buttons)
        {
            if (button.gameObject == currentSelectObject)
            {
                index = idx;
                break;
            }
            ++idx;
        }
    }

}
