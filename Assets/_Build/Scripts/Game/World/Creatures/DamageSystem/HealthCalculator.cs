using LostKaiju.Utils;

namespace LostKaiju.Game.World.Creatures.DamageSystem
{
    public class HealthCalculator : ClampedValue<int>
    {
        public HealthCalculator(int bottomLimit, int maxHealth, int initialValue) : base(bottomLimit, maxHealth, initialValue)
        {
        }

        /// <summary>
        /// A simple constructor for determining health with a lower limit of 0 and a maximum health at the beginning.
        /// </summary>
        public HealthCalculator(int maxHealth) : base(minValue: 0, maxValue: maxHealth, initialValue: maxHealth)
        {
        }

        public HealthCalculator(int maxHealth, int initialValue) : base(minValue: 0, maxValue: maxHealth, initialValue: initialValue)
        {
        }

        public void Restore(int amount)
        {
            SetValue(CurrentValue + amount);
        }

        public void Decrease(int amount)
        {
            SetValue(CurrentValue - amount);
        }
    }
}
