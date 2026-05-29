using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// Attempt at making custom timeline scripts for Falling-Gamemode
/// </summary>
public class StartPositionBehaviour : PlayableBehaviour {
    public List<TimelineStartObject> Objects;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
        if (playable.GetTime() <= 0.01f)
            foreach (var n in Objects)
                n.Start();
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info) {
        base.OnBehaviourPlay(playable, info);

    }

}
[Serializable]
public class StartPositionClip : PlayableAsset {
    public StartPositionBehaviour template = new StartPositionBehaviour();


    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        => ScriptPlayable<StartPositionBehaviour>.Create(graph, template);

}
[TrackBindingType(typeof(Transform))]
public class StartPositionTrack : TrackAsset { }

[Serializable]
public class TimelineStartObject {
    public Transform GameObject;
    public Vector3 startPos;
    public Vector3 startRot;

    public void Start() {
        GameObject.position = startPos;
        GameObject.rotation = Quaternion.Euler(startRot);
    }
}