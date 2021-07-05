using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private HashSet<Location> m_Walls;
    private HashSet<Location> m_Barrier;
    private HashSet<Location> m_Water;
    private HashSet<Location> m_Grass;
    private Dictionary<Direction, Location> m_Direction;

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
                (location.x <= width)   &&
                (location.y >= -height) &&
                (location.x <= height);
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
            if (InBounds(next) && !IsWall(next) && !IsBarrier(next))
            {
                list.Add(next);
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
    [SerializeField] private int m_Count = 100;

    private SquareGrid grid;
    private int m_Cellsize = 1; // 网格大小

    private void Awake()
    {
        int height = (int)((m_TopBoundary.position.y - 0.5) / m_Cellsize);
        int width = (int)((m_RightBoundary.position.x - 0.5) / m_Cellsize);
        
        grid = new SquareGrid(height, width, m_Cellsize);

        // 添加地图障碍
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

    private void Update()
    {
        
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
                GameObject obj = Resources.Load("Prefabs/Level/Wall") as GameObject;
                if (null == obj) { yield break; }
                obj = Instantiate(obj, new Vector2(location.x + 0.5f, location.y + 0.5f),
                    gameObject.transform.Find("Map").rotation, gameObject.transform.Find("Map"));
            }
        }

        yield return null;
    }

    public IEnumerator ShowHighlight(Location location)
    {
        GameObject mask = Resources.Load("Prefabs/Level/Mask") as GameObject;
        if (null == mask) { yield break; }

        Vector3 position = new Vector3(location.x, location.y);
        Quaternion quaternion = new Quaternion(1, 1, 1, 1);

        mask = Instantiate(mask, position, quaternion, gameObject.transform.Find("Map"));

        yield return null;
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
