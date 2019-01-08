using UnityEngine;
using System.Collections;


public class YuanOutput : MonoBehaviour {

    public UILabel myLable;
    public YuanInput myYuanInput;
    public UILabel aaa;

    [HideInInspector]
    private string strPrint;

    [HideInInspector]
    private string myText;
    /// <summary>
    /// Text
    /// </summary>
    public string MyText
    {
        get { return myText; }
        set 
        {
            myText = value;
            strPrint = WarpString(myText);
        }
    }

	// Use this for initialization
    IEnumerator Start()
    {

        myLable.text = "yuany1111111";
        yield return new WaitForEndOfFrame();

        aaa.transform.localPosition = new Vector3(GetCenterPivot(myLable.text.Length, myLable).x, GetCenterPivot(myLable.text.Length, myLable).y, aaa.transform.localPosition.z);
	}


    /// <summary>
    /// 解析可以显示的文字
    /// </summary>
    /// <param name="mText"></param>
    /// <returns></returns>
    public string WarpString(string mText)
    {
        for(int i=0;i<mText.Length;i++)
        {

        }


        return mText;
    }

    /// <summary>
    /// 获取NGUI文字位置(隔桢调用)
    /// </summary>
    /// <param name="mNum">字符索引</param>
    /// <param name="mLable">UILable</param>
    /// <returns>位置</returns>
    public static Vector2 GetCenterPivot(int mNum, UILabel mLable)
    {
        Vector2 temp = Vector2.zero;
//        float a = mLable.mVerts[(4 * mNum) - 1].x;
//        float b = mLable.mVerts[(4 * mNum) - 3].x;
//        float mBuffeX = ((b - a) / 2f) + a;
//        temp.x = mBuffeX * mLable.transform.localScale.x;

//        float c = mLable.mVerts[(4 * mNum) - 1].y;
//        float d = mLable.mVerts[(4 * mNum) - 2].y;
//        float mBuffeY = ((d - c) / 2f) + c;
//        temp.y = mBuffeY * mLable.transform.localScale.y;

        return temp;
    }


}
