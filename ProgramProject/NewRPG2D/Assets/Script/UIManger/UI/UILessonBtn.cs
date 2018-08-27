using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILessonBtn : MonoBehaviour
{

    public Image[] starts;

    public Text level;
    public Sprite[] sprites;

    public Button btn_lesson;

    public void UpdateBtn(int level)
    {
        for (int i = 0; i < starts.Length; i++)
        {
            if (i < level)
            {
                starts[i].sprite = sprites[1];
            }
            else
            {
                starts[i].sprite = sprites[0];
            }
        }
    }
}
