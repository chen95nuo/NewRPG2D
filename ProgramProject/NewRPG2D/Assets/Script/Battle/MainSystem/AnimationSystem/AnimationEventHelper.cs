

using UnityEngine;

namespace Assets.Script.Battle
{
    public class AnimationEventHelper : MonoBehaviour
    {
        [SerializeField] private RoleRender roleRender;
        public void Hit()
        {
           EventManager.instance.SendEvent(EventDefineEnum.AnimationHit, roleRender.CurrentRole.InstanceId);
        }

        public void End()
        {
            EventManager.instance.SendEvent(EventDefineEnum.AnimationEnd, roleRender.CurrentRole.InstanceId);
        }
    }
}
