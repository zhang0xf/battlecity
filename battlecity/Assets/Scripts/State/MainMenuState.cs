using UnityEngine;

public class MainMenuState : StateBase
{
    private static MainMenuState mInstance = null;

    private MainMenuState() { }

    public static MainMenuState Instance
    {
        get 
        {
            if (null == mInstance)
            {
                mInstance = new MainMenuState();
                mInstance.AddState(GameState.MIAN_MENU);
            }
            return mInstance;
        }
    }

    public override void AddState(GameState state)
    {
        base.AddState(state);
    }

    public override void OnEnter()
    {
        Debug.Log(string.Format("Enter \"MainMenu State\"."));
        UIManager.Instance.OpenUI(UIType.MAIN_MENU_UI);
        base.OnEnter();
    }

    public override void OnExcute()
    {
        // Debug.Log("enter MainMenuState : OnExcute()");
        UpdateUI();

        base.OnExcute();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }

    public override void UpdateUI()
    {
        command = InputHandler.Instance.UIInputHandler();
        if (null == command) { return; }

        GameObject UIObject = UIManager.Instance.GetLastestUIFromCurrentScene();
        if (null == UIObject) { return; }

        if (!UIObject.name.Equals("MainMenuUI" + "(" + "Clone" + ")")) { return; }

        BaseUI baseUI = UIObject.GetComponent<BaseUI>();
        if (null == baseUI) { return; }

        command.OnExcute(baseUI);

        base.UpdateUI();
    }
}
