using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title,
    Playing,
    Win,
    Lose
}

public class ScoreManager : MonoBehaviour {


    
    public static int Lives;
    public static int Level;
    public static int Score;
    private static string WinLoseTextString;


    Texture2D paddle;
    public Text  LivesText, LevelText, ScoreText, WinLoseText;

    public static string LoseString = "You lose !!!! press R to restart";
    public static string WinString = "You win !!!!! !!!! press R to restart";

    Vector2 scoreLoc, livesLoc, levelLoc;

    public static GameState State = GameState.Playing;

    private static void SetupNewGame()
    {
        Lives = 3;
        Level = 1;
        Score = 0;
        WinLoseTextString = "";
        State = GameState.Playing;
    }

    public static void LoseLife()
    {
        ScoreManager.Lives -= 1;
        if (ScoreManager.Lives <= 0)
        {
            // end Game !!!
            ScoreManager.State = GameState.Lose;
            ScoreManager.WinLoseTextString = ScoreManager.LoseString;
        }
    }

    public static void Win()
    {
        // end Game !!!
        ScoreManager.State = GameState.Win;
        ScoreManager.WinLoseTextString = ScoreManager.WinString;
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //there has got to be a better way to do this, right?
    }

    void Awake()
    {
        ScoreManager.SetupNewGame();
    }

    // Use this for initialization
	void Start () {
        LivesText.text = "Lives: " + ScoreManager.Lives.ToString();
        LevelText.text = "Level: " + ScoreManager.Level.ToString();
        ScoreText.text = "Score: " + ScoreManager.Score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        LivesText.text = "Lives: " + ScoreManager.Lives.ToString();
        LevelText.text = "Level: " + ScoreManager.Level.ToString();
        ScoreText.text = "Score: " + ScoreManager.Score.ToString();
        WinLoseText.text = ScoreManager.WinLoseTextString;

        switch (ScoreManager.State)
        {
            case GameState.Title:
            case GameState.Playing:
                break;
            case GameState.Win:
            case GameState.Lose:
                if (Input.GetKeyDown(KeyCode.R)) ScoreManager.RestartGame();
                break;
        }

	}
}
