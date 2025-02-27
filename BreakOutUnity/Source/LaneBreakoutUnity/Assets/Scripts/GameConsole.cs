﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public interface IGameConsole
{
    string FontName { get; set; }
    string DebugText { get; set; }

    string GetGameConsoleText();
    void GameConsoleWrite(string s);
}

public class GameConsole : MonoBehaviour, IGameConsole {

    protected string fontName;
    public string FontName { get { return fontName; } set { fontName = value; } }

    protected string debugText;
    public string DebugText { get { return debugText; } set { debugText = value; } }

    protected int maxLines;
    public int MaxLines { get { return maxLines; } set { maxLines = value; } }

    protected List<string> gameConsoleText;
    protected GameConsoleState gameConsoleState;

    public KeyCode ToggleConsoleKey;

    public Text Console, Debug;

    public void Awake()
    {
        ToggleConsoleKey = KeyCode.BackQuote;
        gameConsoleText = new List<string>();
        gameConsoleState = GameConsoleState.Open;
        MaxLines = 15;
        GameConsoleWrite("Awake Called");
        debugText = "Default Debug Text";

        //Test for Canvas and GameConsoleText and Debug Text
    }

    public void Update()
    {
        //Console.enabled = false;
        switch(this.gameConsoleState)
        {
            case GameConsoleState.Open:
                Console.enabled = true;
                Debug.enabled = true;
                Console.text = GetGameConsoleText();
                Debug.text = DebugText;
                Console.gameObject.SetActive(true);
                Debug.gameObject.SetActive(true);
                break;
            case GameConsoleState.Closed:
                Console.enabled = false;
                Debug.enabled = false;
                Console.gameObject.SetActive(false);
                Debug.gameObject.SetActive(false);
                break;
        }
        if (gameConsoleState == GameConsoleState.Open)
        {
            
        }

        if(Input.GetKeyUp(ToggleConsoleKey))
        {
            this.ToggleConsole();
        }

    }

    public void ToggleConsole()
    {
        if (this.gameConsoleState == GameConsoleState.Closed)
        {
            this.gameConsoleState = GameConsoleState.Open;
        }
        else
        {
            this.gameConsoleState = GameConsoleState.Closed;
        }
    }

    public void Draw()
    {
        
    }

    public string GetGameConsoleText()
    {
        string Text = "";

        string[] current = new string[System.Math.Min(gameConsoleText.Count, MaxLines)];
        int offsetLines = (gameConsoleText.Count / maxLines) * maxLines;

        int offest = gameConsoleText.Count - offsetLines;

        int indexStart = offsetLines - (maxLines - offest);
        if (indexStart < 0)
            indexStart = 0;

        gameConsoleText.CopyTo(
            indexStart, current, 0, System.Math.Min(gameConsoleText.Count, MaxLines));

        foreach (string s in current)
        {
            Text += s;
            Text += "\n";
        }
        return Text;
    }

    public void GameConsoleWrite(string s)
    {
        gameConsoleText.Add(s);
    }

    //Console State
    public enum GameConsoleState { Closed, Open };
}

