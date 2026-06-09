using UnityEngine;
using SoulBoundAscent.Units;

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

        private CombatGrid combatGrid;
        private Material fallbackPlayerZoneMaterial;
        private Material fallbackEnemyZoneMaterial;
        private Material fallbackNeutralZoneMaterial;
        private Material fallbackHeroMaterial;
        private Material fallbackEnemyMaterial;

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

            combatGrid = new CombatGrid(columns, rows);

            for (var y = 0; y < combatGrid.Rows; y++)
            {
                for (var x = 0; x < combatGrid.Columns; x++)
                {
                    var gridCell = combatGrid.GetCell(new GridPosition(x, y));
                    CreateCell(gridCell);
                }
            }

            for (var i = 0; i < HeroCells.Length; i++)
            {
                var material = heroMaterials != null && i < heroMaterials.Length ? heroMaterials[i] : null;
                CreateUnit($"Hero_{i + 1}", CombatTeam.Hero, HeroCells[i], material, PrimitiveType.Capsule);
            }

            for (var i = 0; i < EnemyCells.Length; i++)
            {
                CreateUnit($"Enemy_{i + 1}", CombatTeam.Enemy, EnemyCells[i], enemyMaterial, PrimitiveType.Sphere);
            }
        }

        private void CreateCell(CombatGridCell gridCell)
        {
            var position = gridCell.Position;
            var cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cell.name = $"Cell_{position.X}_{position.Y}";
            cell.transform.SetParent(gridRoot, false);
            cell.transform.localPosition = GridToWorld(position, 0f);
            cell.transform.localScale = new Vector3(cellSize * 0.92f, 0.05f, cellSize * 0.92f);

            var renderer = cell.GetComponent<Renderer>();
            renderer.sharedMaterial = GetCellMaterial(position.Y);
        }

        private void CreateUnit(string unitName, CombatTeam team, Vector2Int cell, Material material, PrimitiveType primitiveType)
        {
            var position = new GridPosition(cell.x, cell.y);

            if (!combatGrid.IsInBounds(position))
            {
                Debug.LogWarning($"{unitName} cannot spawn at {position.X},{position.Y}; position is outside the combat grid.", this);
                return;
            }

            var gridCell = combatGrid.GetCell(position);
            if (gridCell.IsOccupied)
            {
                Debug.LogWarning($"{unitName} cannot spawn at {position.X},{position.Y}; cell is already occupied by {gridCell.Occupant.Name}.", this);
                return;
            }

            var combatUnit = new CombatUnit(unitName, team, position);
            gridCell.SetOccupant(combatUnit);

            var unit = GameObject.CreatePrimitive(primitiveType);
            unit.name = unitName;
            unit.transform.SetParent(unitsRoot, false);
            unit.transform.localPosition = GridToWorld(position, 0.5f);
            unit.transform.localScale = new Vector3(cellSize * 0.7f, cellSize * 0.45f, cellSize * 0.7f);

            var renderer = unit.GetComponent<Renderer>();
            renderer.sharedMaterial = GetUnitMaterial(team, material);
        }

        private Material GetCellMaterial(int row)
        {
            if (row <= 1)
            {
                return playerZoneMaterial != null ? playerZoneMaterial : GetFallbackMaterial(ref fallbackPlayerZoneMaterial, new Color(0.25f, 0.6f, 1f));
            }

            if (row >= rows - 2)
            {
                return enemyZoneMaterial != null ? enemyZoneMaterial : GetFallbackMaterial(ref fallbackEnemyZoneMaterial, new Color(1f, 0.32f, 0.24f));
            }

            return neutralZoneMaterial != null ? neutralZoneMaterial : GetFallbackMaterial(ref fallbackNeutralZoneMaterial, new Color(0.75f, 0.75f, 0.68f));
        }

        private Material GetUnitMaterial(CombatTeam team, Material assignedMaterial)
        {
            if (assignedMaterial != null)
            {
                return assignedMaterial;
            }

            return team == CombatTeam.Hero
                ? GetFallbackMaterial(ref fallbackHeroMaterial, new Color(0.12f, 0.95f, 1f))
                : GetFallbackMaterial(ref fallbackEnemyMaterial, new Color(1f, 0.1f, 0.1f));
        }

        private static Material GetFallbackMaterial(ref Material material, Color color)
        {
            if (material != null)
            {
                return material;
            }

            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            material = new Material(shader)
            {
                color = color
            };

            return material;
        }

        private Vector3 GridToWorld(GridPosition position, float height)
        {
            return new Vector3(position.X * cellSize, height, position.Y * cellSize);
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
