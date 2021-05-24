using UnityEngine;
using UnityEngine.EventSystems;

public class PointerData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 脚本绑定在UI上，鼠标进入UI时自动调用。
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            // Debug.Log(string.Format("pointerEnter gameobject name : {0}", eventData.pointerEnter.name));
            Notification na = new Notification(NotificationName.UI_BUTTON_SELECTED_BY_MOUSE, this);
            na.Content = eventData.pointerEnter;
            na.Send();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log(string.Format("eventData.pointerEnter is {0}", eventData.pointerEnter.name));
    }

}
