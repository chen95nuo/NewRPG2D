//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

public class Tuo : MonoBehaviour
{

    Transform mTrans;
    public Vector3 tempvector;
    bool mPressed = false;
    int mTouchID = 0;
    bool mIsDragging = false;
    bool mSticky = false;
    //Transform mParent;

    public GameObject rencard = null;
    public int cardid;

    public int wzindex;
    public int szindex;
    public float bilichi;

    public Zhen zhen;
	
	public UILabel nameLabel;
	public UISprite race;
	public UISprite skillType;
	public UISprite star;
	public UISprite tip;
	public GameObject pos;
	public GameObject shadow;

    bool isArena;
	
	private string[] skillTypeNames={"atk","def","hp"};
	
    public void init(int modelId, int wzi,int cardId,int skillType0,bool canShangzhen,int cardTip, int cardBn,bool isArena)
    {
        szindex = wzi;
        wzindex = wzi;
        cardid = modelId;
        this.isArena = isArena;
		if(tip != null)
		{
			tip.gameObject.SetActive(false);
		}
        if (rencard != null)
        {
            Destroy(rencard);
            rencard = null;
        }
        if (modelId >= 0)
        {
            rencard = Instantiate(Resources.Load("Prefabs/Cards/card" + modelId.ToString("000"))) as GameObject;
            rencard.transform.parent = gameObject.transform;
            rencard.transform.localPosition = Vector3.zero;
            rencard.transform.localRotation = Quaternion.identity;
            rencard.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
			if(rencard.GetComponent<CardEffectControl>()!=null)
			{
				rencard.GetComponent<CardEffectControl>().hideEffect();
			}
			GameObject shadowPrefab = Resources.Load("Prefabs/Item/shadow") as GameObject;
			GameObject shadowObj = GameObject.Instantiate(shadowPrefab) as GameObject;
			GameObjectUtil.gameObjectAttachToParent(shadowObj,rencard.transform.FindChild("foot").gameObject);
			shadowObj.transform.localScale = new Vector3(1.3f,1,1.3f);
        }
        //   bilichi = zhen.cam.fieldOfView / 22.5f;
        //  bilichi =1.0f+ (zhen.cam.fieldOfView - 22.5f)/5.0f;
        // float temp = zhen.cam.fieldOfView / 22.5f;
        // tempvector = new Vector3(mTrans.localPosition.x / temp, mTrans.localPosition.z / 1.5f / temp, 0);
        // tempvector =new Vector3(mTrans.localPosition.x,mTrans.localPosition.z/1.5f,0) ;
		if(cardId>0 && skillType0>0)
		{
			CardData cd=CardData.getData(cardId);
			if(cardBn > 0)
			{
				nameLabel.text=cd.name + "+" + cardBn;
			}
			else 
			{
				nameLabel.text=cd.name;
			}
			race.spriteName="race_"+cd.race;
			star.spriteName="card_side_s"+cd.star;
			skillType.spriteName=skillTypeNames[skillType0-1];
			if(wzindex < 3)
			{
				
				pos.transform.localPosition=new Vector3(-15,300,30);
			}
			else 
			{
				pos.transform.localPosition=new Vector3(-15,350,30);
			}
			shadow.SetActive(true);
			if(wzi<=2)
			{
				shadow.GetComponent<UISprite>().spriteName="shadow01";
			}
			else
			{
				shadow.GetComponent<UISprite>().spriteName="shadow02";
			}
			if(tip != null && cardTip > 0)
			{
				tip.gameObject.SetActive(true);
				tip.spriteName = "tip_mark_" + cardTip.ToString();
			}
			
		}
		else
		{
			pos.SetActive(false);
			shadow.SetActive(false);
			if(canShangzhen)
			{
                if (!isArena)
                {
                    createEffect("kapai_shangzhen");
                }
			}
		}
    }
	
	public void createEffect(string effectName)
	{
		GameObject effect=Instantiate(GameObjectUtil.LoadResourcesPrefabs(effectName,1)) as GameObject;
		effect.name=effectName;
		effect.transform.parent=mTrans;
		effect.transform.localPosition=Vector3.zero;
		effect.transform.localScale=new Vector3(0.6f,0.6f,0.6f);
		effect.transform.localRotation=Quaternion.Euler(new Vector3(0,0,0));
		Resources.UnloadUnusedAssets();
	}
	
	public void hideEffect(string effectName)
	{
		Transform effectTf=mTrans.FindChild(effectName);
		if(effectTf!=null)
		{
			effectTf.gameObject.SetActive(false);
		}
	}
	
	public void showEffect(string effectName)
	{
		Transform effectTf=mTrans.FindChild(effectName);
		if(effectTf!=null)
		{
			effectTf.gameObject.SetActive(true);
		}
	}
	
	public void destroyEffect(string effectName)
	{
		Transform effectTf=mTrans.FindChild(effectName);
		if(effectTf!=null)
		{
			Destroy(effectTf.gameObject);
		}
	}
	
