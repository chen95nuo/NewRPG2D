using UnityEngine;
using System.Collections;

public class Zhen : MonoBehaviour
{
	public static Vector3[] wzvectors = new Vector3[6] { new Vector3(300, 0, 0), new Vector3(-40, 0, 0), new Vector3(-385, 0, 0), new Vector3(300, 0, 350), new Vector3(-40, 0, 350), new Vector3(-385, 0, 350) };
	Tuo[] cardmen = new Tuo[6];
    public Camera cam;
    int cardnum;
    public CombinationInterManager yuanlai;
    // Use this for initialization
    void Start()
    {
       
    }
    public void init(int[] modelIds,int[] cardIds,int[] skillTypes,bool canShangzhen,int[] cardTips, int[] cardBns,bool isArena)
    {
      
        for (int i = 0; i < 6; i++)
        {
            if (cardmen[i] != null)
            {
                cardmen[i].destroy();
              //  Debug.Log("destroy"+i);
                Destroy(cardmen[i].gameObject);
                cardmen[i] = null;
            }
        }
        for (int i = 0; i < 6; i++)
        {
            GameObject gobj = Instantiate(GameObjectUtil.LoadResourcesPrefabs("EmbattlePanel/cardtuo",3)) as GameObject;
            gobj.transform.parent = transform;
            gobj.transform.localEulerAngles = new Vector3(0, -180, 0);
            gobj.transform.localScale = new Vector3(100, 100, 100);
            gobj.transform.localPosition = wzvectors[i];
            cardmen[i] = gobj.GetComponent<Tuo>();
            cardmen[i].zhen = this;
            cardmen[i].init(modelIds[i], i,cardIds[i],skillTypes[i],canShangzhen,cardTips[i], cardBns[i],isArena);
        }
    }
    
	public void startSwap(Tuo src)
	{
		foreach(Tuo tuo in cardmen)
		{
			if(tuo!=src)
			{
				tuo.createEffect("kapai_huanzhen");
				tuo.hideEffect("kapai_shangzhen");
			}
		}
	}
	
	public void drop(Tuo card)
    {
		foreach(Tuo tuo in cardmen)
		{
			tuo.destroyEffect("kapai_huanzhen");
			tuo.showEffect("kapai_shangzhen");
		}
		
        float dis = float.MaxValue;
        int newwz = -1;
        for (int i = 0; i < wzvectors.Length; i++)
        {
            float dis1 = Vector3.Distance(card.gameObject.transform.localPosition, wzvectors[i]);
            if (dis1 < dis)
            {
                dis = dis1;
                newwz = i;
            }
        }

        int yuanwz = card.wzindex;
        Tuo zhuren = getcardbywz(newwz);
        if (zhuren != null)
        {
            zhuren.setPosition(yuanwz);
        }
        card.setPosition(newwz);

        if (yuanwz != newwz)
        {
            yuanlai.ChangePositionData(yuanwz, newwz);
        }


    }
    public void renclick(int wzindex,int cid)
    {
        yuanlai.renclick(wzindex,cid);
    }
    Tuo getcardbywz(int wz)
    {
        for (int i = 0; i < cardmen.Length; i++)
        {
            if (cardmen[i] != null && cardmen[i].wzindex == wz)
            {
                return cardmen[i];
            }
        }
        return null;
    }
    public bool testflag = false;
    // Update is called once per frame
    void Update()
    {
        if (testflag)
        {
            testflag = false;

        }
    }
	
	public void gc()
	{
		yuanlai=null;
		for (int i = 0; i < 6; i++)
        {
            if (cardmen[i] != null)
            {
                cardmen[i].destroy();
                Destroy(cardmen[i].gameObject);
				cardmen[i].zhen = null;
                cardmen[i] = null; 
            }
        }
	}
}
