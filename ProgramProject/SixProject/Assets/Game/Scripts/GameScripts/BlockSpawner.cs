using UnityEngine;
using System.Collections;

namespace MadFireOn
{
    public class BlockSpawner : MonoBehaviour
    {
        public static BlockSpawner instance;

        //public Color32[] newColors; //for more colors check link:- https://www.materialui.co/colors
        //this below variable is to spawn some starting blocks and player and are called only in start
        public GameObject[] hexaPlayer, blocks;
        public Transform[] spawnPos; // 0 is for player and others are for blocks 
        public GameObject startingItems; //ref to gameobject where starting items will be spawned as child

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            StartSpawn();//we spawn few object when the game starts
        }

        // Update is called once per frame
        void Update()
        {

        }
        //method which spawn stating items
        void StartSpawn()
        {
            //gets which hexa skin is selected
            int selectedHexa = GameManager.instance.selectedSkin;
            //spawns that hexa 
            GameObject player = (GameObject)Instantiate(hexaPlayer[selectedHexa], spawnPos[0].position, Quaternion.identity);
            //spawns blocks at the remaining position
            for (int i = 1; i < spawnPos.Length; i++)
            {
                //get the block from object pooler
                GameObject blockObj = (GameObject)Instantiate(blocks[Random.Range(0,blocks.Length)], spawnPos[i].position, Quaternion.identity);
                BlockScript blockScript = blockObj.GetComponent<BlockScript>();//get the script attached on the block

                //we go through each pieces and change there color properties
                for (int m = 0; m < blockScript.pieces.Count; m++)
                {
                    //set the color for pieces
                    //blockScript.pieces[m].pieceSprite.color = newColors[Random.Range(0, newColors.Length)];
                }

            }
        }
        //method which keep spawning block when the player solves one
        public void SpawnBlock()
        {
            //get the block
            GameObject block = ObjectPooling.instance.GetSpawnedBlock();
            //assign the position in the world
            block.transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
            //assign the rotation
            block.transform.rotation = Quaternion.identity;
            BlockScript blockScript = block.GetComponent<BlockScript>();//get reference to the script on the block
            //check if list of pieces has any element
            if (blockScript.pieces.Count > 0)
            {
                //if yes goes through all the pieces and apply settings
                for (int i = 0; i < blockScript.pieces.Count; i++)
                {//call the reset methode which resets the position and rotation of block as default
                    blockScript.pieces[i].ResetPosition();

#if UNITY_5_4_OR_NEWER
                    blockScript.pieces[i].isRayOn = false;
#endif
                   // blockScript.pieces[i].isKinematic = true;//deactivates it physics effects

                    //give random color from the color range we have 
                    //blockScript.pieces[i].pieceSprite.color = newColors[Random.Range(0,newColors.Length)];
                    blockScript.pieces[i].gameObject.SetActive(true); //activate the piece
                }
            }
            //then activate the block
            block.SetActive(true);
        }

    }
}//namespace