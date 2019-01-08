using UnityEngine;
using System.Collections;

public class LightningEmitter : MonoBehaviour {
	public int branchesPerReceiver = 1;
	public int sectionsPerBranch = 10;
	public float lightningBoltJitter = 5f;
	public float lightningWidth = 1f;
	public float lightningFrequency = 60f;
	
	public Material lightningMaterial;
	public GameObject[] lightningReceivers;
	
	private LineRenderer[] lineRenderers = null;
	private float timeCount = 0f;
	
	// Use this for initialization
	void Start () {
		//If no receivers are assigned, log an error and delete the script
		if(lightningReceivers.Length < 1) {
			Debug.LogError("No receivers assigned to the Lightning Emitter.");
			Destroy(this);
		}
		
		//Initialise the line renderers
		InitialiseLineRenderers();
	}
	
	// Update is called once per frame
	void Update () {
		//Check if it's time to change the lightning bolt
		timeCount += Time.deltaTime;
		if(timeCount < (1 / lightningFrequency))
			return;
		else
			timeCount = 0f;
		
		//Keep a count of which renderer we're currently working on
		int rendererIndex = 0;
		
		for(int i = 0; i < lightningReceivers.Length; i++) {
			//Determine the length of a section of the bolt
			Vector3 sectionVector = (lightningReceivers[i].transform.position - transform.position) / sectionsPerBranch;
			
			//Initialise an array of vectors for the bolt
			Vector3[] lineVectors = new Vector3[sectionsPerBranch];
			
			//Calculate the vectors for the middle sections
			for(int j = 1; j < lineVectors.Length -1 ; j++)
				lineVectors[j] = transform.position + (sectionVector * j);
			
			//Set the values in the line renderer for ecah bolt
			for(int j = 0; j < branchesPerReceiver; j++) {
				
				if(lineRenderers[rendererIndex]) {				
					//Set the beginning and end of each branch to be on the game objects
					lineRenderers[rendererIndex].SetPosition(0, transform.position);
					lineRenderers[rendererIndex].SetPosition(lineVectors.Length-1, lightningReceivers[i].transform.position);
					lineRenderers[rendererIndex].SetWidth(lightningWidth, lightningWidth);
				
					//Set vectors for the rest of the sections adding jitter in the process
					for(int k = 1; k < (sectionsPerBranch - 1); k++)
						lineRenderers[rendererIndex].SetPosition(k, AddVectorJitter(lineVectors[k], lightningBoltJitter));
				}
				
				rendererIndex++;
			}
		}
	}
	
	//Add a bit of random jitter to a vector
	public Vector3 AddVectorJitter(Vector3 vector, float jitter) {
		vector += Vector3.left * Random.Range(-jitter, jitter);
		vector += Vector3.up * Random.Range(-jitter, jitter);
		vector += Vector3.forward * Random.Range(-jitter, jitter);
		
		return vector;
	}
	
	public void InitialiseLineRenderers() {
		//Clean out any existing line renderers
		lineRenderers = gameObject.GetComponentsInChildren<LineRenderer>();		
		if(lineRenderers != null) {
			for(int i = 0; i < lineRenderers.Length; i++)
				DestroyImmediate(lineRenderers[i].gameObject);
			
			lineRenderers = null;
		}
			
		//Create a game object for each line renderer and parent it to this game object
		for(int i = 0; i < (branchesPerReceiver * lightningReceivers.Length); i++) {
			GameObject temp = new GameObject();
			temp.transform.parent = gameObject.transform;
			temp.name = "Line Renderer " + (i + 1);
			temp.AddComponent<LineRenderer>();
		}
		
		//Get references to all the line renderers we've just created
		lineRenderers = gameObject.GetComponentsInChildren<LineRenderer>();
		
		//Set initial settings for each renderer
		for(int i = 0; i < lineRenderers.Length; i++) {
			lineRenderers[i].castShadows = false;
			lineRenderers[i].receiveShadows = false;
			lineRenderers[i].material = lightningMaterial;
			
			lineRenderers[i].SetVertexCount(sectionsPerBranch);
			lineRenderers[i].SetWidth(lightningWidth, lightningWidth);
		}
	}
	
	private void InitialiseLights() {
		
	}
	
	private void RandomiseLights() {
		
	}
}
