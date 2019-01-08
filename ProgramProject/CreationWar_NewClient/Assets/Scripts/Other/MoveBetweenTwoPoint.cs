using UnityEngine;
using System.Collections;

public class MoveBetweenTwoPoint : MonoBehaviour {
    public Transform start;
    public Transform end;
	
	void OnEnable()
	{
		transform.position = end.position;
	}
	
    void MoveTo() 
    {
        this.transform.position = end.position;
    }

    void BackTo()
    {
        this.transform.position = start.position;
    }
}
