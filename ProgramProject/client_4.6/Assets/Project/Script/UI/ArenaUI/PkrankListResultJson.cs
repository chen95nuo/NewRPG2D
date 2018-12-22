using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PkrankListResultJson : ErrorJson
{
    public List<string> pks; //PK玩家列表，玩家id - 玩家名称 - 头像 - 排名 - 战力 //

    public List<string> cardIds;  //顺序对应pks中的玩家顺序 cardId - id - id - id - id - id;
}
