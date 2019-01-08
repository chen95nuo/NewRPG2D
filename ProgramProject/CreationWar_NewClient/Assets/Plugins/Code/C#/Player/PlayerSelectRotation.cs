using UnityEngine;
using System.Collections;

public class PlayerSelectRotation : MonoBehaviour {

    public Transform rotationTarge;

    void OnDrag(Vector2 mVector)
    {
        rotationTarge.Rotate(rotationTarge.up * -mVector.x);
    }
}
