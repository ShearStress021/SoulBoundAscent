using SoulBoundAscent.Units;

namespace SoulBoundAscent.Grid
{
    public sealed class CombatGridCell
    {
        public GridPosition Position { get; }
        public bool IsOccupied => Occupant != null;
        public CombatUnit Occupant { get; private set; }

        public CombatGridCell(GridPosition position)
        {
            Position = position;
        }

        public void SetOccupant(CombatUnit unit)
        {
            Occupant = unit;
        }

        public void ClearOccupant()
        {
            Occupant = null;
        }
    }
}
