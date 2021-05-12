using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game mInstance = null;

    private Game()
    { 
        
    }

    public static Game Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = new Game();
            return mInstance;
        }
    }


    void Awake()
    {
        Load();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Load()
    {
        Debug.Log("”Œœ∑∆Ù∂Ø£°");
        LoadModule(typeof(PlayerModule));

    }

    private void LoadModule(Type type)
    {
        BaseObject obj = Activator.CreateInstance(type) as BaseObject;  // Creates an instance of the specified type using the constructor that best matches the specified parameters.
        obj.Load();
    }
    

}
