using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NumberPic : MonoBehaviour {

    public Pos anchor;
    public GameObject beforePic;
    public GameObject afterPic;
    public GameObject numPic;

    public enum Pos { Left, Center, Right };

    UISprite numSpr;    //UISprite of numPic

    int num;
    List<GameObject> copyNum = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setNum(int num)
    {
        if (num < 0)
            num = 0;
        this.num = num;

        updateNum();
    }

    void updateNum()
    {
        float w1 = 0, w2 = 0, w = 0;
        if (beforePic != null) w1 = beforePic.GetComponent<UISprite>().width;
        if (afterPic != null) w2 = afterPic.GetComponent<UISprite>().width;

        int[] nn = new int[20];
        int count = 0;
        do
        {
            nn[count++] = num % 10;
            num /= 10;
        } while (num > 0);

        if (numPic != null)
        {
            numSpr = numPic.GetComponent<UISprite>();
            string namePre = numSpr.spriteName.Remove(numSpr.spriteName.Length - 1);
            numSpr.spriteName = namePre + nn[--count];
            numSpr.MakePixelPerfect();
            w += numSpr.width;

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    GameObject obj = Instantiate(numPic) as GameObject;
                    obj.transform.parent = gameObject.transform;
                    obj.transform.localScale = Vector3.one;

                    obj.GetComponent<UISprite>().spriteName = namePre + nn[count-i-1];
                    obj.GetComponent<UISprite>().MakePixelPerfect();
                    copyNum.Add(obj);

                    w += obj.GetComponent<UISprite>().width;
                }
            }
        }

        //first pic position
        float pos = 0;
        if (anchor == Pos.Center)
        {
            pos = -(w + w1 + w2) / 2;
        }
        else if (anchor == Pos.Right)
        {
            pos = -(w + w1 + w2);
        }
        float pos2 = pos;

        //set pic position
        if (beforePic != null)
        {
            pos += w1 / 2;
            beforePic.transform.localPosition = new Vector3(pos, beforePic.transform.localPosition.y, 1);
        }
        if (numPic != null)
        {
            pos += (w1 / 2 + numSpr.width / 2);
            numPic.transform.localPosition = new Vector3(pos, numPic.transform.localPosition.y, 1);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (i == 0)
                    {
                        pos += (numSpr.width / 2 + copyNum[i].GetComponent<UISprite>().width / 2);
                    }
                    else
                    {
                        pos += (copyNum[i - 1].GetComponent<UISprite>().width / 2 + copyNum[i].GetComponent<UISprite>().width / 2);
                    }
                    copyNum[i].transform.localPosition = new Vector3(pos, numPic.transform.localPosition.y, 1);
                }
            }
        }
        if (afterPic != null)
        {
            pos = pos2 + (w + w1 + w2) - w2 / 2;
            afterPic.transform.localPosition = new Vector3(pos, afterPic.transform.localPosition.y, 1);
        }

    }

    public void clearNum()
    {
        if (copyNum != null)
        {
            while (copyNum.Count > 0)
            {
                Destroy(copyNum[0]);
                copyNum.RemoveAt(0);
            }
        }
    }

    public void resetNum(int num)
    {
        clearNum();

        if (num < 0)
            num = 0;
        this.num = num;

        updateNum();
    }
}
