using UnityEngine;
using System.Collections;

public class SaleInfo : MonoBehaviour {
	
	public GameObject sale;
	public UILabel saleNum;
	
	// Use this for initialization
	void Start () {
		sale.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setData(int saleValue)
	{
		sale.SetActive(true);
		saleNum.text=saleValue+"";
	}
	
	public void hide()
	{
		sale.SetActive(false);
	}
	
}
