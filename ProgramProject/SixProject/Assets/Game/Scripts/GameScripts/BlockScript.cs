using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This script is attached to the block object which contains the pieces
/// </summary>
namespace MadFireOn
{
    public class BlockScript : MonoBehaviour
    {
        //[HideInInspector]
        public List<PieceScript> pieces = new List<PieceScript>(); //list which stores the child elements
        int childNum; //to keep track of total child count


        void Awake()
        {
            childNum = pieces.Count; //get the child count
        }
        
        void Start()
        {
           
        }       

        // Update is called once per frame
        void Update()
        {
            DeactivateBlock();
        }
        //method which deactivates the block when all the children are deactivted
        void DeactivateBlock()
        {
            //this loop check for the active childrens
            for (int i = 0; i < childNum; i++)
            {
                //Checks if the piece is enabled.
                if (pieces[i].isActiveAndEnabled)
                    return;
            }
            //if all the children are deactive we get out of the for loop and execute below code


            //here we want to give points after solving 2 blocks
            //so we get the total blocks solved divide by 2 and check for remainder
            //if remainder is zero we add one point
            if (GameManager.instance.sceneName == "MainGame")
            {
                GameManager.instance.blocksSolved++;
                if (GameManager.instance.blocksSolved > 0 && GameManager.instance.blocksSolved % 2 == 0)
                {
                    GameManager.instance.points++;
                    GameManager.instance.Save();
                }
            }
            Debug.Log("sss");
            //when all the children are deactive we deactivate the block
            gameObject.SetActive(false);
        }
    }

}//namespace