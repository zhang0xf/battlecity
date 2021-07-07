using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PQTest : MonoBehaviour
{
    PriorityQueue<double, string> pq;

    // Start is called before the first frame update
    void Start()
    {
        pq = new PriorityQueue<double, string>(10);
    }

    // Update is called once per frame
    void Update()
    {
        pq.Insert(new KeyValuePair<double, string>(5, "test1"));
        pq.Insert(new KeyValuePair<double, string>(8, "test2"));
        pq.Insert(new KeyValuePair<double, string>(3, "test3"));
        pq.Insert(new KeyValuePair<double, string>(4, "test4"));

        while (!pq.IsEmpty())
        {
            var min = pq.DeleteMin();
            Debug.LogFormat("cost is {0}, string is {1}", min.Key, min.Value);
        }
    }
}