    public void destroy()
    {
        if (cardid >= 0)
        {
            Destroy(rencard.gameObject);
        }
    }
    public void setPosition(int wzi)
    {
        wzindex = wzi;
        mTrans.localPosition = Zhen.wzvectors[wzi];
        tempvector = new Vector3(mTrans.localPosition.x / bilichi, mTrans.localPosition.z / 1.4f / bilichi, 0);
		
		if(wzindex < 3)
		{
			
			pos.transform.localPosition=new Vector3(-15,300,30);
		}
		else 
		{
			pos.transform.localPosition=new Vector3(-15,350,30);
		}
    }

    /// <summary>
    /// Update the table, if there is one.
    /// </summary>

    void UpdateTable()
    {
        /*
       UITable table = NGUITools.FindInParents<UITable>(gameObject);
        if (table != null) table.repositionNow = true;
          */
    }

    /// <summary>
    /// Drop the dragged object.
    /// </summary>

    void Drop()
    {
        zhen.drop(this);
        /*
       // Is there a droppable container?
       Collider col = UICamera.lastHit.collider;
       DragDropContainer container = (col != null) ? col.gameObject.GetComponent<DragDropContainer>() : null;

       if (container != null)
       {
           // Container found -- parent this object to the container
           mTrans.parent = container.transform;

           Vector3 pos = mTrans.localPosition;
           pos.z = 0f;
           mTrans.localPosition = pos;
       }
       else
       {
           // No valid container under the mouse -- revert the item's parent
           mTrans.parent = mParent;
       }

       // Restore the depth
       UIWidget[] widgets = GetComponentsInChildren<UIWidget>();
       for (int i = 0; i < widgets.Length; ++i) widgets[i].depth = widgets[i].depth - 100;

       // Notify the table of this change
       UpdateTable();

       // Make all widgets update their parents
       NGUITools.MarkParentAsChanged(gameObject);
         */
    }

    /// <summary>
    /// Cache the transform.
    /// </summary>

    void Awake()
    {
        mTrans = transform;

    }

    UIRoot mRoot;

    /// <summary>
    /// Start the drag event and perform the dragging.
    /// </summary>

    void OnDrag(Vector2 delta)
    {
        if (!isArena)
        {
            if (mPressed && UICamera.currentTouchID == mTouchID && enabled)
            {
                if (!mIsDragging)
                {
                    zhen.startSwap(this);

                    mIsDragging = true;
                    //mParent = mTrans.parent;
                    mRoot = NGUITools.FindInParents<UIRoot>(mTrans.gameObject);
                    //    tempvector = mTrans.localPosition;
                    //if (DragDropRoot.root != null)
                    //    mTrans.parent = DragDropRoot.root;

                    //Vector3 pos = mTrans.localPosition;
                    //pos.z = 0f;
                    //mTrans.localPosition = pos;

                    //// Inflate the depth so that the dragged item appears in front of everything else
                    //UIWidget[] widgets = GetComponentsInChildren<UIWidget>();
                    //for (int i = 0; i < widgets.Length; ++i) widgets[i].depth = widgets[i].depth + 100;

                    //NGUITools.MarkParentAsChanged(gameObject);

                    //  Debug.Log("dddddddddddddddddd");

                    bilichi = zhen.cam.fieldOfView / 22.5f;
                    tempvector = new Vector3(mTrans.localPosition.x / bilichi, mTrans.localPosition.z / 1.4f / bilichi, 0);
                }
                else
                {
                    //  Debug.Log("ttttttttttttttttttttt");
                    // mTrans.localPosition += (Vector3)delta * mRoot.pixelSizeAdjustment;

                    tempvector += (Vector3)delta * mRoot.pixelSizeAdjustment;
                    //   Debug.Log(bilichi);
                    //  tempvector += (Vector3)delta;
                    // float temp = 1 + (zhen.cam.fieldOfView - 22.5f) / 25;   
                    mTrans.localPosition = new Vector3(tempvector.x * bilichi, 0, tempvector.y * 1.4f * bilichi);
                }
            }
        }
    }

    /// <summary>
    /// Start or stop the drag operation.
    /// </summary>

    void OnPress(bool isPressed)
    {
        if (cardid < 0)
        {
            return;
        }

        if (enabled)
        {
            if (isPressed)
            {
                if (mPressed) return;

                mPressed = true;
                mTouchID = UICamera.currentTouchID;

                if (!UICamera.current.stickyPress)
                {
                    mSticky = true;
                    UICamera.current.stickyPress = true;
                }
            }
            else
            {
                mPressed = false;

                if (mSticky)
                {
                    mSticky = false;
                    UICamera.current.stickyPress = false;
                }
            }

            mIsDragging = false;
            Collider col = collider;
            if (col != null) col.enabled = !isPressed;
            if (!isPressed) Drop();
        }
    }


    void OnClick()
    {

        Debug.Log("click");
        if (isArena)
        {
            if (this.transform.childCount > 1)
            {
                zhen.renclick(wzindex, cardid);
            }
        }
        else
            zhen.renclick(wzindex, cardid);
        
    }
}
