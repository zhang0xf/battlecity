using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{ 
    private static ResourceManager mInstance = null;
    private Dictionary<string, GameObject> Dict = null;

    private ResourceManager()
    {
        Dict = new Dictionary<string, GameObject>();
    }

    public static ResourceManager Instance
    {
        get {
            if (mInstance == null) {
                mInstance = new ResourceManager();
            }
            return mInstance;
        }        
    }

    /* ResourceData background = new ResourceData("BackGround", "Resources/Prefabs/BackGround");
     ResourceData title = new ResourceData("Title", "Resources/Prefabs/Title");*/

    public bool AddResources(ResourceData data) 
    {
        GameObject obj = Resources.Load(data.Path) as GameObject;

        if (!Dict.ContainsKey(data.Name)) 
        {
            Dict.Add(data.Name, obj);
            return true;
        }
        return false;
    }

    public GameObject GetResources(string name)
    {
        GameObject obj = null;

        if (Dict.ContainsKey(name)) 
        {
            obj =  Dict[name];
        }

        return obj;
    }




}

