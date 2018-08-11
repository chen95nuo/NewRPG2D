using UnityEngine;
using Assets.Script.Battle;
using Spine.Unity;
using UnityEngine.Rendering;
using Transform = UnityEngine.Transform;

public class RoleRender : MonoBehaviour
{
    public SkeletonAnimation roleAnimation;
    [SerializeField] private SortingGroup currentSortingGroup;
    public RoleBase CurrentRole { get; private set; }
    private Transform _trans;
    private Transform _renderTrans;
    private bool bCanContorl;

    // Use this for initialization
    private void Awake()
    {
        _trans = transform;
    }

    private void Update()
    {
        SetRendererSorintRender();
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
        CurrentRole.RoleMoveMoment.SetTargetPositionAndChangeActionState(movePostion);
    }

    public void ChangeTarget(RoleBase targetRole)
    {
        DebugHelper.LogFormat("target name == {0}, team id = {1}, currentRole = {2} team id = {3} ", targetRole.MonoRoleRender.name, targetRole.TeamId, name, CurrentRole.TeamId);
        if (targetRole.TeamId == CurrentRole.TeamId)
        {
            return;
        }

        CurrentRole.RoleSearchTarget.SetTarget(targetRole);
        DebugHelper.Log(" attack  " + targetRole.InstanceId);
    }

    private void SetRendererSorintRender()
    {
        currentSortingGroup.sortingOrder = (int)((6 - transform.position.y) * 1000);
    }


}


