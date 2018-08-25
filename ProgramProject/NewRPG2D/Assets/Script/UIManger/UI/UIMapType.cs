using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapType : MonoBehaviour
{

    public Button btn_map;

    private Button[] btn_maps;

    private List<MapData> maps;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        UpdateMaps();
    }

    /// <summary>
    /// 有多少地图刷新多少格子
    /// </summary>
    private void UpdateMaps()
    {
        maps = GameMapData.Instance.items;
        Debug.Log(maps.Count);
        btn_maps = new Button[maps.Count];
        btn_map.gameObject.SetActive(false);
        for (int i = 0; i < btn_maps.Length; i++)
        {
            GameObject go = Instantiate(btn_map.gameObject, btn_map.transform.parent.transform) as GameObject;
            go.SetActive(true);
            btn_maps[i] = go.GetComponent<Button>();
            btn_maps[i].interactable = false;
            //图片暂无 btn_rounds[i].Image
            btn_maps[i].onClick.AddListener(ChickMapBtn);
            btn_maps[i].GetComponentInChildren<Text>().text = maps[i].Name;
        }
        ChickMap();
    }
    /// <summary>
    /// 检查地图信息
    /// </summary>
    private void ChickMap()
    {
        PlayerRoundData data = GetPlayerRoundData.Instance.items;
        for (int i = 0; i < maps.Count; i++)
        {
            Debug.Log(maps[i].Id);
            Debug.Log(data.UnlockMapID.Count);
            for (int j = 0; j < data.UnlockMapID.Count; j++)
            {
                Debug.Log(data.UnlockMapID[j]);
                if (maps[i].Id == data.UnlockMapID[j])
                {
                    btn_maps[i].interactable = true;
                }
            }
        }
    }

    private void ChickMapBtn()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        int index = 0;
        for (int i = 0; i < btn_maps.Length; i++)
        {
            if (btn_maps[i].gameObject == go)
            {
                index = i;
            }
        }
        GetPlayData.Instance.player[0].MapNumber = index;
        TinyTeam.UI.TTUIPage.ClosePage<UIMapTypePage>();
    }
}
