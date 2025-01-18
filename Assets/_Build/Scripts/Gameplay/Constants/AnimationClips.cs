using UnityEngine;

public static class AnimationClips
{
    public static readonly int IDLE = Animator.StringToHash("Idle");
    public static readonly int SITTING = Animator.StringToHash("Sitting");
    public static readonly int WALK = Animator.StringToHash("Walk");
    public static readonly int LOOK_AROUND = Animator.StringToHash("LookAround");
    public static readonly int EMPTY = Animator.StringToHash("Empty");
    public static readonly int ATTACK_FORWARD = Animator.StringToHash("AttackForward");
}
