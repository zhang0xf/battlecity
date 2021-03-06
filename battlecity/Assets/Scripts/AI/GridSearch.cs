using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSearch
{
    private static double Heuristic(Location start, Location goal)
    {
        // 使用当前位置和目标的曼哈顿距离作为启发式函数的值
        // 当前启发式函数默认每步的花费是1，可调整以适应性能要求。
        return Mathf.Abs(goal.x - start.x) + Mathf.Abs(goal.y - start.y);
    }

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

    // 图的迪克斯特拉算法
    public static void DijkstraSearch(
        SquareGrid grid, 
        Location start, 
        Location goal, 
        out Dictionary<Location, Location> comeFrom,
        out Dictionary<Location, double> costSoFar)
    {
        if (null == grid) 
        {
            comeFrom = null;
            costSoFar = null;
            return;
        }

        costSoFar = new Dictionary<Location, double>();
        comeFrom = new Dictionary<Location, Location>();
        PriorityQueue<double, Location> priorityQueue = new PriorityQueue<double, Location>(100);

        costSoFar[start] = 0;
        comeFrom[start] = start;
        priorityQueue.Insert(new KeyValuePair<double, Location>(0, start));

        while (!priorityQueue.IsEmpty())
        {
            Location current = priorityQueue.DeleteMin().Value;

            if (current.Equals(goal))
            {
                // Debug.LogFormat("find goal:({0}, {1})", goal.x, goal.y);
                break;
            }

            List<Location> neighbors = grid.Neighbors(current);
            if (null == neighbors) { continue; }

            foreach (Location next in neighbors)
            {
                double newCost = costSoFar[current] + grid.cost(current, next);

                if (!costSoFar.ContainsKey(next) || costSoFar[next] > newCost)
                {
                    costSoFar[next] = newCost;
                    priorityQueue.Insert(new KeyValuePair<double, Location>(newCost, next));
                    comeFrom[next] = current;
                }
            }
        }
    }

    // 图的A*搜索
    public static void AStarSearch(
        SquareGrid grid, 
        Location start, 
        Location goal, 
        out Dictionary<Location, Location> comeFrom, 
        out Dictionary<Location, double> costSoFar)
    {
        if (null == grid)
        {
            comeFrom = null;
            costSoFar = null;
            return;
        }

        costSoFar = new Dictionary<Location, double>();
        comeFrom = new Dictionary<Location, Location>();
        PriorityQueue<double, Location> priorityQueue = new PriorityQueue<double, Location>(100);

        costSoFar[start] = 0;
        comeFrom[start] = start;
        priorityQueue.Insert(new KeyValuePair<double, Location>(0, start));

        while (!priorityQueue.IsEmpty())
        {
            Location current = priorityQueue.DeleteMin().Value;

            if (current.Equals(goal))
            {
                // Debug.LogFormat("find goal:({0}, {1})", goal.x, goal.y);
                break;
            }

            List<Location> neighbors = grid.Neighbors(current);
            if (null == neighbors) { continue; }

            foreach (Location next in neighbors)
            {
                double newCost = costSoFar[current] + grid.cost(current, next);

                if (!costSoFar.ContainsKey(next) || costSoFar[next] > newCost)
                {
                    costSoFar[next] = newCost;
                    double priority = costSoFar[next] + Heuristic(next, goal);
                    priorityQueue.Insert(new KeyValuePair<double, Location>(priority, next));
                    comeFrom[next] = current;
                }
            }
        }
    }
}
