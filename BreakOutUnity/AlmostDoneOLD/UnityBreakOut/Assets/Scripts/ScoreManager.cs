using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {


    
    public static int Lives;
    public static int Level;
    public static int Score;

    Texture2D paddle;
    public Text  LivesText, LevelText, ScoreText;
   
    Vector2 scoreLoc, livesLoc, levelLoc;

    private static void SetupNewGame()
    {
        Lives = 3;
        Level = 1;
        Score = 0;
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
	}
}
