using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MadFireOn
{
    [System.Serializable]
    public class LevelItems
    {
        public string levelName; // scene name which to load
        public int levelIndex; //number on the level icon
        public string levelNum; //level number
        [HideInInspector]
        public bool unLock;
        [HideInInspector]
        public int starAchieved;
        public Button.ButtonClickedEvent thingToDo;
    }

    public class LevelScrollList : MonoBehaviour
    {
        public static LevelScrollList instance;

        public Text points, levelSolved;
        public Button homeBtn;                    //ref to home button
        public string homeScene;                  //ref to home scene name
        public GameObject refButton;              //ref to sample button prefab
        public Transform scrollPanel;             //ref to scroll panel where all the buttons will be Instantiated

        public LevelItems[] levels;                    //levels array

        void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            //to hide banner ads in the level select scene
            //bannerView.Hide()

            LevelSolved();

            //the methode called when we click home button
            homeBtn.GetComponent<Button>().onClick.AddListener(() => { HomeButton(); });

            /// <summary>
            /// we check how many levels are unlocked and how many stars are achieved
            /// before loading all the buttons
            /// </summary>
            for (int i = 0; i < GameManager.instance.levels.Length; i++)
            {
                levels[i].unLock = GameManager.instance.levels[i];
                levels[i].starAchieved = GameManager.instance.starAchieved[i];
            }

            //handle the requirement for each level as per the list
            foreach (LevelItems i in levels)
            {

                GameObject btn = Instantiate(refButton);//here we get ref to the instanciated button

                SampleButton samBtn = btn.GetComponent<SampleButton>();//ref to the script of button

                samBtn.levelIndex = i.levelIndex; //here we set index to keep track whihc level is on
                samBtn.levelNum.text = i.levelNum;//here we set the number of level button
                samBtn.levelName = i.levelName; //here we set the name os scene which to be loaded

                //if level is unlocked we do following
                if (i.unLock == true)
                {
                    samBtn.lockObj.gameObject.SetActive(false);
                    samBtn.levelNum.color = Color.black;
                    for (int j = 0; j < i.starAchieved; j++)
                    {
                        samBtn.stars[j].color = new Color32(255, 211, 43, 255);
                    }

                    samBtn.button.interactable = true;
                }
                //if not we do following
                else
                {
                    samBtn.lockObj.gameObject.SetActive(true);

                    samBtn.button.interactable = false;
                }


                samBtn.button.onClick = i.thingToDo;

                btn.transform.SetParent(scrollPanel);
                //we set the scale of button fo by default they dot become too large or too small
                btn.transform.localScale = new Vector3(1f, 1f, 1f);

            }

        }

        // Update is called once per frame
        void Update()
        {
            //keep updating points
            points.text = "" + GameManager.instance.points;
        }

        void HomeButton()
        {
            SceneManager.LoadScene(homeScene);
        }

        void LevelSolved()
        {
            int j = 0;

            for (int i = 0; i < GameManager.instance.levels.Length; i++)
            {
                if (GameManager.instance.levels[i] == true)
                {
                    j++;
                }
            }

            int totalLevels = GameManager.instance.totalLevel;

            levelSolved.text = "" + j + "/" + totalLevels;
        }



    }//class
}//namespace