using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState {PAUSE, PLAY, CUTSCENE}
public class GameManager : MonoBehaviour
{

    public static GameState state;
    [SerializeField] GameState visibleState;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        state = GameState.PLAY;
    }

    // Update is called once per frame
    void Update()
    {
        visibleState = state;
    }

    public void PauseAndUnpause()
    {
        if(state == GameState.PAUSE) 
        { 
            state = GameState.PLAY;
            Time.timeScale = 1;
        }
        else if(state == GameState.PLAY)
        {
            state = GameState.PAUSE;
            Time.timeScale = 0;
        }
    }
}
