using System;
using System.Collections.Generic;

public class ModuleManager
{
    private static ModuleManager m_Instance = null;
    private Dictionary<string, BaseObject> m_ModuleRecord = null;
    private Dictionary<string, BaseObject> m_UpdateRecord = null;

    public ModuleManager()
    {
        m_ModuleRecord = new Dictionary<string, BaseObject>();
        m_UpdateRecord = new Dictionary<string, BaseObject>();
    }

    public static ModuleManager Instance
    {
        get
        {
            if (null == m_Instance)
                m_Instance = new ModuleManager();
            return m_Instance;
        }
    }

    public void RegisterModule(BaseObject obj)
    {
        Type t = obj.GetType();
        string name = t.ToString();
        RegisterModule(name, obj);
    }

    public void UnRegisterModule(BaseObject obj)
    {
        Type t = obj.GetType();
        string name = t.ToString();
        UnRegisterModule(name);
    }

    private void RegisterModule(string name, BaseObject obj)
    {
        if (m_ModuleRecord.ContainsKey(name))
            m_ModuleRecord.Remove(name);
        m_ModuleRecord.Add(name, obj);

        if (obj is IUpdate && !m_UpdateRecord.ContainsKey(name))
        {
            m_UpdateRecord.Add(name, obj);
        }
    }

    private void UnRegisterModule(string name)
    {
        if (!m_ModuleRecord.ContainsKey(name)) { return; }
        m_ModuleRecord.Remove(name);

        if (!m_UpdateRecord.ContainsKey(name)) { return; }
        m_UpdateRecord.Remove(name);
    }
}
