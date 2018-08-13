using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AnimationTest : MonoBehaviour
{
    public SkeletonGraphic test;

    private void Start()
    {
        test.enabled = false;
        Invoke("enableImage", 0.5f);
    }

    private void enableImage()
    {
        test.enabled = true;

        test.AnimationState.SetAnimation(0, "chuxian", false);
    }
}
