using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MadFireOn
{
    public class MainMenuManager : MonoBehaviour
    {

        public Button playBtn, soundBtn, challengeBtn, removeAdsBtn, leaderboardBtn, achievementsBtn, shopBtn;
        public Button rewardAds;
        public Image soundImage;
        public Sprite[] soundSprite; // 1 is on and 0 is off
        public string gamePlayScene, levelMenuScene;
        public GameObject shopPanel;
        public Text lastScore, bestScore, points;
        private AudioSource sound;

        // Use this for initialization
        void Start()
        {
            //this line of code make the default button of android  visible
            //*Important google feature requirement
            Screen.fullScreen = false;

            bestScore.text = "" + GameManager.instance.bestScore;
            lastScore.text = "" + GameManager.instance.lastScore;
            points.text = "" + GameManager.instance.points;
            shopPanel.SetActive(false);
            sound = GetComponent<AudioSource>();
            if (GameManager.instance.isMusicOn == true)
            {
                AudioListener.volume = 1;
                soundImage.sprite = soundSprite[1];
            }
            else
            {
                AudioListener.volume = 0;
                soundImage.sprite = soundSprite[0];
            }

            playBtn.GetComponent<Button>().onClick.AddListener(() => { PlayBtn(); });    //play
            removeAdsBtn.GetComponent<Button>().onClick.AddListener(() => { RemoveAdsBtn(); });    //noAds
            soundBtn.GetComponent<Button>().onClick.AddListener(() => { SoundBtn(); });  //sound
            leaderboardBtn.GetComponent<Button>().onClick.AddListener(() => { LeaderboardBtn(); });    //leaderboard
            achievementsBtn.GetComponent<Button>().onClick.AddListener(() => { AchievementsBtn(); });    //achievement
            shopBtn.GetComponent<Button>().onClick.AddListener(() => { ShopBtn(); });  //Shop
            rewardAds.GetComponent<Button>().onClick.AddListener(() => { RewaradAdsBtn(); });  //reward ads

            if (!GameManager.instance.canShowAds)
            {
                removeAdsBtn.interactable = false;
            }


        }

        // Update is called once per frame
        void Update()
        {
            points.text = "" + GameManager.instance.points;
            //this is for the android default back button *Important google feature requirement
            //if (Input.GetKeyDown(KeyCode.Escape) && !shopPanel.activeInHierarchy)
            //{
            //    Application.Quit();
            //}
        }

        void PlayBtn()
        {
            sound.Play();

            try
            {
                //AnalyticsTools.Instance.GetUMAnalytics().OnEvent("start_Click");
                UmengGameAnalytics.instance.UpdataEvent("start_Click");
            }
            catch (System.Exception)
            {
                Debug.Log("start_Click");
            }

            GameManager.instance.currentScore = 0;
            SceneManager.LoadScene(gamePlayScene);

            ClickPoint.Instance.InterstitialADNumStart--;

            if (ClickPoint.Instance.InterstitialADNumStart == 0)
            {
                try
                {
                    //GameObject.Find("AdsController").GetComponent<TestInterstitialAd>().HandleShowAdButtonClick();

                }
                catch (System.Exception)
                {

                }
                ClickPoint.Instance.InterstitialADNumStart = 3;
            }

        }

        void SoundBtn()
        {
            sound.Play();
            if (GameManager.instance.isMusicOn == true)
            {
                GameManager.instance.isMusicOn = false;
                AudioListener.volume = 0;
                soundImage.sprite = soundSprite[0];
                GameManager.instance.Save();
            }
            else
            {
                GameManager.instance.isMusicOn = true;
                AudioListener.volume = 1;
                soundImage.sprite = soundSprite[1];
                GameManager.instance.Save();
            }
        }

        void ChallengeBtn()
        {
            sound.Play();
            SceneManager.LoadScene(levelMenuScene);
        }

        /// <summary>
        /// 去除广告按钮（内购）
        /// </summary>
        void RemoveAdsBtn()
        {
            sound.Play();
            //Purchaser.instance.BuyNoAds(); //uncomment after adding respective sdk
            UmengGameAnalytics.instance.PayNoADs();
            GameManager.instance.canShowAds = false;
            //AdsController.instance.HideADs();
            UmengGameAnalytics.instance.UpdataEvent("cost_Click");
        }

        void LeaderboardBtn()
        {
            try
            {
                UmengGameAnalytics.instance.UpdataEvent("rank_Click");
            }
            catch (System.Exception)
            {
                Debug.Log("rank_Click");
            }

            sound.Play();
            GooglePlayManager.singleton.OpenLeaderboardsScore();
        }

        void AchievementsBtn()
        {
            sound.Play();
            GooglePlayManager.singleton.OpenAchievements();
        }

        void ShopBtn()
        {
            sound.Play();
            shopPanel.SetActive(true);
        }

        void RewaradAdsBtn()
        {
            sound.Play();
            //AdsController.instance.ShowRewardedAd();          //uncomment after adding respective sdk
        }



    }
}//namespace
