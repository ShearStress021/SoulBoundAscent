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
    }
}
