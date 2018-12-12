using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// It creates the levels
/// </summary>
namespace MadFireOn
{

    public class LevelCreator : MonoBehaviour
    {
        public static LevelCreator instance;

        public Transform spawnPos; //starting position from where blocks will spawn
        public Transform itemHolder; //ref to gameobject where starting items will be spawned as child
        public GameObject hexaPlayer, basePrefab, starLine;
        public GameObject[] blocks;
        public Color32[] newColors; //for more colors check link:- https://www.materialui.co/colors
        float startY, endY; //to get the total distance the hexa have to travel
        private List<GameObject> starList = new List<GameObject>();
        private GameObject playerObj;  //ref to the hexa in the scene
        /// <summary>
        /// ref to current level index and stars achieved
        /// </summary>
        private int currentLevel, starsAchieved;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //this is the index of the level player has selected
            int currentLevel = GameManager.instance.currentLevel;
            //this is to check how many stars player has achieved in selected level
            int starsAchieved = GameManager.instance.starAchieved[currentLevel];

            startY = spawnPos.position.y;
            StartSpawn();
            StarLineSpawn();
            //here we check number of stars achieved in the level and deactivate the
            //collider of star line which increase the points when hexa collides with it
            //this is because the player wont get points for each try
            for (int i = 0; i < starsAchieved; i++)
            {
                starList[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if player is failed then no code will be run below this code
            if (LevelGuiManager.instance.levelFailed)
                return;
            //here we get the total star list (the object which contain star and a line)
            //and keep track of hexa position along y axis
            for (int i = 0; i < starList.Count; i++)
            {
                //if the starline object y values become more than hexa ,that means is hexa is moving down
                //then we check which starline object is that and make it deactive
                //and make the gui star on the top yellow
                if (starList[i].transform.position.y > (playerObj.transform.position.y))
                {
                    starList[i].SetActive(false);
                    LevelGuiManager.instance.starImgs[i].color = new Color32(255, 211, 43,255);
                }
            }
            //when the level is complete the 3rd starline is set to deactive
            //and last star is set to yellow
            if (LevelGuiManager.instance.levelComplete)
            {
                starList[2].SetActive(false);
                LevelGuiManager.instance.starImgs[2].color = new Color32(255, 211, 43, 255);
            }
        }

        //method which spawn stating items
        void StartSpawn()
        {
            Vector2 playerPos = spawnPos.position + new Vector3(0, 0.78f, 0);
            //spawns that hexa 
            GameObject player = (GameObject)Instantiate(hexaPlayer, playerPos, Quaternion.identity);
            playerObj = player;
            player.transform.parent = itemHolder;
            //spawns blocks at the remaining position
            for (int i = 0; i < blocks.Length + 1; i++)
            {
                Vector2 blockPos = spawnPos.position;
                if (i < blocks.Length)
                {
                    //get the block from object pooler
                    GameObject blockObj = (GameObject)Instantiate(blocks[i], blockPos, Quaternion.identity);
                    blockObj.transform.parent = itemHolder;
                    BlockScript blockScript = blockObj.GetComponent<BlockScript>();//get the script attached on the block

                    //we go through each pieces and change there color properties
                    for (int m = 0; m < blockScript.pieces.Count; m++)
                    {
                        //set the color for pieces
                        blockScript.pieces[m].pieceSprite.color = newColors[Random.Range(0, newColors.Length)];
                    }
                    spawnPos.position = blockPos - new Vector2(0, 3.1f);
                }
                else
                {
                    //when all the blocks are spawn at the end we spawn the base
                    GameObject baseObj = (GameObject)Instantiate(basePrefab, blockPos, Quaternion.identity);
                    baseObj.transform.parent = itemHolder;
                    endY = baseObj.transform.position.y;
                }
            }
        }

        void StarLineSpawn()
        {
            //here we calculate the distance
            float distance = Mathf.Abs(endY - startY);
            //and place the starline object at the 3 position which is obtain by dividing distance 3 times
            Vector2 tempPos = new Vector2(0, - (distance / 3f)); 
            for (int i = 0; i < 3; i++)
            {
                //here we spawn the starline object
                GameObject starLineObj = (GameObject)Instantiate(starLine, tempPos, Quaternion.identity);
                starLineObj.transform.parent = itemHolder;
                tempPos = tempPos - new Vector2(0, distance / 3f);
                // we add starline object to the list as we will need the ref to every starline object
                starList.Add(starLineObj);
            }
        }

    }//class
}//namespace