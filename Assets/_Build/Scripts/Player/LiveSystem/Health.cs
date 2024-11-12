namespace LostKaiju.Player.LiveSystem
{
    public class Health : ClampedValue<int>
    {
        public Health(int bottomLimit, int topLimit, int initialValue) : base(bottomLimit, topLimit, initialValue)
        {
        }
    }
}
