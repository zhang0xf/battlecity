using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAlgorithm
{
    // 图的广度优先搜索
    public static Dictionary<Location, Location> BreadthFirstSearch(SquareGrid grid, Location start)
    {
        Queue<Location> frontier = new Queue<Location>();
        Dictionary<Location, Location> comeFrom = new Dictionary<Location, Location>();

        frontier.Enqueue(start);
        comeFrom[start] = start;

        while (frontier.Count != 0)
        {
            Location current = frontier.Dequeue();
            List<Location> neighbors = grid.Neighbors(current);

            foreach (Location next in neighbors)
            {
                if (!comeFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    comeFrom[next] = current;
                }
            }
        }

        return comeFrom;
    }

    // 图的最佳优先级搜索

    // 图的迪克斯特拉算法

    // 图的A*搜索
}
