using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Battle
{
    public class WeaponMoment
    {
        public WeaponType CurrentWeaponType;
        private RoleBase currentHeroRole;
        public void SetCurrentRole(RoleBase mRole)
        {
            currentHeroRole = mRole;
        }
    }
}
