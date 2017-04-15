using UnityEngine;
using System;
using System.Collections;

public class SHMainToEntry : SHMonoWrapper
{
    public override void Start() 
    {
        Single.Scene.GoTo(eSceneType.InGame);
    }
}
