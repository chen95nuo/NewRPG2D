using UnityEngine;
using Assets.Script.Battle;
using DragonBones;
using Transform = UnityEngine.Transform;

public class RoleRender : MonoBehaviour
{
    public UnityArmatureComponent roleAnimation;
    public RoleBase CurrentRole { get; private set; }
    private Transform _trans;
    private Transform _renderTrans;
    private bool bCanContorl;

    // Use this for initialization
    void Awake()
    {
        _trans = transform;
    }

    public void SetRoleBaseInfo(RoleBase roleBase)
    {
        CurrentRole = roleBase;
        SetIsControlState(roleBase.IsCanControl);
    }



    public void SetIsControlState(bool canControl)
    {
        bCanContorl = canControl;
    }

    public void Move(Vector3 movePostion)
    {
        CurrentRole.RoleMoveMoment.SetTargetPosition(movePostion);
    }

    public void ChangeTarget(RoleBase targetRole)
    {
        if (targetRole.TeamId == CurrentRole.TeamId)
        {
            return;
        }

        CurrentRole.RoleSearchTarget.SetTarget(targetRole);
        DebugHelper.Log(" attack  " + targetRole.InstanceId);

    }

}


