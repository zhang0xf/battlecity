using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : IEquatable<Location>
{
    public Location(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x { set; get; }
    public int y { set; get; }

    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Location);
    }

    public bool Equals(Location other)
    {
        return (x == other.x) && (y == other.y);
    }
}

// 图
public class SquareGrid
{
    // 原点在中心：height与width只是一半
    // 正方形左下顶点表示网格坐标
    private int height;
    private int width;
    private int m_CellSize;
    private double m_DefaultCost = 1;
    private double m_GrassCost = 3;
    private double m_WaterCost = 5;
    private HashSet<Location> m_Walls;
    private HashSet<Location> m_Barrier;
    private HashSet<Location> m_Water;
    private HashSet<Location> m_Grass;
    private Dictionary<Direction, Location> m_Direction;

    public Location m_Start;    // 起点
    public Location m_Goal;     // 目标
    public Dictionary<Location, Location> m_ComeFrom;   // 路径
    public Dictionary<Location, double> m_CostSoFar;    // 路径花费

    public SquareGrid(int height, int width, int cellSize)
    {
        this.height = height;
        this.width = width;
        m_CellSize = cellSize;
        init();
    }

    private void init()
    {
        m_Walls = new HashSet<Location>();
        m_Barrier = new HashSet<Location>();
        m_Water = new HashSet<Location>();
        m_Grass = new HashSet<Location>();
        m_Direction = new Dictionary<Direction, Location>();

        m_Direction.Add(Direction.UP, new Location(0, 1));
        m_Direction.Add(Direction.DOWN, new Location(0, -1));
        m_Direction.Add(Direction.LEFT, new Location(-1, 0));
        m_Direction.Add(Direction.RIGHT, new Location(1, 0));
    }

    private bool IsHome(Location location) { return location.Equals(new Location(0, 0)); }
    private bool IsWall(Location location) { return m_Walls.Contains(location); }
    private bool IsGrass(Location location) { return m_Grass.Contains(location); }
    private bool IsBarrier(Location location) { return m_Barrier.Contains(location); }
    private bool IsWater(Location location) { return m_Water.Contains(location); }
    private bool InBounds(Location location)
    {
        return  (location.x >= -width)  &&
                (location.x < width)   &&
                (location.y >= -height) &&
                (location.y < height);
    }

    public double cost(Location from, Location to)
    {
        if (Neighbors(from).Contains(to))
        {
            if (IsGrass(to)) { return m_GrassCost; }
            else if (IsWater(to)) { return m_WaterCost; }
            else return m_DefaultCost;
        }
        return -1;
    }

    public Location RandomLocation()
    {
        while (true)
        {
            int x = UnityEngine.Random.Range(-width, width);
            int y = UnityEngine.Random.Range(-height, height);
            Location goal = new Location(x, y);
            if (!InBounds(goal)) { continue; }
            if (IsBarrier(goal) || IsWall(goal) || IsHome(goal)) { continue; }
            return goal;
        }
    }

    public bool AddWall(Location location)
    {
        if (InBounds(location) && !m_Walls.Contains(location)) 
        {
            m_Walls.Add(location);
            return true;
        }
        return false;
    }

    public bool AddGrass(Location location)
    {
        if (InBounds(location) && !m_Grass.Contains(location)) 
        { 
            m_Grass.Add(location);
            return true;
        }
        return false;
    }

    public bool AddBarrier(Location location)
    {
        if (InBounds(location) && !m_Barrier.Contains(location))
        {
            m_Barrier.Add(location);
            return true;
        }
        return false;
    }

    public bool AddWater(Location location)
    {
        if (InBounds(location) && !m_Water.Contains(location)) 
        {
            m_Water.Add(location);
            return true;
        }
        return false;
    }

    // 图坐标 => 世界坐标
    public Vector2 SquareGridToWorld(Location location)
    {
        float x = (location.x + 0.5f) * m_CellSize;
        float y = (location.y + 0.5f) * m_CellSize;
        return new Vector2(x, y);
    }

    // 图路径 => 世界路径
    public Stack<Vector2> SquareGridToWorld(Dictionary<Location, Location> comeFrom)
    {
        if (null == comeFrom) { return null; }

        Stack<Vector2> stack = new Stack<Vector2>();

        Location current = m_Goal;

        while (!current.Equals(m_Start))
        {
            Vector2 location = SquareGridToWorld(current);
            stack.Push(location);
            current = comeFrom[current];
        }
        return stack;
    }

    // 图坐标 => 世界坐标
    public Dictionary<Vector2, double> SquareGridToWorld(Dictionary<Location, double> costSoFar)
    {
        if (null == costSoFar) { return null; }

        Dictionary<Vector2, double> dict = new Dictionary<Vector2, double>();

        foreach (var kv in costSoFar)
        {
            Vector2 location = SquareGridToWorld(kv.Key);
            dict.Add(location, kv.Value);
        }

        return dict;
    }

    // 世界坐标 => 图坐标
    public Location WorldToSquareGrid(Vector2 location)
    {
        // 向下取整
        int x = (int)Math.Floor(location.x);
        int y = (int)Math.Floor(location.y);
        return new Location(x, y);
    }

