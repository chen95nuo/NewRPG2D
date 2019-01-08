using UnityEngine;
using System.Collections;

public class SpriteForDungeon : BtnDisable {

    public UILabel lblName;
    public UISprite[] listStars;

    private int numStars;

    public int NumStars
    {
        get { return numStars; }
        set 
        {
            numStars = value;
            SetStar(numStars);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetStar(this.numStars);
    } 

    private void SetStar(int mNumStars)
    {
        int num = mNumStars >> 1;
        int half = mNumStars % 2;
        int tempNum = 0;
        foreach (UISprite item in listStars)
        {
            if (tempNum > (num - 1))
            {
                item.spriteName = "start0";
            }
            else
            {
                item.spriteName = "Start - new";
            }
            tempNum++;
        }
        if (half != 0&&num<tempNum)
        {
            listStars[num].spriteName = "start0";
        }
    }



}
