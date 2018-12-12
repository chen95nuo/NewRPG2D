using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Script which detects the input from player
/// </summary>
namespace MadFireOn
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;

        [SerializeField]
        private LayerMask pieceLayer; //ref to layer 
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
            //at start we want score to be zero
        }

        // Update is called once per frame
        void Update()
        {
            //we check for 3 conditions
            //1: mouse clicked , 2: game is not over , 3: game is not paused
            //if (GameManager.instance.sceneName == "MainGame")
            //{
                if (Input.GetMouseButtonDown(0) && !GameManager.instance.gameOver && !InGameGUI.instance.gamePause)
                {
                    DetectPiece();
                }
            //}
            //else
            //{
            //    if (Input.GetMouseButtonDown(0) && !GameManager.instance.gameOver && !LevelGuiManager.instance.gamePause)
            //    {
            //        DetectPiece();
            //    }
            //}

            

        }

        //method which detects the piece
        void DetectPiece()
        {//creates the ray at mouse click
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //add physics to the ray
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, pieceLayer);
            //checks if it has collided with any piece
            if (hit.collider != null)
            {
                sound.Play();
                if (GameManager.instance.sceneName == "MainGame")
                {
                    //here with every click on pieces we increase score by 5
                    GameManager.instance.AddScore(5);
                }
                //we deactivate the piece and create particle effect
                PieceScript script = hit.collider.GetComponent<PieceScript>();
                //Color32 newColor = script.pieceSprite.color;                        //get the color of piece
                GameObject explosion = ObjectPooling.instance.GetExplosionEffect(); //creates the explosion
                explosion.transform.position = ray.origin;                          //change its position to tap pos
                explosion.transform.rotation = Quaternion.identity;                 //change its rotation
                //explosion.GetComponent<ParticleSystem>().startColor = newColor;     //change its color to piece color
                explosion.SetActive(true);                                          //activate it
                hit.collider.gameObject.SetActive(false);                           //then deactivate the piece

                Debug.Log("Creat");

                try
                {
                    GameObject.Find("RewardButton").gameObject.SetActive(false);
                    GameObject.Find("HandTap").gameObject.SetActive(false);
                    ClickPoint.Instance.isShowTop = false;

                }
                catch (System.Exception)
                {

                    //Debug.Log("Can not see Ads");
                }

            }
        }
    }
}//namespace