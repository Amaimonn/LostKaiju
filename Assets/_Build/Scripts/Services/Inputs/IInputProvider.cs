using R3;

using LostKaiju.Boilerplates.Locator;

namespace LostKaiju.Services.Inputs
{
    public interface IInputProvider : IService
    {
        public float GetHorizontal { get; }
        public Observable<bool> OnHorizontalCanceled  { get; }
        public float GetVertical { get; }
        public Observable<bool> OnVerticalCanceled  { get; }
        public bool GetJump { get; }
        public bool GetShift { get; }
        public bool GetAttack { get; }
        public Observable<Unit> OnEscape { get; }
    }
}
