using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonRolePoint : MonoBehaviour
{

    public int Type_1;
    public int Type_2;
    public int Type_3;

    public int GetType(WeaponProfessionEnum professionEnum)
    {
        switch (professionEnum)
        {
            case WeaponProfessionEnum.None:
                break;
            case WeaponProfessionEnum.Fighter:
                return Type_1;
            case WeaponProfessionEnum.Shooter:
                return Type_2;
            case WeaponProfessionEnum.Magic:
                return Type_3;
            default:
                break;
        }
        return 0;
    }

}
