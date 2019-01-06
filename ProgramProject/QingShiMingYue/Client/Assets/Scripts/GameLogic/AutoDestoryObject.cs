using UnityEngine;

public class AutoDestoryObject : MonoBehaviour
{
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void Destroy()
	{
		GameObject.Destroy(this.gameObject);
	}
}