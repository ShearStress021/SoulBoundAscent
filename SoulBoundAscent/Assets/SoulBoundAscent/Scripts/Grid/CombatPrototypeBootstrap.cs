using UnityEngine;

namespace SoulBoundAscent.Grid
{
    public sealed class CombatPrototypeBootstrap : MonoBehaviour
    {
        [Header("Board")]
        [SerializeField] private int columns = 5;
        [SerializeField] private int rows = 6;
        [SerializeField] private float cellSize = 1f;

        [Header("Scene Roots")]
        [SerializeField] private Transform gridRoot;
        [SerializeField] private Transform unitsRoot;

        [Header("Cell Materials")]
        [SerializeField] private Material playerZoneMaterial;
        [SerializeField] private Material enemyZoneMaterial;
        [SerializeField] private Material neutralZoneMaterial;

        [Header("Unit Materials")]
        [SerializeField] private Material[] heroMaterials;
        [SerializeField] private Material enemyMaterial;

        private static readonly Vector2Int[] HeroCells =
        {
            new(0, 0),
            new(1, 0),
            new(2, 0),
            new(3, 0),
        };

        private static readonly Vector2Int[] EnemyCells =
        {
            new(1, 5),
            new(2, 5),
            new(3, 5),
        };

        private void Start()
        {
            BuildPrototypeBoard();
        }

        private void BuildPrototypeBoard()
        {
            if (gridRoot == null || unitsRoot == null)
            {
                Debug.LogError("CombatPrototypeBootstrap needs GridRoot and UnitsRoot assigned.", this);
                return;
            }

            ClearChildren(gridRoot);
            ClearChildren(unitsRoot);

            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    CreateCell(x, y);
                }
            }

            for (var i = 0; i < HeroCells.Length; i++)
            {
                var material = heroMaterials != null && i < heroMaterials.Length ? heroMaterials[i] : null;
                CreateUnit($"Hero_{i + 1}", HeroCells[i], material, PrimitiveType.Capsule);
            }

            for (var i = 0; i < EnemyCells.Length; i++)
            {
                CreateUnit($"Enemy_{i + 1}", EnemyCells[i], enemyMaterial, PrimitiveType.Sphere);
            }
        }

        private void CreateCell(int x, int y)
        {
            var cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cell.name = $"Cell_{x}_{y}";
            cell.transform.SetParent(gridRoot, false);
            cell.transform.localPosition = GridToWorld(x, y, 0f);
            cell.transform.localScale = new Vector3(cellSize * 0.92f, 0.05f, cellSize * 0.92f);

            var renderer = cell.GetComponent<Renderer>();
            renderer.sharedMaterial = GetCellMaterial(y);
        }

        private void CreateUnit(string unitName, Vector2Int cell, Material material, PrimitiveType primitiveType)
        {
            var unit = GameObject.CreatePrimitive(primitiveType);
            unit.name = unitName;
            unit.transform.SetParent(unitsRoot, false);
            unit.transform.localPosition = GridToWorld(cell.x, cell.y, 0.45f);
            unit.transform.localScale = new Vector3(cellSize * 0.45f, cellSize * 0.45f, cellSize * 0.45f);

            var renderer = unit.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
        }

        private Material GetCellMaterial(int row)
        {
            if (row <= 1)
            {
                return playerZoneMaterial;
            }

            if (row >= rows - 2)
            {
                return enemyZoneMaterial;
            }

            return neutralZoneMaterial;
        }

        private Vector3 GridToWorld(int x, int y, float height)
        {
            return new Vector3(x * cellSize, height, y * cellSize);
        }

        private static void ClearChildren(Transform root)
        {
            for (var i = root.childCount - 1; i >= 0; i--)
            {
                Destroy(root.GetChild(i).gameObject);
            }
        }
    }
}
