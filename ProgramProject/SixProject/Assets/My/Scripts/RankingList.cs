using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityPluginPlayGames;
//using UnityPluginPlayGames.Common;

public class RankingList : MonoBehaviour
{
    //public Button btnSignIn;           //登陆
    //public Button btnSignOut;           //退出

    //public Button btnAchievePrime;          //实现
    //public Button btnAchieveSecond;         //实现第二个          
    //public Button btnShowAchieve;           //显示完成        

    //public Button btnScore;             //5000
    //public Button btnScore10000;            //10000
    public Button btnShowLeaderboard;       //展示排行榜

    //private IPlayGamesService playGamesService;

    // Use this for initialization
    void Start()
    {
        //btnShowLeaderboard = GameObject.Find("LeaderboardButton").GetComponent<Button>();

        //playGamesService = AdsClientFactory.GetPlayGamesService();

        //try
        //{
        //    playGamesService.OnSignInSucceeded += OnSignInSucceeded;
        //    playGamesService.OnSignInFailed += OnSignInFailed;
        //    playGamesService.OnRequireSignIn += OnRequireSignIn;

        //    playGamesService.Setup();
        //}
        //catch (System.Exception)
        //{

        //    Debug.Log("No Active");
        //}


        //btnSignIn.onClick.AddListener(() => {
        //playGamesService.BeginUserInitiatedSignIn();
        //});

        //btnSignOut.onClick.AddListener(() => {
        //playGamesService.SignOut();
        //Debug.Log("Sign Out");
        //ShowSignInButton();
        //});

        //btnAchievePrime.onClick.AddListener(() => {
        //    playGamesService.UnlockAchievement("CgkIhb2XjIMSEAIQAw");
        //});

        //btnAchieveSecond.onClick.AddListener(() => {
        //    playGamesService.UnlockAchievement("CgkIhb2XjIMSEAIQBA");
        //});

        //btnShowAchieve.onClick.AddListener(() => {
        //    bool isSignIn = playGamesService.IsSignedIn();
        //    Debug.Log("IsSignIn: " + isSignIn);
        //    playGamesService.ShowAchievements();
        //});

        //btnScore.onClick.AddListener(() => {
        //    playGamesService.SubmitScore("CgkIxtab18QbEAIQAQ", 5000);
        //});

        //btnScore10000.onClick.AddListener(() => {
        //    playGamesService.SubmitScore("CgkIhb2XjIMSEAIQCA", 10000);
        //});

        //btnShowLeaderboard.onClick.AddListener(() =>
        //{
        //    Debug.Log("Open RankingList");

        //    try
        //    {
        //        playGamesService.ShowLeaderboard("CgkIxtab18QbEAIQAQ");

        //    }
        //    catch (System.Exception)
        //    {

        //        Debug.Log("Ranking");

        //    }
        //});
    }

    void OnSignInSucceeded()
    {
        Debug.Log("OnSignInSucceeded!!!!!!!!!!!");
        
    }

    void OnSignInFailed(string reason)
    {
        Debug.LogWarning("OnSignInFailed " + reason);
        
    }


    void OnRequireSignIn()
    {
        Debug.Log("OnRequireSignIn~~~~~~~~~~~~~~~~~~");
        
    }


}
