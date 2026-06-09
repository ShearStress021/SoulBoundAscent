using SoulBoundAscent.Grid;

namespace SoulBoundAscent.Units
{
    public sealed class CombatUnit
    {
        public string Name { get; }
        public CombatTeam Team { get; }
        public GridPosition Position { get; private set; }

        public CombatUnit(string name, CombatTeam team, GridPosition position)
        {
            Name = name;
            Team = team;
            Position = position;
        }

        public void SetPosition(GridPosition position)
        {
            Position = position;
        }
    }
}
