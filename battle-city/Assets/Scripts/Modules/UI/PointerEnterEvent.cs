using UnityEngine;
using UnityEngine.EventSystems;

// Add this scripts to gameObject. 

public class PointerEnterEvent : MonoBehaviour, IPointerEnterHandler
{
    // required interface when using the OnPointerEnter method.
    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData pointerEnterData)
    {
        if (pointerEnterData.pointerEnter != null)
        {
            // Debug.Log(string.Format("pointerEnter name : {0}", pointerEnterData.pointerEnter.name));
            Notification na = new Notification(NotificationName.POINTER_ENTER, this);
            na.Content = pointerEnterData.pointerEnter;
            na.Send();
        }
    }
}
