using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager
{
    private static ModuleManager mInstance = null;
    private Dictionary<string, BaseObject> dict = null;

    public ModuleManager()
    {
        dict = new Dictionary<string, BaseObject>();
    }

    public static ModuleManager Instance
    {
        get
        {
            if (null == mInstance)
                mInstance = new ModuleManager();
            return mInstance;
        }
    }

    public void RegisterModule(string name, BaseObject obj)
    {
        if (dict.ContainsKey(name))
            dict.Remove(name);
        dict.Add(name, obj);
    }

    public void UnRegisterModule(string name)
    {
        if (!dict.ContainsKey(name)) { return; }
        dict.Remove(name);
    }

    public void RegisterModule(BaseObject obj)
    {
        Type t = obj.GetType(); // Gets the Type of the current instance.(obj = 子类)
        string name = t.ToString();
        RegisterModule(name, obj);
    }

    public void UnRegisterModule(BaseObject obj)
    {
        Type t = obj.GetType();
        string name = t.ToString();
        UnRegisterModule(name);
    }
}
