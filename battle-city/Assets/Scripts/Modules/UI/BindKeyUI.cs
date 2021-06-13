using UnityEngine;

public class BindKeyUI : BaseUI
{
    // private UIType uIType;

    protected override void OnLoad()
    {
        init();

        // GameManager.Instance.CreateCurrScene(SceneName.MENU, typeof(BindKeyScene));

        // MessageController.Instance.AddNotification(NotificationName.BIND_KEY, RecvBindKey);

        base.OnLoad();
    }

    private void init()
    {
        // uIType = UIType.BIND_KEY_UI;
    }

    public override void OnRelease()
    {
        // MessageController.Instance.RemoveNotification(NotificationName.BIND_KEY, RecvBindKey);
        base.OnRelease();
    }

/*    public void RecvBindKey(Notification notify)
    {
*//*        if (!UIManager.Instance.IsUIReady(uIType)) { return; }

        bindKeyScene = GameManager.Instance.CurrScene as BindKeyScene;
        if (null == bindKeyScene) { return; }

        if (bindKeyScene.CurrState != ObjState.READY) { return; }

        KeyCode keycode = (KeyCode)notify.Content;

        UIManager.Instance.PopUI();*//*

        base.RecvBack(notify);
    }*/

}
