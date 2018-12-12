using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace MadFireOn
{

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        private GameData data;

        #region Variables not saved on device
        [HideInInspector]
        public string sceneName;
        [HideInInspector]
        public bool gameOver = false;
        [HideInInspector]
        public int currentScore;
        [HideInInspector]
        public int blocksSolved = 0;
        [HideInInspector]
        public int starsGot;
        public bool levelComplete;
        public int currentLevel = 0; //this is used to unlock levels because the array start from zero
        ////this is for change from one level to another when player completes level , it start from level 1
        //public int currentLevelNumber = 0;
        #endregion

        #region Variables saved on device
        //variables which are saved on the device
        [HideInInspector]
        public bool isGameStartedFirstTime;
        [HideInInspector]
        public bool canShowAds;
        [HideInInspector]
        public bool isMusicOn;
        [HideInInspector]
        public bool fbBtnClicked, twitterBtnClicked;
        [HideInInspector]
        public int bestScore, lastScore;
        //[HideInInspector]
        public bool[] skinUnlocked, achievements;
        //[HideInInspector]
        public int selectedSkin;
        //[HideInInspector]
        public int points; //to buy new skins
        public int totalLevel = 5;
        public bool[] levels; // to keep track on levels
        //[HideInInspector]
        public int[] starAchieved;
        #endregion

        void Awake()
        {
            MakeSingleton();
            InitializeGameVariables();
        }

        // Use this for initialization
        void Start()
        {
        }

        void MakeSingleton()
        {
            //this state that if the gameobject to which this script is attached , if it is present in scene then destroy the new one , and if its not present
            //then create new 
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnLevelWasLoaded()
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        void InitializeGameVariables()
        {
            Load();
            if (data != null)
            {
                isGameStartedFirstTime = data.getIsGameStartedFirstTime();
            }
            else
            {
                isGameStartedFirstTime = true;
            }

            if (isGameStartedFirstTime)
            {
                isGameStartedFirstTime = false;
                isMusicOn = true;
                canShowAds = true;
                bestScore = lastScore = 0;
                points = 10;

                levels = new bool[totalLevel];
                starAchieved = new int[totalLevel];

                levels[0] = true;
                starAchieved[0] = 0;

                for (int i = 1; i < levels.Length; i++)
                {
                    levels[i] = false;
                    starAchieved[i] = 0;
                }

                skinUnlocked = new bool[12];//if you want ot add more skins change the value here
                skinUnlocked[0] = true;
                for (int i = 1; i < skinUnlocked.Length; i++)
                {
                    skinUnlocked[i] = false;
                }
                selectedSkin = 0;

                achievements = new bool[5];//if you want ot add more achievements change the value here
                for (int i = 0; i < achievements.Length; i++)
                {
                    achievements[i] = false;
                }


                fbBtnClicked = twitterBtnClicked = false;
                

                data = new GameData();

                data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                data.setMusicOn(isMusicOn);
                data.setCanShowAds(canShowAds);
                data.setFbClick(fbBtnClicked);
                data.setTwitterClick(twitterBtnClicked);
                data.setBestScore(bestScore);
                data.setLastScore(lastScore);
                data.setSkinUnlocked(skinUnlocked); //add this line 
                data.setPoints(points);
                data.setSelectedSkin(selectedSkin);   
                data.setAchievementsUnlocked(achievements);
                data.setLevels(levels);
                data.setStarsAchieved(starAchieved);

                Save();

                Load();
            }
            else
            {
                isGameStartedFirstTime = data.getIsGameStartedFirstTime();
                isMusicOn = data.getMusicOn();
                canShowAds = data.getCanShowAds();
                fbBtnClicked = data.getFbClick();
                twitterBtnClicked = data.getTwitterClick();
                bestScore = data.getBestScore();
                lastScore = data.getLastScore();
                points = data.getPoints();
                selectedSkin = data.getSelectedSkin();
                skinUnlocked = data.getSkinUnlocked();
                achievements = data.getAchievementsUnlocked();
                levels = data.getLevels();
                starAchieved = data.getStarsAchieved();
            }
        }


        //                              .........this function take care of all saving data like score , current player , current weapon , etc
        public void Save()
        {
            FileStream file = null;
            //whicle working with input and output we use try and catch
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                file = File.Create(Application.persistentDataPath + "/GameData.dat");

                if (data != null)
                {
                    data.setLevels(levels);
                    data.setStarsAchieved(starAchieved);
                    data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                    data.setMusicOn(isMusicOn);
                    data.setCanShowAds(canShowAds);
                    data.setFbClick(fbBtnClicked);
                    data.setTwitterClick(twitterBtnClicked);
                    data.setBestScore(bestScore);
                    data.setLastScore(lastScore);
                    data.setSkinUnlocked(skinUnlocked);
                    data.setPoints(points);
                    data.setSelectedSkin(selectedSkin);
                    data.setAchievementsUnlocked(achievements);
                    bf.Serialize(file, data);
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }


        }
        //                            .............here we get data from save
        public void Load()
        {
            FileStream file = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
                data = (GameData)bf.Deserialize(file);

            }
            catch (Exception e)
            {
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        //for resetting the gameManager

        public void ResetGameManager()
        {
            isGameStartedFirstTime = false;
            isMusicOn = true;
            canShowAds = true;

            bestScore = lastScore = 0;
            points = 10;

            levels = new bool[totalLevel];
            starAchieved = new int[totalLevel];

            levels[0] = true;
            starAchieved[0] = 0;
            for (int i = 1; i < levels.Length; i++)
            {
                levels[i] = false;
                starAchieved[i] = 0;
            }

            skinUnlocked = new bool[12];//if you want ot add more skins change the value here

            skinUnlocked[0] = true;

            for (int i = 1; i < skinUnlocked.Length; i++)
            {
                skinUnlocked[i] = false;
            }

            achievements = new bool[5];//if you want ot add more achievements change the value here
            for (int i = 0; i < achievements.Length; i++)
            {
                achievements[i] = false;
            }

            fbBtnClicked = twitterBtnClicked = false;
            

            data = new GameData();

            data.setIsGameStartedFirstTime(isGameStartedFirstTime);
            data.setMusicOn(isMusicOn);
            data.setCanShowAds(canShowAds);
            data.setFbClick(fbBtnClicked);
            data.setTwitterClick(twitterBtnClicked);
            data.setBestScore(bestScore);
            data.setLastScore(lastScore);
            data.setSkinUnlocked(skinUnlocked);
            data.setPoints(points);
            data.setLevels(levels);
            data.setStarsAchieved(starAchieved);
            data.setSelectedSkin(selectedSkin);
            data.setAchievementsUnlocked(achievements);
            Save();
            Load();

            Debug.Log("GameManager Reset");
        }

        public void AddScore(int _Gold)
        {
            this.currentScore += _Gold;
        }
    }

    [Serializable]
    class GameData
    {
        private bool isGameStartedFirstTime;
        private bool isMusicOn;
        private bool canShowAds;
        private bool fbBtnClicked, twitterBtnClicked;
        private int bestScore, lastScore;
        private bool[] skinUnlocked, achievements;
        private int selectedSkin;
        private int points; //to buy new skins
        private bool[] levels; //this keep track of which level is locked and which is not
        private int[] starAchieved;

        public void setCanShowAds(bool canShowAds)
        {
            this.canShowAds = canShowAds;
        }

        public bool getCanShowAds()
        {
            return this.canShowAds;
        }

        public void setIsGameStartedFirstTime(bool isGameStartedFirstTime)
        {
            this.isGameStartedFirstTime = isGameStartedFirstTime;

        }

        public bool getIsGameStartedFirstTime()
        {
            return this.isGameStartedFirstTime;

        }
        //                                                                    ...............music
        public void setMusicOn(bool isMusicOn)
        {
            this.isMusicOn = isMusicOn;

        }

        public bool getMusicOn()
        {
            return this.isMusicOn;

        }
        //                                                                      .......music
        
        //....................................................for fb btn
        public void setFbClick(bool fbBtnClicked)
        {
            this.fbBtnClicked = fbBtnClicked;

        }

        public bool getFbClick()
        {
            return this.fbBtnClicked;

        }

        //....................................................for twitter btn
        public void setTwitterClick(bool twitterBtnClicked)
        {
            this.twitterBtnClicked = twitterBtnClicked;

        }

        public bool getTwitterClick()
        {
            return this.twitterBtnClicked;

        }
        //best score
        public void setBestScore(int bestScore)
        {
            this.bestScore = bestScore;
        }

        public int getBestScore()
        {
            return this.bestScore;
        }
        //last score
        public void setLastScore(int lastScore)
        {
            this.lastScore = lastScore;
        }

        public int getLastScore()
        {
            return this.lastScore;
        }

        //points
        public void setPoints(int points)
        {
            this.points = points;
        }

        public int getPoints()
        {
            return this.points;
        }

        //skin unlocked
        public void setSkinUnlocked(bool[] skinUnlocked)
        {
            this.skinUnlocked = skinUnlocked;
        }

        public bool[] getSkinUnlocked()
        {
            return this.skinUnlocked;
        }

        //selectedSkin
        public void setSelectedSkin(int selectedSkin)
        {
            this.selectedSkin = selectedSkin;
        }

        public int getSelectedSkin()
        {
            return this.selectedSkin;
        }

        //achievements unlocked
        public void setAchievementsUnlocked(bool[] achievements)
        {
            this.achievements = achievements;
        }

        public bool[] getAchievementsUnlocked()
        {
            return this.achievements;
        }

        //                                                                       ..................Level locked/unlocked
        public void setLevels(bool[] levels)
        {
            this.levels = levels;

        }

        public bool[] getLevels()
        {
            return this.levels;

        }
        //                                                                       ..................Level locked/unlocked
        //stars achieved
        public void setStarsAchieved(int[] starAchieved)
        {
            this.starAchieved = starAchieved;
        }

        public int[] getStarsAchieved()
        {
            return this.starAchieved;
        }

    }
}//namespace MadFireOn
