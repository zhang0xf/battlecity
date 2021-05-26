using System;
using System.Collections.Generic;

public class ModuleManager
{
    private static ModuleManager mInstance = null;
    private Dictionary<string, BaseObject> moduleDict = null;
    private Dictionary<string, BaseObject> updateDict = null;

    public ModuleManager()
    {
        moduleDict = new Dictionary<string, BaseObject>();
        updateDict = new Dictionary<string, BaseObject>();
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
        if (moduleDict.ContainsKey(name))
            moduleDict.Remove(name);
        moduleDict.Add(name, obj);

        if (obj is IUpdate && !updateDict.ContainsKey(name))
        {
            updateDict.Add(name, obj);
        }
    }

    public void UnRegisterModule(string name)
    {
        if (!moduleDict.ContainsKey(name)) { return; }
        moduleDict.Remove(name);

        if (!updateDict.ContainsKey(name)) { return; }
        updateDict.Remove(name);
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
