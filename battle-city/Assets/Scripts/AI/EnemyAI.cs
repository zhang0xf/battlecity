using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject m_Level;
    private BehaviorTree BT;

    private void Awake()
    {
        // 构建敌人的行为树
        BehaviorTreeBuilder budiler = new BehaviorTreeBuilder();
        BT = budiler
            .CreateSelector()
                .CreateSequence()
                    .CreateCondition(ConditionMode.IS_SEE_ENEMY, false)
                        .Back()
                    .CreateAction(ActionMode.ATTACK)
                        .Back()
                    .Back()
                .CreateSequence()
                    .CreateAction(ActionMode.PATROL)
                        .Back()
                    .Back()
                .Back()
            .End();

        // 消息注册
        MessageController.Instance.AddNotification(NotificationName.LEVEL_CHANGE, RecvLevelChange);
    }

    private void FixedUpdate()
    {
        BT.Tick(gameObject, m_Level);
    }

    private void RecvLevelChange(Notification notify)
    {
        if (null == notify) { return; }
        GameObject level = (GameObject)notify.Content;
        m_Level = level;
    }
}
