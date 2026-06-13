
using SoulBoundAscent.Units;

namespace SoulBoundAscent.Grid

{
    public sealed class CombatGrid
    {
        private readonly CombatGridCell[,] cells;

        public int Columns { get; }
        public int Rows { get; }

        public CombatGrid(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            cells = new CombatGridCell[columns, rows];

            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    cells[x, y] = new CombatGridCell(new GridPosition(x, y));
                }
            }
        }

        public bool IsInBounds(GridPosition position)
        {
            return position.X >= 0 &&
                   position.X < Columns &&
                   position.Y >= 0 &&
                   position.Y < Rows;
        }

        public CombatGridCell GetCell(GridPosition position)
        {
            return cells[position.X, position.Y];
        }

        public bool TryMoveUnit(CombatUnit unit, GridPosition destination)
        {
            if(unit == null || !IsInBounds(destination))
            {
                return false;
            }

            var currentPosition = unit.Position;

            if (!IsInBounds(currentPosition))
            {
                return false;
            }


            var currentCell = GetCell(currentPosition);
            var destinationCell = GetCell(destination);

            if(currentCell.Occupant != unit || destinationCell.IsOccupied)
            {
                return false;
            }

            currentCell.ClearOccupant();
            destinationCell.SetOccupant(unit);
            unit.SetPosition(destination);
            return true;

        }
        public bool RemoveUnit(CombatUnit unit)
        {
            if (unit == null || !IsInBounds(unit.Position))
            {
                return false;
            }

            var cell = GetCell(unit.Position);

            if (cell.Occupant != unit)
            {
                return false;
            }

            cell.ClearOccupant();
            return true;
        }
    }
}
