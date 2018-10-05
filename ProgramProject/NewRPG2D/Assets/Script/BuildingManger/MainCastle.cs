using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCastle : Castle
{
    public static MainCastle instance;

    private void Awake()
    {
        instance = this;

        Init();
    }

}
