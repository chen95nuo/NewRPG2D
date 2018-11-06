using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Req
public class ReqLogin {
    public string uid;
    public string pwd;
}

public class ReqRegist {
    public string uid;
    public string pwd;
    public string uname;
    public int sex;
}

public class ReqSendPos {
    public int x;
    public int y;
}
#endregion

#region Rev
public class Result {
    public int errid;

}

public class RUserData  {
    public int errid;
    public string uname;
    public int sex;
    public int money;
}

public class Dog {
    public int id;
    public string tmp1;
    public string name;
    public int sex;
    public int age;
    public int state;
}

public class RDogData : Result {
    public Dog[] dogs;
}

public class RCoupleData : Result {

}
#endregion