using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{
    private static int LevelsPassed = 0;
    private static int CurrentLevel = 1;
    private static bool gameOver = false;
    private static bool hasPlayed = false;
    private static bool gameActive = false;
    private static bool startingFromEnd = false;
    private static bool redCheck = false;
    private static bool whiteCheck = false;
    private static bool blueCheck = false;

    public static void SetCurrentLevel(int levelNumber)
    {
        CurrentLevel = levelNumber;
    }

    public static int GetLevelsPassed()
    {
        return LevelsPassed;
    }

    public static void SetLevelsPassed(int num)
    {
        LevelsPassed = num;
    }

    public static void AddLevelPassed(int num){
        LevelsPassed += num;
    }

    public static void SetGameOver(bool gameStatus)
    {
        gameOver = gameStatus;
    }

     public static void SetGameActive()
    {
        gameOver = false;
    }

    public static bool IsGameOver()
    {
        return gameOver;
    }

    public static void EndGameFinale()
    {
        if(gameOver){return;}
        Debug.Log("Ending game finale");
        gameOver = true;
        hasPlayed = false;
        EndGameScript endGameScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EndGameScript>();
        endGameScript.EndGame();
    }

    public static int GetCurrentLevelNumber()
    {
        return CurrentLevel;
    }

    public static bool GetHasPlayed()
    {
        return hasPlayed;
    }

    public static void SetHasPlayed(bool played)
    {
        hasPlayed = played;
    }

    public static bool IsGameActive(){
        return gameActive;
    }

    public static void SetGameActive(bool active)
    {
        gameActive = active;
    }

    public static void SetStartingFromEnd(bool start)
    {
        startingFromEnd = start;
    }

    public static bool GetStartingFromEnd()
    {
        return startingFromEnd;
    }

    public static bool GetRedCheck()
    {
        return redCheck;
    }

     public static bool GetWhiteCheck()
    {
        return whiteCheck;
    }

     public static bool GetBlueCheck()
    {
        return blueCheck;
    }

    public static void SetRedCheck(bool val)
    {
        redCheck = val;
    }

    public static void SetWhiteCheck(bool val)
    {
        whiteCheck = val;
    }

    public static void SetBlueCheck(bool val)
    {
        blueCheck = val;
    }
}
