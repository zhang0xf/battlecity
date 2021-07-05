using System.Collections.Generic;
using UnityEngine;

// 行为节点基类
public class Behavior
{
    // 行为树节点运行状态
    public enum BStatus
    {
        INVALID,
        SUCCESS,
        FAILURE,
        RUNNING,
        ABORTED,
    }

    // 并行复合节点判定策略
    public enum Poly
    {
        REQUIRE_ONE,
        REQUIRE_ALL,
    }

    public BStatus Tick(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        if (m_Status != BStatus.RUNNING)
        {
            OnInitialize();
        }

        // 每次TIck()执行一次OnUpdate()
        m_Status = OnUpdate(tank, level);

        if (m_Status != BStatus.RUNNING)
        {
            OnTerminate();
        }

        return m_Status;
    }

    protected virtual void OnInitialize() { }
    protected virtual void OnTerminate() { }
    protected virtual BStatus OnUpdate(GameObject tank, GameObject grid) { return BStatus.INVALID; }
    public bool IsTerminate() { return m_Status == BStatus.FAILURE || m_Status == BStatus.SUCCESS; }
    public bool IsRunning() { return m_Status == BStatus.RUNNING; }
    public bool IsSuccess() { return m_Status == BStatus.SUCCESS; }
    public bool IsFailure() { return m_Status == BStatus.FAILURE; }
    public void Abort() { m_Status = BStatus.ABORTED; }
    public void Reset() { m_Status = BStatus.INVALID; }
    public virtual string Name() { return "Behavior"; }
    public virtual void AddChild(Behavior child) { m_child = child; }

    protected BStatus m_Status;
    protected Behavior m_child;
}

// 复合节点基类
public class Composite : Behavior
{
    public Composite()
    {
        m_Childs = new List<Behavior>();
    }

    public override void AddChild(Behavior child) { m_Childs.Add(child); }

    public void RemoveChild(Behavior child)
    {
        if (m_Childs.Contains(child))
        {
            m_Childs.Remove(child);
        }
    }

    public void ClearChild()
    {
        m_Childs.Clear();
        m_Childs = null;
    }

    protected List<Behavior> m_Childs;
}

// 条件基类
public class Condition : Behavior
{
    public Condition(bool negation) { m_Negation = negation; }
    // 是否取反
    protected bool m_Negation = false;    
}


// 动作基类
public class Action : Behavior
{ 
    
}

// 复合节点顺序执行器
public class Sequence : Composite
{
    public Sequence() { }

    public static Behavior Create() { return new Sequence(); }
    public override string Name() { return "Sequence"; }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        foreach (Behavior behavior in m_Childs)
        {
            BStatus state = behavior.Tick(tank, level);

            // 遇到一个不成功的行为便返回
            if (state != BStatus.SUCCESS)
                return state;
        }

        return BStatus.SUCCESS;
    }
}

// 复合节点选择器
public class Selector : Composite
{
    public Selector() { }

    public static Behavior Create() { return new Selector(); }
    public override string Name() { return "Selector"; }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        foreach (Behavior behavior in m_Childs)
        {
            BStatus state = behavior.Tick(tank, level);

            // 遇到一个成功的行为便返回
            if (state == BStatus.SUCCESS)
                return state;
        }
        return BStatus.FAILURE;
    }
}

// 复合节点并行器
public class Parallel : Composite
{
    public Parallel(Poly success, Poly failure ) 
    {
        m_Success = success;
        m_Failure = failure;
    }

    public static Behavior Create(Poly success, Poly failure) { return new Parallel(success, failure); }
    public override string Name() { return "Parallel"; }

    protected override void OnTerminate()
    {
        foreach (Behavior behavior in m_Childs)
        {
            if (behavior.IsRunning())
            {
                behavior.Abort();
            }
        }
    }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        int successCount = 0;
        int failureCount = 0;

        int count = m_Childs.Count;

        foreach (Behavior behavior in m_Childs)
        {
            if (behavior.IsTerminate()) { continue; }

            behavior.Tick(tank, level);

            if (behavior.IsSuccess())
            {
                ++successCount;
                if (m_Success == Poly.REQUIRE_ONE)
                {
                    behavior.Reset();
                    return BStatus.SUCCESS;
                }
            }
            else if (behavior.IsFailure())
            {
                ++failureCount;
                if (m_Failure == Poly.REQUIRE_ONE)
                {
                    behavior.Reset();
                    return BStatus.FAILURE;
                }
            }
        }

        if (successCount == count && m_Success == Poly.REQUIRE_ALL)
        {
            foreach (Behavior behavior in m_Childs)
            {
                behavior.Reset();
            }
            return BStatus.SUCCESS;
        }

        if (failureCount == count && m_Failure == Poly.REQUIRE_ALL)
        {
            foreach (Behavior behavior in m_Childs)
            {
                behavior.Reset();
            }
            return BStatus.FAILURE;
        }

        return BStatus.RUNNING;
    }

    private Poly m_Success;
    private Poly m_Failure;
}

// 是否看见敌人
public class Condition_IsSeeEnemy : Condition
{
    public Condition_IsSeeEnemy(bool negation) : base(negation) { }
    public static Behavior Create(bool negation) { return new Condition_IsSeeEnemy(negation); }
    public override string Name() { return "Condition_IsSeeEnemy"; }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        // RaycastHit2D result = Physics2D.Raycast(gameObject.transform.position );

        return BStatus.FAILURE;
    }
}

// 攻击动作
public class AttckAction : Action
{
    public AttckAction() { }

    public static Behavior Create() { return new AttckAction(); }
    public override string Name() { return "AttckAction"; }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        // Debug.LogFormat("{0} begin AttckAction", gameObject.name);

        return BStatus.SUCCESS;
    }
}

// 计算路径动作
public class CalculatePathAction : Action
{
    public CalculatePathAction() { }

    public static Behavior Create() { return new CalculatePathAction(); }
    public override string Name() { return "CalculatePathAction"; }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        SquareGridManager squareGridManager = level.GetComponent<SquareGridManager>();
        if (null == squareGridManager) { return BStatus.FAILURE; }

        EnemyMovement move = tank.GetComponent<EnemyMovement>();
        if (null == move) { return BStatus.FAILURE; }

        SquareGrid grid = squareGridManager.grid;
        if (null == grid) { return BStatus.FAILURE; }

        Location location = grid.WorldToSquareGrid(new Vector2(tank.transform.position.x, tank.transform.position.y));

        move.m_Path = GridAlgorithm.BreadthFirstSearch(grid, location);

        if (move.m_Path.Count != 0)
            return BStatus.SUCCESS;
        return BStatus.FAILURE;
    }
}

// 巡逻动作
public class PatrolAction : Action
{
    public PatrolAction() { }

    public static Behavior Create() { return new PatrolAction(); }
    public override string Name() { return "PatrolAction"; }

    protected override BStatus OnUpdate(GameObject tank, GameObject level)
    {
        if (null == tank || null == level) { return BStatus.INVALID; }

        SquareGridManager grid = level.GetComponent<SquareGridManager>();
        if (null == grid) { return BStatus.FAILURE; }

        EnemyMovement move = tank.GetComponent<EnemyMovement>();
        if (null == grid) { return BStatus.FAILURE; }

        if (move.m_Path.Count != 0)
        {
            Debug.LogFormat("find grid count :{0}", move.m_Path.Count);
            move.m_Path.Clear();
        }

        return BStatus.SUCCESS;
    }
}
