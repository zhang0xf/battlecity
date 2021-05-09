using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private GameState State = GameState.NULL;

    void Awake()
    {
        LoadGame();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void LoadGame()
    {
        Debug.Log("game is started!");
        ChangeState(GameState.START);
        // SceneManager.LoadScene("SampleScene");
    }

    public void ChangeState(GameState state) 
    {
        State = state;
        switch (State) 
        {
            case GameState.START: 
                {
                    Debug.Log("game is started !");


                    break;
                }
            case GameState.OPTION:
                {
                    break;
                }
            case GameState.ENTERGAME:
                {
                    break;
                }
            default:
                break;
        }

    }
    



}
