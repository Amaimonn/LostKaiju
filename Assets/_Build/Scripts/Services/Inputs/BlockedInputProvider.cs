using R3;

namespace LostKaiju.Services.Inputs
{
    public class BlockedInputProvider : IInputProvider
    {
        public float GetHorizontal => 0;

        public Observable<bool> OnHorizontalCanceled => _horizontalCanceled.Skip(1);

        public float GetVertical => 0;

        public Observable<bool> OnVerticalCanceled => _verticalCanceled.Skip(1);

        public bool GetJump => false;

        public bool GetShift => false;

        public bool GetAttack => false;

        public Observable<Unit> OnEscape => new Subject<Unit>();
        public Observable<Unit> OnOptions => new Subject<Unit>();

        private readonly ReactiveProperty<bool> _horizontalCanceled = new(true);
        private readonly ReactiveProperty<bool> _verticalCanceled = new(true);
    }
}