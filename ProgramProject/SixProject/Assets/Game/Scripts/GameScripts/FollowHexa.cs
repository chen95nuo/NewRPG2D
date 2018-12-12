using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Script attached to the camera to follow hexa player object
/// </summary>
namespace MadFireOn
{
    public class FollowHexa : MonoBehaviour
    {
        
        public static FollowHexa instance;

        private Vector2 velocity;
        private GameObject player; //ref to the player object in the scene
        private float distance; //distance between camera and player
        private float cameraY; //updated camera position
        private float lastCameraY; // position saved when the last block was spawned
        private BlockSpawner blockSpawner; //ref to block spawner object
        private bool playerGot = false;   //check if player is available

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //gets the blockspawner object
            if (GameManager.instance.sceneName == "MainGame")
            {
                blockSpawner = transform.Find("BlockSpawner").GetComponent<BlockSpawner>();
            }
            cameraY = transform.position.y; //gets the latest poisiton y
            lastCameraY = cameraY; // set it equal to last positon y
            //the difference between cameraY and lastCameraY helps tp spawn new block
        }
        //method which will be called by hexaplayer when the hexaplayer spawns
        public void PlayerSettings()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            //gets the distance between player and camera , we need this distance so to maintain it in the game
            distance = (player.transform.position.y - transform.position.y);
            playerGot = true;
        }

        void Update()
        {
            if (!GameManager.instance.gameOver && GameManager.instance.sceneName == "MainGame")
                CheckForY();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!GameManager.instance.gameOver && playerGot)
                Movement();
        }
        //methode which make camera move smoothly
        void Movement()
        {
            float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y - distance, ref velocity.y, 0.05f);
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
        //method which spawn new block when the distance is more than required
        void CheckForY()
        {
            cameraY = transform.position.y;
            if (Mathf.Abs(cameraY - lastCameraY) > 3.1f)
            {//reset the lastCameraY position
                lastCameraY = cameraY;
                //call the method from the blockspawner script
                blockSpawner.SpawnBlock();
            }
        }
       
    }
}//namespace