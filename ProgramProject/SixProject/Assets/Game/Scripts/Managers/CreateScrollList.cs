using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//this script creates the scrolling shop buttons in shop menu
namespace MadFireOn
{
    [System.Serializable]
    public class Items
    {
        public Sprite skinSprite;
        public int skinIndex;
        public int skinCost;
        [HideInInspector]
        public bool unLock;
        public Button.ButtonClickedEvent thingToDo;
    }

    public class CreateScrollList : MonoBehaviour
    {

        public static CreateScrollList instance;

        [SerializeField]
        private string fbPageLink, twitterPageLink;
        public Button facebookLikeBtn, twitterFollowBtn;                    
        public GameObject shopPanel;
        public Items[] skins;                    //skins array
        public Text totalPointsShopPanel;
        public GameObject refButton;              //ref to sample button prefab

        public Transform scrollPanel;             //ref to scroll panel where all the buttons will be Instantiated

        private AudioSource sound;

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

        void Start()
        {
            totalPointsShopPanel.text = "" + GameManager.instance.points;
            sound = GetComponent<AudioSource>();
            facebookLikeBtn.GetComponent<Button>().onClick.AddListener(() => { FacebookLikeBtn(); });
            twitterFollowBtn.GetComponent<Button>().onClick.AddListener(() => { TwitterFollowBtn(); });

            //we check how many skins are unlocked before loading all the buttons
            for (int i = 0; i < GameManager.instance.skinUnlocked.Length; i++)
            {
                skins[i].unLock = GameManager.instance.skinUnlocked[i];
            }

            //handle the requirement for each level as per the list
            foreach (Items i in skins)
            {

                GameObject btn = Instantiate(refButton);//here we get ref to the instanciated button

                SampleHexaButton samBtn = btn.GetComponent<SampleHexaButton>();//ref to the script of button

                samBtn.skinIndex = i.skinIndex;     //here we set index to keep track which skin is selected
                samBtn.starCost = i.skinCost;      //here we set the cost of skin
                samBtn.cost.text = "" + samBtn.starCost;     //here we set the cost text of skin
                samBtn.hexaSkin.sprite = i.skinSprite;       //here we set the skin image of button
                //if skin is unlocked we do following
                if (i.unLock == true)
                {
                    //deactivate the lock image
                    samBtn.lockImg.SetActive(false);
                    //deactivate the cost holder
                    samBtn.costHolder.SetActive(false);
                }
                //if not we do following
                else
                {
                    //activate the lock image
                    samBtn.lockImg.SetActive(true);
                    //activate the cost holder
                    samBtn.costHolder.SetActive(true);
                }

                if (i.skinIndex == GameManager.instance.selectedSkin)
                {
                    samBtn.tickImg.SetActive(true);
                }
                else
                {
                    samBtn.tickImg.SetActive(false);
                }

                samBtn.button.onClick = i.thingToDo;
                samBtn.button.interactable = true;
                btn.transform.SetParent(scrollPanel);
                //we set the scale of button fo by default they dot become too large or too small
                btn.transform.localScale = new Vector3(1f, 1f, 1f);

            }
        }

        void Update()
        {
            totalPointsShopPanel.text = "" + GameManager.instance.points;

            if (GameManager.instance.fbBtnClicked)
            {
                facebookLikeBtn.interactable = false;
            }

            if (GameManager.instance.twitterBtnClicked)
            {
                twitterFollowBtn.interactable = false;
            }
            //this is for the android default back button *Important google feature requirement
            if (Input.GetKeyDown(KeyCode.Escape) && shopPanel.activeInHierarchy)
            {
                shopPanel.SetActive(false);
            }

        }

        public void HomeButton()
        {
            sound.Play();
            shopPanel.SetActive(false);
        }

        void FacebookLikeBtn()
        {
            sound.Play();
            //1st the url is opened
            Application.OpenURL(fbPageLink);
            GameManager.instance.fbBtnClicked = true;
            //points is increased by 10
            GameManager.instance.points += 10;
            //its then save
            GameManager.instance.Save();
        }

        void TwitterFollowBtn()
        {
            sound.Play();
            //1st the url is opened
            Application.OpenURL(twitterPageLink);
            GameManager.instance.twitterBtnClicked = true;
            //points is increased by 10
            GameManager.instance.points += 10;
            //its then save
            GameManager.instance.Save();
        }



    }
}//namespace