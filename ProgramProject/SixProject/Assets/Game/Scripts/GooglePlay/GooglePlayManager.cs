using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if GooglePlayDef
using GooglePlayGames; 
using UnityEngine.SocialPlatforms;
#endif 

namespace MadFireOn
{
    public class GooglePlayManager : MonoBehaviour
    {

        public static GooglePlayManager singleton;

        private const string Master = "CgkI4MTMtpUQEAIQAQ"; //replace this id with yours

        private const string Pro = "CgkI4MTMtpUQEAIQAg"; //replace this id with yours

        private const string God = "CgkI4MTMtpUQEAIQAw"; //replace this id with yours

        private const string UnlockMaster = " CgkI4MTMtpUQEAIQBA"; //replace this id with yours

        private const string GameLeader = "CgkI4MTMtpUQEAIQBQ"; //replace this id with yours

        private string[] achievements_names = { Master, Pro, God, UnlockMaster, GameLeader };

        private bool[] achievements;

        private AudioSource sound;

        [HideInInspector]
        public ManageVariables vars;

        void OnEnable()
        {
            vars = Resources.Load("ManageVariablesContainer") as ManageVariables;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);
            }
        }


        void InitializeAchievements()
        {
            achievements = GameManager.instance.achievements;

            for (int i = 0; i < achievements.Length; i++)
            {
                if (!achievements[i])
                {
                    Social.ReportProgress(achievements_names[i], 0.0f, (bool success) =>
                    {
                        //handle success
                    });
                }
            }
        }


        // Use this for initialization
        void Start()
        {
            sound = GetComponent<AudioSource>();
#if GooglePlayDef
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                InitializeAchievements();
            }
        });
#endif

        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            CheckIfAnyUnlockedAchievements();
            ReportScore(GameManager.instance.currentScore);
        }

        public void OpenLeaderboardsScore()
        {
#if GooglePlayDef
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(vars.leaderBoardID);
        }
#endif
        }

        void ReportScore(int score)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, vars.leaderBoardID, (bool success) => { });
            }
        }

        public void OpenAchievements()
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowAchievementsUI();
            }
        }

        void UnlockAchievements(int index)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(achievements_names[index], 100.0f, (bool success) =>
                {
                    if (success)
                    {
                        sound.Play(); //here we play the soucd when we achieve new achievement
                        achievements[index] = true;
                        GameManager.instance.achievements = achievements;
                        GameManager.instance.Save();
                    }
                });
            }
        }

        void CheckIfAnyUnlockedAchievements()
        {
            //we check if GameManager is present
            if (GameManager.instance != null)
            {
                //then we check if our score is greater than of equal to 150
                if (GameManager.instance.currentScore >= 300)
                {
                    //then we check if our 1st achievement is unlocke or not
                    if (!achievements[0])
                    {
                        //if not then we check if player is logged in
                        if (Social.localUser.authenticated)
                        {
                            //then we unlock the achievement
                            UnlockAchievements(0);
                        }
                    }
                }
            }//Achievement 0


            //we check if GameManager is present
            if (GameManager.instance != null)
            {
                //then we check if our score is greater than of equal to required score
                if (GameManager.instance.currentScore >= 600)
                {
                    //then we check if our 2nd achievement is unlocke or not
                    if (!achievements[1])
                    {
                        //if not then we check if player is logged in
                        if (Social.localUser.authenticated)
                        {
                            //then we unlock the achievement
                            UnlockAchievements(1);
                        }
                    }
                }
            }//Achievement 1


            //we check if GameManager is present
            if (GameManager.instance != null)
            {
                //then we check if our score is greater than of equal to required score
                if (GameManager.instance.currentScore >= 1200)
                {
                    //then we check if our 3rd achievement is unlocke or not
                    if (!achievements[2])
                    {
                        //if not then we check if player is logged in
                        if (Social.localUser.authenticated)
                        {
                            //then we unlock the achievement
                            UnlockAchievements(2);
                        }
                    }
                }
            }//Achievement 2


            //we check if GameManager is present
            if (GameManager.instance != null)
            {
                //then we check if our score is greater than of equal to required score
                if (GameManager.instance.currentScore >= 1600)
                {
                    //then we check if our 4th achievement is unlocke or not
                    if (!achievements[3])
                    {
                        //if not then we check if player is logged in
                        if (Social.localUser.authenticated)
                        {
                            //then we unlock the achievement
                            UnlockAchievements(3);
                        }
                    }
                }

            }//Achievement 3


            //we check if GameManager is present
            if (GameManager.instance != null)
            {
                //then we check if our score is greater than of equal to required score
                if (GameManager.instance.currentScore >= 2000)
                {
                    //then we check if our 5th achievement is unlocke or not
                    if (!achievements[4])
                    {
                        //if not then we check if player is logged in
                        if (Social.localUser.authenticated)
                        {
                            //then we unlock the achievement
                            UnlockAchievements(4);
                        }
                    }
                }
            }//Achievement 4



        }//CheckIfAnyUnlockedAchievements



    }
}//namespace