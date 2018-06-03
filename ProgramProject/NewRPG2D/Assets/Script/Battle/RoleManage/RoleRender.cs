using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Battle;

[ExecuteInEditMode]
public class RoleRender : MonoBehaviour
{
    public bool _isStatic;
    public int _x, _y, _z;

    private Transform _trans;
    private Transform _renderTrans;
    private RoleBase currentRole;

    // Use this for initialization
    void Awake()
    {
        _trans = transform;
        _renderTrans = _trans.Find("Render");
    }

    public void SetRoleBaseInfo (RoleBase roleBase) {
        currentRole = roleBase;
    }
}
