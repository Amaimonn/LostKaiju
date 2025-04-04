using UnityEngine;

namespace LostKaiju.Game.Constants
{
    public static class AnimationClips
    {
        public static readonly int IDLE = Animator.StringToHash("Idle");
        public static readonly int SITTING = Animator.StringToHash("Sitting");
        public static readonly int WALK = Animator.StringToHash("Walk");
        public static readonly int LOOK_AROUND = Animator.StringToHash("LookAround");
        public static readonly int LYING = Animator.StringToHash("Lying");
        public static readonly int LYING_SCALES = Animator.StringToHash("LyingScales");
        public static readonly int EMPTY = Animator.StringToHash("Empty");
        public static readonly int ATTACK_FORWARD = Animator.StringToHash("AttackForward");
        public static readonly int AIR_UP = Animator.StringToHash("AirUp");
        public static readonly int AIR_DOWN = Animator.StringToHash("AirDown");
    }
}
