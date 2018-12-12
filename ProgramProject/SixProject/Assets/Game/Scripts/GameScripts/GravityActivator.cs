using UnityEngine;
using System.Collections;
/// <summary>
/// Script which activates the gravity of pieces
/// </summary>
namespace MadFireOn
{
    public class GravityActivator : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        //when the piece comes in contact with activate its gravity is set active
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other != null)
            {
                //the activator collider only collides with piece so we dont have to specify any tags ,names, etc
#if UNITY_5_4_OR_NEWER
                other.GetComponent<PieceScript>().isRayOn = true;
#else
                other.GetComponent<PieceScript>().isKinematic = false;
#endif
            }
        }
    }
}//namespace