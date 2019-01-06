using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerUnit : MonoBehaviour
{
	public Transform towerMesh;

	public Transform meshExchangeForwardTrigger;

	public Transform meshExchangeBackTrigger;

	public List<Transform> pathNodes;

	public List<Transform> doorNodes;

}