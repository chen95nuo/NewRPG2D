using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExploreMapControl : MonoBehaviour
{
    public RectTransform map_1;
    public RectTransform map_2;
    private Image map1;
    private Image map2;

    public float moveSpeed = 500f;
    public bool isRun = false;
    public Transform cardPoint;

    public Button btn_Box;

    public UIExplore explore;

    private SkeletonGraphic[] anims;
    private ExpeditionTeam teamData;

    private void Awake()
    {
        map1 = map_1.GetComponent<Image>();
        map2 = map_2.GetComponent<Image>();

        btn_Box.onClick.AddListener(ChickBox);
    }
    private void Update()
    {
        if (isRun)
        {
            map_1.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            map_2.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if (map_1.anchoredPosition.x <= -(map_1.sizeDelta.x))
            {
                map_1.anchoredPosition = new Vector2(map_1.sizeDelta.x, 0);
            }

            if (map_2.anchoredPosition.x <= -(map_2.sizeDelta.x))
            {
                map_2.anchoredPosition = new Vector2(map_2.sizeDelta.x, 0);
            }
            if (teamData.NowTime <= 0)
            {
                teamData.ExploreType = ExploreType.End;
                UpdateCompoment();
            }
        }

    }

    public void UpdateMap(ExpeditionTeam data, Sprite mapSprite)
    {
        teamData = data;
        btn_Box.gameObject.SetActive(false);
        //临时删除处理
        if (anims != null)
        {
            for (int i = 0; i < anims.Length; i++)
            {
                Destroy(anims[i].gameObject);
            }
        }

        map1.sprite = mapSprite;
        map2.sprite = mapSprite;
        anims = new SkeletonGraphic[data.CardsData.Length];

        for (int i = 0; i < data.CardsData.Length; i++)
        {
            GameObject AnimGo = Resources.Load<GameObject>("UIPrefab/CardPrefab/" + data.CardsData[i].AnimationName);
            GameObject Go = Instantiate(AnimGo, cardPoint) as GameObject;
            anims[i] = Go.GetComponent<SkeletonGraphic>();
            anims[i].AnimationState.SetAnimation(0, "run", true);
        }
        isRun = true;

    }
    /// <summary>
    /// 任务完成
    /// </summary>
    public void UpdateCompoment()
    {
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].AnimationState.SetAnimation(0, "stand", true);
        }
        isRun = false;
        btn_Box.gameObject.SetActive(true);
    }

    private void ChickBox()
    {
        Debug.Log("ChickBox");
        //宝箱失活 重置小队信息 获得道具
        teamData.ExploreType = ExploreType.Nothing;
        for (int i = 0; i < teamData.CardsData.Length; i++)
        {
            teamData.CardsData[i].Fighting = false;
        }
        //播放动画
        //
        //计算获得道具
        int dropBoxID = teamData.CurrentMap.DroppingBoxId;
        DropBagData dropData = GameDropBagData.Instance.GetItem(dropBoxID);
        GainData[] datas = GameDropBagData.Instance.GetGains(dropBoxID);
        if (datas.Length <= 0)
        {
            Debug.Log("无奖励");
            return;
        }
        //打开奖励面板
        TinyTeam.UI.TTUIPage.ShowPage<UIRewardTipPage>();
        UIEventManager.instance.SendEvent<GainData[]>(UIEventDefineEnum.UpdateRewardMessageEvent, datas);

        explore.ResetPage();
    }
}
