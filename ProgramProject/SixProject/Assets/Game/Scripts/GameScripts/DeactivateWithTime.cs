using UnityEngine;
using System.Collections;
/// <summary>
/// Used to deactivate the object after some time when its spawned , used for exlosion effect
/// </summary>
namespace MadFireOn
{
    public class DeactivateWithTime : MonoBehaviour
    {

        public float timeToDeactive = 2f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(StartCountDown());
            }
        }

        IEnumerator StartCountDown()
        {
            yield return new WaitForSeconds(timeToDeactive);
            gameObject.SetActive(false);
        }
    }
}//namespace