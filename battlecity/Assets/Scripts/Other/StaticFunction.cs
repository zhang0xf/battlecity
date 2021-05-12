using System;
using UnityEngine;

public static class StaticFunction
{
    public static T CreateObjectByType<T>(Type type)
    {
        try
        {
            if (null == type)
                return default(T);
            else
                return (T)Activator.CreateInstance(type);
        }
        catch(Exception e)
        {
            Debug.Log(string.Format("exception : {0}", e));
            return default(T);
        }
    }

}
