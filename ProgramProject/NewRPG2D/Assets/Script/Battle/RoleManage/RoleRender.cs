using UnityEngine;
using Assets.Script.Battle;

[ExecuteInEditMode]
public class RoleRender : MonoBehaviour
{
    //public UnityArmatureComponent roleAnimation;
    private Transform _trans;
    private Transform _renderTrans;
    private RoleBase currentRole;

    // Use this for initialization
    void Awake()
    {
        _trans = transform;
    }

    public void SetRoleBaseInfo (RoleBase roleBase) {
        currentRole = roleBase;
    }
}


