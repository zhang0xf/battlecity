using UnityEngine;
using UnityEngine.EventSystems;

// Add this scripts to gameObject. 

public class PointerData : MonoBehaviour, IPointerEnterHandler
{
    // required interface when using the OnPointerEnter method.

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            // Debug.Log(string.Format("pointerEnter name : {0}", eventData.pointerEnter.name));
            Notification na = new Notification(NotificationName.POINTER_ENTER, this);
            na.Content = eventData.pointerEnter;
            na.Send();
        }
    }
}
