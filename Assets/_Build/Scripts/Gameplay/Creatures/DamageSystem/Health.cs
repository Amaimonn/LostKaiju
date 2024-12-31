using LostKaiju.Utils;

namespace LostKaiju.Gameplay.Creatures.DamageSystem
{
    public class Health : ClampedValue<int>
    {
        public Health(int bottomLimit, int maxHealth, int initialValue) : base(bottomLimit, maxHealth, initialValue)
        {
        }

        /// <summary>
        /// A simple constructor for determining health with a lower limit of 0 and a maximum health at the beginning.
        /// </summary>
        public Health(int maxHealth) : base(minValue: 0, maxValue: maxHealth, initialValue: maxHealth)
        {
        }

        public void Increase(int amount)
        {
            SetValue(CurrentValue + amount);
        }

        public void Decrease(int amount)
        {
            SetValue(CurrentValue - amount);
        }
    }
}
