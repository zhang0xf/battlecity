using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

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
                .CreateAction(ActionMode.PATROL)
                    .Back()
                .Back()
            .End();
    }

    private void FixedUpdate()
    {
        BT.Tick(gameObject);
    }

    private Direction GetDirection()
    {
        AnimatorStateInfo animStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.IsName("EnemyUp"))
        {
            return Direction.UP;
        }
        else if (animStateInfo.IsName("EnemyLeft"))
        {
            return Direction.LEFT;
        }
        else if (animStateInfo.IsName("EnemyDown"))
        {
            return Direction.DOWN;
        }
        else if (animStateInfo.IsName("EnemyRight"))
        {
            return Direction.RIGHT;
        }

        return Direction.NONE;
    }
}
