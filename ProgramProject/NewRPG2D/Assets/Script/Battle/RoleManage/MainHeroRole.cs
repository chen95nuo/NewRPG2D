﻿using Assets.Script.Battle.RoleState;

namespace Assets.Script.Battle
{
   public class MainHeroRole : RoleBase
    {
        public override void InitFSM()
        {
            base.InitFSM();
            RoleIdleState idleState = new RoleIdleState();
            RoleActionStateDic.Add((int)ActorStateEnum.Idle, idleState);
            RoleActionStateDic.Add((int)ActorStateEnum.Run, new RoleRunState());
            RoleActionStateDic.Add((int)ActorStateEnum.Hit, new RoleHitState());
            RoleActionStateDic.Add((int)ActorStateEnum.NormalAttack, new RoleNormalAttackState());
            RoleActionStateDic.Add((int)ActorStateEnum.Skill1, new RoleSkill1State());
            RoleActionStateDic.Add((int)ActorStateEnum.Skill2, new RoleSkill2State());
            RoleActionMachine.SetCurrentState(idleState);
        }

        public override void FixedUpdateLogic(float deltaTime)
       {
          
       }

       public override void Dispose()
       {
           
       }

        public override void Death()
        {
            base.Death();
            GameRoleMgr.instance.RolesHeroList.Remove(this);
        }
    }
}
