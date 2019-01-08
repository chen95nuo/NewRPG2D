using UnityEngine;
using System.Collections;

public class EventCollider : MonoBehaviour {

    public delegate void DelCollider(Collider mCollider);
    public event DelCollider eventCollider;

    void OnTriggerEnter(Collider mCollider)
    {
        if (eventCollider != null)
        {
            eventCollider(mCollider);
        }
    }
}
