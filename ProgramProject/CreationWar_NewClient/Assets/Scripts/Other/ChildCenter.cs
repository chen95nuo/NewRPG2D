using UnityEngine;
using System.Collections;

public class ChildCenter : MonoBehaviour {
    private UICenterOnChild centerOnChild;

	void Start () {
        if (null == centerOnChild)
        {
            centerOnChild = this.GetComponent<UICenterOnChild>();
        }
	}

    void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0) && this.transform.childCount > 0)
        {
            centerOnChild.Recenter();
        }
    }
}
