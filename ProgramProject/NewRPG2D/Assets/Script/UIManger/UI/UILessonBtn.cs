using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILessonBtn : MonoBehaviour
{

    public Image[] starts;

    public Text level;
    public Sprite[] sprites;

    private Button lessonBtn;

    private void Awake()
    {
        lessonBtn = GetComponent<Button>();
        lessonBtn.onClick.AddListener(ChickBtn);
    }
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

    private void ChickBtn()
    {

    }
}
