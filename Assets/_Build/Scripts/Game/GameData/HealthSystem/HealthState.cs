namespace LostKaiju.Game.GameData.HealthSystem
{
    public class HealthState
    {
        public int MaxHealth;
        public int CurrentHealth;

        public HealthState(int health)
        {
            MaxHealth = health;
            CurrentHealth = health;
        }

        public HealthState(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }
}