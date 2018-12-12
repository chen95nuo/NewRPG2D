using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MadFireOn
{
    public class LevelGuiManager : MonoBehaviour
    {
        public static LevelGuiManager instance;

        #region Public Variables        
        public Image[] starImgs; //ref to star images in the scene
        public Image soundImage; //ref to sound button image
        public Sprite[] soundSprite; // 1 is on and 0 is off
        public GameObject pausePanel; //ref to pause panel
        public Text points, levelStatus; //ref to text elements in the scene
        public string levelMenu; //ref to the name of level menu scene
        [HideInInspector]//few bools to control the game status
        public bool gamePause = false, levelComplete = false, levelFailed = false;
        #endregion

        #region Private Variables
        private AudioSource sound;
        int currentLevel; //ref to level index
        #endregion


        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            Screen.fullScreen = true;
            sound = GetComponent<AudioSource>();
            //at start of game scene we want game over false
            GameManager.instance.gameOver = false;
            points.text = "" + GameManager.instance.points;
            //sets the sound button and audio
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

            //this is the index of the level player has selected
            currentLevel = GameManager.instance.currentLevel;
            //this is to check how many stars player has achieved in selected level
            int starsAchieved = GameManager.instance.starAchieved[currentLevel];
            //we then at start make the cretain amount of stars active
            for (int i = 0; i < starsAchieved; i++)
            {
                starImgs[i].color = new Color32(255, 211, 43, 255);
            }

        }

        // Update is called once per frame
        void Update()
        {
            //keep updating points
            points.text = "" + GameManager.instance.points;

            //when level is completed
            if (levelComplete)
            {
                //activate the levelStatus text
                levelStatus.gameObject.SetActive(true);
                // set its value
                levelStatus.text = "LevelCompleted!";
                //unlock the next level
                if (currentLevel + 1 < GameManager.instance.totalLevel)
                {
                    GameManager.instance.levels[currentLevel + 1] = true;
                }
                //save it to the device
                GameManager.instance.Save();
                //go to level menu
                StartCoroutine(GoToLevelMenu());
            }
            //if level is failed
            if (levelFailed)
            {
                //activate the levelStatus text
                levelStatus.gameObject.SetActive(true);
                // set its value
                levelStatus.text = "LevelFailed!";
                //go to level menu
                StartCoroutine(GoToLevelMenu());
            }
        }

        public void PauseBtn()
        {
            sound.Play();
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            gamePause = true;
        }

        public void ResumeBtn()
        {
            sound.Play();
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            gamePause = false;
        }

        public void HomeBtn()
        {
            sound.Play();
            Time.timeScale = 1;
            gamePause = false;
            SceneManager.LoadScene(levelMenu);
        }

        public void SoundBtn()
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

        public void AboutUsBtn()
        {
            sound.Play();
        }

        IEnumerator GoToLevelMenu()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(levelMenu);
        }

    }//class
}//namespace