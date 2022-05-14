using System;
using System.Collections.Generic;

// 面向最小元素的优先队列：使用二叉堆实现
public class PriorityQueue<Priority, Type> where Priority : IComparable<Priority>
{
    private int count;  // 元素个数
    private Priority[] priorities;  // 索引从1~N
    private Type[] types;   // 关联数组

    public PriorityQueue(int capcity)
    {
        count = 0;
        priorities = new Priority[capcity + 1];
        types = new Type[capcity + 1];
    }

    private void Resize(int capcity)
    {
        if (capcity <= count) { return; }

        Priority[] temp1 = new Priority[capcity + 1];
        Type[] temp2 = new Type[capcity + 1];

        for (int i = 0; i <= count; i++)
        {
            temp1[i] = priorities[i];
            temp2[i] = types[i];
        }

        priorities = temp1;
        types = temp2;
    }

    public bool IsEmpty()
    {
        return 0 == count;
    }

    public int Size() 
    {
        return count; 
    }

    private bool Greater(int i, int k)
    {
        return priorities[i].CompareTo(priorities[k]) > 0;
    }

    private void Exch(int i, int k)
    {
        var temp1 = priorities[i];
        priorities[i] = priorities[k];
        priorities[k] = temp1;

        var temp2 = types[i];
        types[i] = types[k];
        types[k] = temp2;
    }

    public KeyValuePair<Priority, Type> Min()
    {
        if (!IsEmpty())
        {
            return new KeyValuePair<Priority, Type>(priorities[1], types[1]);
        }
        return new KeyValuePair<Priority, Type>();
    }

    public void Insert(KeyValuePair<Priority, Type> pair)
    {
        if (count == priorities.Length - 1)
        {
            Resize(2 * priorities.Length);
        }

        ++count;

        priorities[count] = pair.Key;
        types[count] = pair.Value;

        Swim(count);
    }

    public KeyValuePair<Priority, Type> DeleteMin()
    {
        if (!IsEmpty())
        {
            var minP = priorities[1];
            var minT = types[1];

            Exch(1, count--);
            Sink(1);

            priorities[count + 1] = default(Priority);  // 处理游离态数据
            types[count + 1] = default(Type);

            if (count > 0 && count == (priorities.Length - 1) / 4)
            {
                Resize(priorities.Length / 2);
            }

            return new KeyValuePair<Priority, Type>(minP, minT);
        }

        return new KeyValuePair<Priority, Type>();
    }

    // 上浮
    private void Swim(int k)
    {
        while (k > 1 && Greater(k / 2, k))
        {
            Exch(k, k / 2);
            k = k / 2;
        }
    }

    // 下沉
    private void Sink(int k)
    {
        while (2 * k <= count)
        {
            int j = 2 * k;
            if (j < count && Greater(j, j + 1)) j++;
            if (!Greater(k, j)) break;
            Exch(k, j);
            k = j;
        }
    }
}