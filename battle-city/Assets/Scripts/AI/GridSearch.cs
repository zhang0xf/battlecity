using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSearch
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
                Debug.LogFormat("find goal:({0}, {1})", goal.x, goal.y);
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
                    comeFrom[next] = current;
                    priorityQueue.Insert(new KeyValuePair<double, Location>(newCost, next));
                }
            }
        }
    }

    // 图的最佳优先级搜索
    public static Dictionary<Location, Location> BestFirstSearch(SquareGrid grid, Location start, Location goal)
    {

        return null;
    }

    // 图的A*搜索
    public static Dictionary<Location, Location> AStarSearch(SquareGrid grid, Location start, Location goal)
    {

        return null;
    }


}
