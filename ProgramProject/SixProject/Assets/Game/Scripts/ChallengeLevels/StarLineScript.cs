using UnityEngine;
using System.Collections;

namespace MadFireOn
{
    public class StarLineScript : MonoBehaviour {

        private int i = 0;
        int currentLevel; //ref to current level index

        // Use this for initialization
        void Start()
        {
            currentLevel = GameManager.instance.currentLevel;
        }

        // Update is called once per frame
        void Update() {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //i is use so that if the player object is detected twice the 
            //points wont increase by twice
            if (other.CompareTag("Player") && i == 0)
            {
                i = 1;
                GameManager.instance.points++; //increase points
                GameManager.instance.starAchieved[currentLevel]++;//increase stars achieved
                GameManager.instance.Save();
            }
        }

    }//class
}//namespace