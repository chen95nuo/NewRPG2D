using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script.Battle.LevelManager
{
    public class LoadLevel : MonoBehaviour
    {
        public void Awake()
        {
            SceneManager.LoadScene("Scene_1");
        }
    }
}
