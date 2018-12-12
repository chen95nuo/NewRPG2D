using UnityEngine;
using System.Collections;
/// <summary>
/// Script for the hexa object
/// </summary>
namespace MadFireOn
{
    public class PlayerHexa : MonoBehaviour
    {

        private Rigidbody2D mybody;
        // Use this for initialization
        void Start()
        {
            mybody = GetComponent<Rigidbody2D>();
            FollowHexa.instance.PlayerSettings();
        }

        // Update is called once per frame
        void Update()
        {
            //when the hexa object falls of the blocks and goes beyond limits game is over
            if (transform.position.x <= -2 || transform.position.x >= 2 && GameManager.instance.gameOver != true)
            {
                //Add you ads code here.
                GameManager.instance.gameOver = true;
                StartCoroutine(DeactivateGravity()); //and after some time its gravity is deactivated
#if AdmobDef
                AdsManager.instance.ShowInterstitial();
#endif
                if (GameManager.instance.sceneName != "MainGame")
                {
                    LevelGuiManager.instance.levelFailed = true;
                    //remove ads code from here
                }

            }

        }

        IEnumerator DeactivateGravity()
        {
            yield return new WaitForSeconds(2f);
            mybody.isKinematic = true;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Base")
            {
                if (GameManager.instance.sceneName != "MainGame")
                {
                    LevelGuiManager.instance.levelComplete = true;
                }
            }
        }

    }
}//namespace