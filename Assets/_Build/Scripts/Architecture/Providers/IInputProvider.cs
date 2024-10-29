using R3;

public interface IInputProvider : IService
{
    public float GetHorizontal { get; }
    public Observable<bool> OnHorizontalCanceled  { get; }

    public float GetVertical { get; }
    public Observable<bool> OnVerticalCanceled  { get; }

    public bool GetJump { get; }

    public bool GetShift { get; }
}
