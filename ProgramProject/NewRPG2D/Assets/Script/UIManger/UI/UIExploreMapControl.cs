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

    private SkeletonGraphic[] anims;

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
        }

    }

    public void UpdateMap(ExpeditionTeam data, Sprite mapSprite)
    {

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

    public void UpdateCompoment()
    {

        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].AnimationState.SetAnimation(0, "stand", true);
        }
        btn_Box.gameObject.SetActive(true);

    }

    private void ChickBox()
    {

    }
}
