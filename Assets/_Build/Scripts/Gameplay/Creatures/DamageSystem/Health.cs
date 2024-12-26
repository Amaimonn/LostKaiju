using LostKaiju.Utils;

namespace LostKaiju.Gameplay.Creatures.DamageSystem
{
    public class Health : ClampedValue<int>
    {
        public Health(int bottomLimit, int topLimit, int initialValue) : base(bottomLimit, topLimit, initialValue)
        {
        }
    }
}
