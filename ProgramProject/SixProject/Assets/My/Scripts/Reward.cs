using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My.Script
{
    public class Reward : MonoBehaviour
    {

        public static Reward _instance;

        private void Awake()
        {
            _instance = this;
        }



        void Start()
        {

            DesSelf();
        }

        void Update()
        {

        }
        public void Open()
        {
            this.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Pieces")
            {
                collision.gameObject.transform.parent.gameObject.SetActive(false);
                Debug.Log("Destroy1");
            }
        }


        void DesSelf()
        {
            Debug.Log("GO");
            GameObject.Destroy(GameObject.Find("RewardButton"));
            GameObject.Destroy(gameObject, 4f);
        }
    }
}
