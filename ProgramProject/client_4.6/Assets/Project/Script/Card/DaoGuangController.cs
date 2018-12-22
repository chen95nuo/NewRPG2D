using UnityEngine;
using System.Collections;

public class DaoGuangController : MonoBehaviour 
{

	public WeaponTrail trail;
	protected float t = 0.033f;

	void Start () 
	{
		if(trail != null){
			
			trail.StartTrail(0.5f, 0.4f);
		}
//		a.SetTime (0.5f, 0.2f, 0.2f);	

	}
	
	void FixedUpdate(){
		if(trail != null){
			
			if (trail.time > 0) {
				trail.Itterate (Time.time - t );
			} else {
				trail.ClearTrail ();
			}
			
			if (trail.time > 0) {
				trail.UpdateTrail (Time.time, t);
			}
		}
	}
}
