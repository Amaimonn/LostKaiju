using R3;

using Assets._Build.Scripts.Architecture.Services;

namespace Assets._Build.Scripts.Architecture.Providers
{
    public interface IInputProvider : IService
    {
        public float GetHorizontal { get; }
        public Observable<bool> OnHorizontalCanceled  { get; }

        public float GetVertical { get; }
        public Observable<bool> OnVerticalCanceled  { get; }

        public bool GetJump { get; }

        public bool GetShift { get; }
    }
}
