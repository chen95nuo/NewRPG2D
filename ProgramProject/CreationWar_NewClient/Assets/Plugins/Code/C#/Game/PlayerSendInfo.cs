using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[System.Serializable]
    public struct PlayerSendInfo
    {
        /// <summary>
        /// 位置x
        /// </summary>
       public  int x;
        /// <summary>
        /// 位置y
        /// </summary>
       public int y;
        /// <summary>
        /// 位置z
        /// </summary>
       public int z;
        
       /// <summary>
       /// 旋转
       /// </summary>
       public int rotation;

        /// <summary>
        /// 动画状态
        /// </summary>
       public int animStuat;

        /// <summary>
        /// 动画速度
        /// </summary>
       public int animSeed;
 		

    }

