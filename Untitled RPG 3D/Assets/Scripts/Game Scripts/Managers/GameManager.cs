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

    public GameObject player;

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

        player = GameObject.Find("Player");
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
            player.GetComponent<PlayerController>().EnablePlayerActionMap();
            this.GetComponent<MenuManager>().MenuUIPauseUnpause();
            

        }
        else if(gameState == GameState.PLAY)
        {
            this.GetComponent<SoundManager>().StopAllAudio();
            gameState = GameState.PAUSE;
            Time.timeScale = 0;
            player.GetComponent<PlayerController>().DisablePlayerActionMap();
            this.GetComponent<MenuManager>().MenuUIPauseUnpause();
            
        }
    }
}
