using UnityEngine;
using System.Collections;
/// <summary>
/// Script which controls the behaviour of the piece object
/// </summary>
namespace MadFireOn
{
    public class PieceScript : MonoBehaviour
    {

        Rigidbody2D myBody;                //ref to rigidbody of the piece
        public bool isKinematic = true, isRayOn = false;    // when piece becomes active in scene we want its gravity off
        private Vector3 defaultPos;        //variable to store the local position of piece
        private Quaternion defaultRot;     //variable to store the local rotation of piece
        public SpriteRenderer pieceSprite; // ref to sprite

        //below code is only used when unity editor is 5.4+
        private string pieceName;          //store name of the piece
        public Transform[] rayPos;         //position for the ray to spawn
        public LayerMask pieceLayer;       //raf to the layer on which objects are assigned
        public bool[] rayHitting;          


        void Awake()
        {
            if (pieceSprite == null)
            {
                pieceSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
            myBody = GetComponent<Rigidbody2D>(); //we get the component attached to the object
            defaultPos = transform.localPosition; //get the default values
            defaultRot = transform.localRotation;

#if UNITY_5_4_OR_NEWER
            pieceName = gameObject.name;
#endif
        }

        // Use this for initialization
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

#if UNITY_5_4_OR_NEWER

            if (isRayOn)
            {
                DetectThePiece();
            }

#endif

            if (!isKinematic) //check if isKinematic bool is true or false
            {
                myBody.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                myBody.bodyType = RigidbodyType2D.Static;
                myBody.simulated = true;
            }

            if (Mathf.Abs(myBody.velocity.y) >= 20f)
            {
                gameObject.SetActive(false);
            }

            if (GameObject.FindGameObjectWithTag("Player").transform.position.y - gameObject.transform.position.y < 6)
            {
                isKinematic = false;
            }
            else {
                isKinematic = true;

            }


        }
        //methode used to reset the pieces values when they are reused to spawn
        public void ResetPosition()
        {
            transform.localPosition = defaultPos;
            transform.localRotation = defaultRot;
        }

#if UNITY_5_4_OR_NEWER
        //ray is used to detect the other pieces below the piece
        //this ray help to make body apply physcis , its is used only in Unity 5.4+
        void DetectThePiece()
        {
            for (int i = 0; i < rayPos.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(rayPos[i].position, -Vector3.up, 0.05f, pieceLayer);
                Debug.DrawRay(rayPos[i].position, -Vector3.up * 0.05f, Color.red);
                //here we check if all the rays of the piece are detecting objects
                if (hit.collider != null)
                {
                    //if yes we keep isKinematic true
                    if (hit.collider.gameObject.name != pieceName)
                    {
                        rayHitting[i] = true;
                    }

                }
                else
                {
                    //if no we keep isKinematic false
                    rayHitting[i] = false;
                }
            }

            for (int i = 0; i < rayHitting.Length; i++)
            {
                if (rayHitting[i] == false)
                {
                    //Debug.Log("transform y " + gameObject.transform.position.y);
                    isKinematic = false;
                    return;
                }

                //isKinematic = true;
            }
            
        }


#endif

    }
}//namespace