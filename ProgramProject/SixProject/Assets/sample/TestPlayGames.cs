using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityPluginPlayGames;
//using UnityPluginPlayGames.Common;

public class TestPlayGames : MonoBehaviour {

	public Button btnSignIn;
	public Button btnSignOut;

	public Button btnAchievePrime;
	public Button btnAchieveSecond;
	public Button btnShowAchieve;

	public Button btnScore5000;
	public Button btnScore10000;
	public Button btnShowLeaderboard;

	//private IPlayGamesService playGamesService;

	// Use this for initialization
	void Start () {
		//playGamesService = AdsClientFactory.GetPlayGamesService();

		//playGamesService.OnSignInSucceeded += OnSignInSucceeded;
		//playGamesService.OnSignInFailed += OnSignInFailed;
		//playGamesService.OnRequireSignIn += OnRequireSignIn;

		//playGamesService.Setup();

		//btnSignIn.onClick.AddListener(() => {
		//	playGamesService.BeginUserInitiatedSignIn();
		//});

		//btnSignOut.onClick.AddListener(() => {
		//	playGamesService.SignOut();
		//	Debug.Log("Sign Out");
		//	ShowSignInButton();
		//});

		//btnAchievePrime.onClick.AddListener(() => {
		//	playGamesService.UnlockAchievement("CgkIhb2XjIMSEAIQAw");
		//});

		//btnAchieveSecond.onClick.AddListener(() => {
		//	playGamesService.UnlockAchievement("CgkIhb2XjIMSEAIQBA");
		//});

		//btnShowAchieve.onClick.AddListener(() => {
		//	bool isSignIn = playGamesService.IsSignedIn();
		//	Debug.Log("IsSignIn: " + isSignIn);
		//	playGamesService.ShowAchievements();
		//});

		//btnScore5000.onClick.AddListener(() => {
		//	playGamesService.SubmitScore("CgkIhb2XjIMSEAIQCA", 5000);
		//});

		//btnScore10000.onClick.AddListener(() => {
		//	playGamesService.SubmitScore("CgkIhb2XjIMSEAIQCA", 10000);
		//});

		//btnShowLeaderboard.onClick.AddListener(() => {
		//	playGamesService.ShowLeaderboard("CgkIhb2XjIMSEAIQCA");
		//});
	}
	
	void OnSignInSucceeded() {
		Debug.Log("OnSignInSucceeded!!!!!!!!!!!");
		ShowSignOutButton();
	}

	void OnSignInFailed(string reason) {
		Debug.LogWarning("OnSignInFailed " + reason);
		ShowSignInButton();
	}

	void ShowSignInButton() {
		btnSignIn.gameObject.SetActive(true);
		btnSignOut.gameObject.SetActive(false);
	}

	void ShowSignOutButton() {
		btnSignIn.gameObject.SetActive(false);
		btnSignOut.gameObject.SetActive(true);
	}

	void OnRequireSignIn() {
		Debug.Log("OnRequireSignIn~~~~~~~~~~~~~~~~~~");
		ShowSignInButton();
	}


}
