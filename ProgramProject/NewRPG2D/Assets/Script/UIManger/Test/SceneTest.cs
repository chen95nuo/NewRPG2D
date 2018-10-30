using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Invoke("GoBack", 2.0f);
    }

    void GoBack()
    {
        SceneManager.LoadScene("EasonMainScene");
    }
}
