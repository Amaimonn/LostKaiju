using UnityEngine;

public static class AnimationClips
{
    public static readonly int IDLE = Animator.StringToHash("idle");
    public static readonly int SITTING = Animator.StringToHash("sitting");
    public static readonly int WALK = Animator.StringToHash("walk");
    public static readonly int LOOK_AROUND = Animator.StringToHash("look_around");
    public static readonly int EMPTY = Animator.StringToHash("empty");
}
