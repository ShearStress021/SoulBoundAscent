using SoulBoundAscent.Grid;

namespace SoulBoundAscent.Units
{
    public sealed class CombatUnit
    {
        public string Name { get; }
        public CombatTeam Team { get; }
        public GridPosition Position { get; private set; }
        public CombatUnit CurrentTarget { get; private set; }

        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }
        public int AttackDamage { get; }
        public bool IsDefeated => CurrentHealth <= 0;

        public CombatUnit(string name, CombatTeam team, GridPosition position)
        {
            Name = name;
            Team = team;
            Position = position;


            MaxHealth = 100;
            AttackDamage = 10;
            CurrentHealth = MaxHealth;
        }

        public void SetPosition(GridPosition position)
        {
            Position = position;
        }
        public void SetTarget(CombatUnit target)
        {
            CurrentTarget = target;
        }

        public void ClearTarget()
        {
            CurrentTarget = null;
        }

        public int TakeDamage(int amount)
        {
            if (amount <= 0 || IsDefeated)
            {
                return 0;
            }

            var previousHealth = CurrentHealth;

            CurrentHealth = System.Math.Max(
                0,
                CurrentHealth - amount);

            return previousHealth - CurrentHealth;
        }
    }
}