    public List<Location> Neighbors(Location location)
    {
        List<Location> list = new List<Location>();

        foreach (var direction in m_Direction)
        {
            Location next = new Location(location.x + direction.Value.x, location.y + direction.Value.y);
            if (InBounds(next))
            {
                if (!IsWall(next) && !IsBarrier(next))
                {
                    list.Add(next);
                }
            }
        }

        return list;
    }
}

public class SquareGridManager : MonoBehaviour
{
    [SerializeField] private Transform m_LeftBoundary;
    [SerializeField] private Transform m_RightBoundary;
    [SerializeField] private Transform m_TopBoundary;
    [SerializeField] private Transform m_ButtomBoundary;
    [SerializeField] private Transform m_Home;
    [SerializeField] private Transform[] m_Walls;
    [SerializeField] private Transform[] m_Grasses;
    [SerializeField] private Transform[] m_Barriers;
    [SerializeField] private Transform[] m_Waters;
    [SerializeField] private Transform[] m_Spawnpoints;
    [SerializeField] private int m_Count = 100;
    [SerializeField] private int m_Cellsize = 1; // 网格大小

    [HideInInspector] public SquareGrid grid;
    
    private GameObject m_GoalPrefab;
    private Queue<GameObject> m_PathPrefabs;

    private void Awake()
    {
        int height = (int)((m_TopBoundary.position.y - 0.5) / m_Cellsize);
        int width = (int)((m_RightBoundary.position.x - 0.5) / m_Cellsize);
        
        grid = new SquareGrid(height, width, m_Cellsize);
        m_PathPrefabs = new Queue<GameObject>();

        // 添加地图预设障碍
        foreach (var wall in m_Walls)
        {
            Location location = grid.WorldToSquareGrid(wall.position);
            grid.AddWall(location);
        }

        // 随机生成障碍
        StartCoroutine(GenerateWalls(grid));

#if UNITY_EDITOR
        // 绘制网格
        DrawSquareGrid();
#endif
    }

    private IEnumerator GenerateWalls(SquareGrid grid)
    {
        if (null == grid) { yield break; }

        for (int i = 0; i < m_Count; i++)
        {
            float random_x =
                UnityEngine.Random.Range((int)m_LeftBoundary.position.x + 0.5f, (int)m_RightBoundary.position.x - 0.5f);
            float random_y = 
                UnityEngine.Random.Range((int)m_ButtomBoundary.position.y + 0.5f, (int)m_TopBoundary.position.y - 0.5f);

            Location location = grid.WorldToSquareGrid(new Vector2(random_x, random_y));

            if (grid.AddWall(location))
            {
                GameObject wall = Resources.Load("Prefabs/Level/MapElements/Wall") as GameObject;
                if (null == wall) { yield break; }
                GameObject map = gameObject.transform.Find("Map").gameObject;
                if (null == map) { yield break; }
                wall = Instantiate(wall, grid.SquareGridToWorld(location), map.transform.rotation, map.transform);
            }
        }

        yield return null;
    }

    public IEnumerator DrawPath(Stack<Vector2> path, Vector2 goal)
    {
        if (null == path) { yield break; }

        Queue<Vector2> pathClone = new Queue<Vector2>(path.ToArray());

        GameObject pathPrefab = Resources.Load("Prefabs/Level/MapElements/Path") as GameObject;
        if (null == pathPrefab) { yield break; }

        GameObject map = gameObject.transform.Find("Map").gameObject;
        if (null == map) { yield break; }

        while (pathClone.Count != 0)
        {
            Vector2 location = pathClone.Dequeue();
            if (Vector2.Distance(location, goal) > 0.05)
            {
                pathPrefab = Instantiate(pathPrefab, location, map.transform.rotation, map.transform);
                m_PathPrefabs.Enqueue(pathPrefab);
            }
        }
    }

    public IEnumerator DrawGoal(Vector2 goal)
    {
        GameObject goalPrefab = Resources.Load("Prefabs/Level/MapElements/Goal") as GameObject;
        if (null == goalPrefab) { yield break; }

        GameObject map = gameObject.transform.Find("Map").gameObject;
        if (null == map) { yield break; }

        m_GoalPrefab = Instantiate(goalPrefab, goal, map.transform.rotation, map.transform);
    }

    public void DestroyPathPrefabs()
    {
        while (m_PathPrefabs.Count != 0)
        {
            Destroy(m_PathPrefabs.Dequeue(), 0.0f);
        }
        m_PathPrefabs.Clear();
    }

    public void DestroyGoalPrefab()
    {
        if (m_GoalPrefab != null)
        {
            Destroy(m_GoalPrefab, 0.0f);
        }
    }

    public void DrawSquareGrid()
    {
        for (float x = m_LeftBoundary.position.x + 0.5f; x <= m_RightBoundary.position.x -0.5f; x++)
        {
            Vector2 start = new Vector2(x, m_TopBoundary.position.y - 0.5f);
            Vector2 end = new Vector2(x, m_ButtomBoundary.position.y + 0.5f);
            Debug.DrawLine(start, end, Color.white, float.MaxValue);
        }

        for (float y = m_ButtomBoundary.position.y + 0.5f; y <= m_TopBoundary.position.y - 0.5f; y++)
        {
            Vector2 start = new Vector2(m_LeftBoundary.position.x + 0.5f, y);
            Vector2 end = new Vector2(m_RightBoundary.position.x - 0.5f, y);
            Debug.DrawLine(start, end, Color.white, float.MaxValue);
        }
    }
}
