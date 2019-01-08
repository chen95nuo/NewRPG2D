using UnityEngine;
using System.Collections;

public class PicSelectClick : MonoBehaviour {

    [HideInInspector]
    public SetPic sender;
    [HideInInspector]
    public SelectType selectType;
    [HideInInspector]
    public string num;
    public enum SelectType : int
    {
        Background,
        Mark,
        Mask,
    }


    void OnClick()
    {
        if (sender != null)
        {
            sender.OnCbkClick(this.selectType, this.num);
        }
    }
}
