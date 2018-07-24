using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHatcheryPool : MonoBehaviour
{

    public UIHatcheryGrid[] grids;

    private void Awake()
    {

        Init();

    }

    private void Init()
    {
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].UpdateGrid();
            grids[i].btn_hatchery.onClick.AddListener(ChickGrid);
        }
    }


    public void ChickGrid()
    {

    }

}
