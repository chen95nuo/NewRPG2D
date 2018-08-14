using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
public class AnimationTest : MonoBehaviour
{
    public SkeletonGraphic test;
    private TrackEntry testTrack;

    private void Start()
    {
        test.enabled = false;
        Invoke("enableImage", 0.5f);
    }

    private void enableImage()
    {
        test.enabled = true;
        test.AnimationState.SetAnimation(0, "open", false);
        test.AnimationState.Data.DefaultMix = 10;
        Debug.Log(test.AnimationState.Data.DefaultMix);
        test.AnimationState.Data.DefaultMix = 10;
        testTrack = test.AnimationState.AddAnimation(0, "stand", true, 0);

    }
}
