using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int cols = 5;
    [SerializeField] private int rows = 6;
    [SerializeField] private float cellSize = 1.1f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Material playerZoneMat, enemyZoneMat, neutralZoneMat;

    private void Start() => GenerateGrid();

    private void GenerateGrid()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{y}";

                GridCell gc = cell.GetComponent<GridCell>();
                gc.gridPos = new Vector2Int(x, y);

                Renderer r = cell.GetComponent<Renderer>();
                if (y <= 1)
                {
                    gc.zone = GridCell.ZoneType.Enemy;
                    r.material = enemyZoneMat;
                }
                else if (y >= 4)
                {
                    gc.zone = GridCell.ZoneType.Player;
                    r.material = playerZoneMat;
                }
                else
                {
                    gc.zone = GridCell.ZoneType.Neutral;
                    r.material = neutralZoneMat;
                }
            }
        }
    }
}
