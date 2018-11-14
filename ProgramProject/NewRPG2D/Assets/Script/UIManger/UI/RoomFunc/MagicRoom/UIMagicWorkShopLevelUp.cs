using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicWorkShopLevelUp : UILevelUp
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_UseNum;
    public Text txt_NextNum;

    public Image slider_1;
    public Image slider_2;
    public GameObject magicGrid;
    public Transform magicPoint;
    private List<UIMagicGrid> grids = new List<UIMagicGrid>();

    private void Awake()
    {
        txt_Tip_1.text = "升级增加";
        txt_Tip_2.text = "数量";
    }
    protected override void Init(RoomMgr data)
    {
        base.Init(data);
        slider_1.fillAmount = data.currentBuildData.buildingData.Param2 / 6;
        txt_UseNum.text = data.currentBuildData.buildingData.Param2.ToString();
        BuildingData buildData = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.currentBuildData.buildingData.NextLevelID);
        slider_2.fillAmount = buildData.Param2 / 6;
        txt_NextNum.text = buildData.Param2.ToString();

        List<MagicData> magicDatas = MagicDataMgr.instance.GetMagic(buildData.Level);
        for (int i = 0; i < magicDatas.Count; i++)
        {
            if (grids.Count <= i)
            {
                GameObject go = Instantiate(magicGrid, magicPoint) as GameObject;
                UIMagicGrid grid = go.GetComponent<UIMagicGrid>();
                grids.Add(grid);
            }
            grids[i].gameObject.SetActive(true);
            grids[i].UpdateInfo(magicDatas[i], MagicGridType.CanLevelUp);
        }
        for (int i = magicDatas.Count; i < grids.Count; i++)
        {
            grids[i].gameObject.SetActive(false);
        }
    }
}
