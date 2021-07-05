using System.Collections.Generic;
using UnityEngine;

public enum ActionMode
{
    CALCULATE,
    ATTACK,
    PATROL,
}

public enum ConditionMode
{
    IS_SEE_ENEMY,
}

public class BehaviorTree
{
    public BehaviorTree(Behavior root) { m_Root = root; }

    public void Tick(GameObject tank, GameObject grid)
    {
        if (null == tank || null == grid) { return; }
        m_Root.Tick(tank, grid);
    }

    Behavior m_Root;
}

public class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder()
    {
        m_Stack = new Stack<Behavior>();
    }

    public void AddBehavior(Behavior behavior) 
    {
        if (null == behavior) { return; }

        if (null == m_TreeRoot)
        {
            m_TreeRoot = behavior;
        }
        else
        {
            m_Stack.Peek().AddChild(behavior);
        }

        m_Stack.Push(behavior);
    }

    public BehaviorTreeBuilder CreateSequence()
    {
        Behavior behavior = Sequence.Create();
        AddBehavior(behavior);
        return this;
    }

    public BehaviorTreeBuilder CreateSelector()
    {
        Behavior behavior = Selector.Create();
        AddBehavior(behavior);
        return this;
    }

    public BehaviorTreeBuilder CreateParallel(Behavior.Poly success, Behavior.Poly failure)
    {
        Behavior behavior = Parallel.Create(success, failure);
        AddBehavior(behavior);
        return this;
    }

    public BehaviorTreeBuilder CreateCondition(ConditionMode mode, bool negation)
    {
        Behavior behavior; 

        switch (mode)
        {
            case ConditionMode.IS_SEE_ENEMY:
                behavior = Condition_IsSeeEnemy.Create(negation); 
                break;
            default:
                behavior = null;
                break;
        }

        AddBehavior(behavior);

        return this;
    }

    public BehaviorTreeBuilder CreateAction(ActionMode mode)
    {
        Behavior behavior;

        switch (mode)
        {
            case ActionMode.CALCULATE:
                behavior = CalculatePathAction.Create();
                break;
            case ActionMode.ATTACK:
                behavior = AttckAction.Create();
                break;
            case ActionMode.PATROL:
                behavior = PatrolAction.Create();
                break;
            default:
                behavior = null;
                break;
        }

        AddBehavior(behavior);

        return this;
    }

    public BehaviorTreeBuilder Back()
    {
        m_Stack.Pop();
        return this;
    }

    public BehaviorTree End()
    {
        m_Stack.Clear();
        BehaviorTree tmp = new BehaviorTree(m_TreeRoot);
        m_TreeRoot = null;
        return tmp;  
    }

    Behavior m_TreeRoot;
    Stack<Behavior> m_Stack;
}
