using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Joshua

public enum GameState {PAUSE, PLAY, CUTSCENE}
public enum CurrentLevel { }
public class GameManager : MonoBehaviour
{

    public static GameState gameState;
    [SerializeField] GameState visibleGameState;

    public static CurrentLevel currentLevelState;
    [SerializeField] CurrentLevel visibleCurrentLevelState;

    public static GameManager GameManagerInstance;

    private void Awake()
    {
        if (GameManagerInstance == null)
            GameManagerInstance = this;
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
        gameState = GameState.PLAY;
    }

    // Update is called once per frame
    void Update()
    {
        visibleGameState = gameState;
        visibleCurrentLevelState = currentLevelState;
    }

    public void PauseAndUnpause()
    {
        if(gameState == GameState.PAUSE) 
        { 
            gameState = GameState.PLAY;
            Time.timeScale = 1;
        }
        else if(gameState == GameState.PLAY)
        {
            gameState = GameState.PAUSE;
            Time.timeScale = 0;
        }
    }
}
