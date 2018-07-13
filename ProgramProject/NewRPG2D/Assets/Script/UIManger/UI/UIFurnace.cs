using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIFurnace : MonoBehaviour
{

    public Button menu;
    public Text menuNumber;
    public GameObject menuNull;
    public GameObject something;
    public Text menuTime;
    public Slider menuSlider;

    public Button addFurnace;
    public Button[] popUp;
    public GameObject typeTime;
    public Text time;
    public Slider timeSlider;

    public Text goldCoin;
    public Text needTime;
    public Button start;

    public Button rawMaterial;
    public RawMaterials[] rawMaterials;

    private void Awake()
    {
        AwakeInitialization();//初始化
    }

    private void Start()
    {


    }

    public void AwakeInitialization()
    {
        rawMaterial.gameObject.SetActive(false);
        for (int i = 0; i < rawMaterials.Length; i++)
        {
            GameObject go = Instantiate(rawMaterial.gameObject, rawMaterial.transform.parent.transform) as GameObject;
            go.SetActive(true);
            rawMaterials[i].btn_rawMaterials = go.GetComponent<Button>();
            rawMaterials[i].propImage = go.transform.Find("Prop/Image").GetComponent<Image>();
            rawMaterials[i].propQuality = go.transform.Find("Prop").GetComponent<Image>();
            rawMaterials[i].noPorp = go.transform.Find("NoProp").GetComponent<Image>();
            rawMaterials[i].propQuality.gameObject.SetActive(false);
            rawMaterials[i].noPorp.gameObject.SetActive(true);
        }

        TimeSerialization(0, needTime);

    }

    void TimeSerialization(int time, Text text)
    {
        int hour = time / 3600;
        int minute = (time - hour * 3600) / 60;
        int milliScecond = (time - hour * 3600 - minute * 60);

        text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, milliScecond);
    }

}
[System.Serializable]
public class RawMaterials
{
    [SerializeField]
    public Button btn_rawMaterials;
    [SerializeField]
    public Image propImage;
    [SerializeField]
    public Image propQuality;
    [SerializeField]
    public Image noPorp;
}
