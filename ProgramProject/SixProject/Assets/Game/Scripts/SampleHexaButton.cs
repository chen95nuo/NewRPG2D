using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MadFireOn
{

    public class SampleHexaButton : MonoBehaviour {

        public static SampleHexaButton instance;

        public GameObject lockImg, costHolder, tickImg;
        [HideInInspector]
        public int starCost;     //need to be assigned by createScrollList
        public Image hexaSkin;   //need to be assigned by createScrollList
        public Text cost;        //need to be assigned by createScrollList
        public Button button;
        [HideInInspector]        //need to be assigned by createScrollList
        public int skinIndex;    //this the index which is respective to the "skinUnlocked" bool array in GameManager
        private AudioSource sound;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            sound = GetComponent<AudioSource>();
            button.GetComponent<Button>().onClick.AddListener(() => { ButtonPressed(); });

        }

        // Update is called once per frame
        void Update()
        {
            //if the hexa is unlocked we will set his star cost to zero
            if (GameManager.instance.skinUnlocked[skinIndex] == true)
            {
                starCost = 0;
            }

            if (tickImg.activeInHierarchy)
            {
                if (skinIndex != GameManager.instance.selectedSkin)
                {
                    tickImg.SetActive(false);
                }
            }

            if (GameManager.instance.points < starCost)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }

        //methode called when we press the button
        public void ButtonPressed()
        {
            //here we check if the skin is unlocked
            if (GameManager.instance.skinUnlocked[skinIndex] == true)
            {
                //if yes we select the skin
                tickImg.SetActive(true);
                GameManager.instance.selectedSkin = skinIndex;
                GameManager.instance.Save();
                sound.Play();
            }
            else
            {//if no we check for the cost and total points player has
                if (GameManager.instance.points >= starCost)
                {//if points are more or equal to the required points
                    //the cost amount is deducted from the total points
                    GameManager.instance.points -= starCost;
                    //the respective skin is unlocked
                    GameManager.instance.skinUnlocked[skinIndex] = true;
                    //the respective skin is selected
                    GameManager.instance.selectedSkin = skinIndex;
                    //all the dala is then stored in the device
                    GameManager.instance.Save();
                    lockImg.SetActive(false);
                    costHolder.SetActive(false);
                    tickImg.SetActive(true);
                    sound.Play();
                }
            }

        }
    }
}//namespace