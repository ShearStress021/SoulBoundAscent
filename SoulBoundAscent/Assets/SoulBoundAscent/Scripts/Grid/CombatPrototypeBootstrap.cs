using System.Collections;
using System.Collections.Generic;
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


        [Header("Movement")]
        [SerializeField] private float movementInterval = 0.5f;
        [SerializeField] private float movementDuration = 0.25f;

        private CombatGrid combatGrid;
        private readonly List<UnitRuntime> runtimeUnits = new();
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

            runtimeUnits.Clear();
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

            StartCoroutine(MovementLoop());
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

            runtimeUnits.Add(new UnitRuntime(combatUnit,unit.transform));
        }

        private IEnumerator MovementLoop()
        {
            var wait = new WaitForSeconds(movementInterval);

            while (true)
            {
                yield return wait;

                var movements = MoveUnits();

                if(movements.Count > 0)
                {
                    yield return AnimateMovements(movements);
                }
            }
        }




        private Dictionary<Transform, Vector3> MoveUnits()
        {
            var movements = new Dictionary<Transform, Vector3>();
            foreach (var runtimeUnit in runtimeUnits)
            {
                var target = FindNearestOpponent(runtimeUnit.Model);

                if (target == null)
                {
                    continue;
                }

                var distance = GetDistance(
                    runtimeUnit.Model.Position,
                    target.Position);

                if (distance <= 1)
                {
                    continue;
                }

                if (!TryChooseNextPosition(
                        runtimeUnit.Model,
                        target,
                        out var destination))
                {
                    continue;
                }

                if (combatGrid.TryMoveUnit(
                        runtimeUnit.Model,
                        destination))
                {
                    movements[runtimeUnit.Visual] =
                        GridToWorld(destination, 0.5f);
                }
            }

            return movements;
        }
        private CombatUnit FindNearestOpponent(CombatUnit unit)
        {
            CombatUnit nearest = null;
            var nearestDistance = int.MaxValue;

            foreach (var candidate in runtimeUnits)
            {
                if (candidate.Model.Team == unit.Team)
                {
                    continue;
                }

                var distance = GetDistance(
                    unit.Position,
                    candidate.Model.Position);

                if (distance < nearestDistance)
                {
                    nearest = candidate.Model;
                    nearestDistance = distance;
                }
            }

            return nearest;
        }

        private static int GetDistance(
            GridPosition first,
            GridPosition second)
        {
            return Mathf.Abs(first.X - second.X) +
                   Mathf.Abs(first.Y - second.Y);
        }

        private bool TryChooseNextPosition(
    CombatUnit unit,
    CombatUnit target,
    out GridPosition destination)
        {
            var current = unit.Position;
            var deltaX = target.Position.X - current.X;
            var deltaY = target.Position.Y - current.Y;

            var horizontalStep = new GridPosition(
                current.X + GetDirection(deltaX),
                current.Y);

            var verticalStep = new GridPosition(
                current.X,
                current.Y + GetDirection(deltaY));

            if (Mathf.Abs(deltaY) >= Mathf.Abs(deltaX))
            {
                if (CanEnter(verticalStep))
                {
                    destination = verticalStep;
                    return true;
                }

                if (CanEnter(horizontalStep))
                {
                    destination = horizontalStep;
                    return true;
                }
            }
            else
            {
                if (CanEnter(horizontalStep))
                {
                    destination = horizontalStep;
                    return true;
                }

                if (CanEnter(verticalStep))
                {
                    destination = verticalStep;
                    return true;
                }
            }

            destination = current;
            return false;
        }

        private bool CanEnter(GridPosition position)
        {
            return combatGrid.IsInBounds(position) &&
                   !combatGrid.GetCell(position).IsOccupied;
        }

        private static int GetDirection(int difference)
        {
            if (difference > 0)
            {
                return 1;
            }

            if (difference < 0)
            {
                return -1;
            }

            return 0;
        }
        private IEnumerator AnimateMovements(Dictionary<Transform, Vector3> movements)
        {
            var startingPositions =
                new Dictionary<Transform, Vector3>();

            foreach (var movement in movements)
            {
                startingPositions[movement.Key] =
                    movement.Key.localPosition;
            }

            var elapsed = 0f;

            while (elapsed < movementDuration)
            {
                elapsed += Time.deltaTime;

                var progress = movementDuration <= 0f
                    ? 1f
                    : Mathf.Clamp01(elapsed / movementDuration);

                foreach (var movement in movements)
                {
                    movement.Key.localPosition = Vector3.Lerp(
                        startingPositions[movement.Key],
                        movement.Value,
                        progress);
                }

                yield return null;
            }

            foreach (var movement in movements)
            {
                movement.Key.localPosition = movement.Value;
            }
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

        private sealed class UnitRuntime
        {
            public CombatUnit Model { get; }
            public Transform Visual { get; }

            public UnitRuntime(CombatUnit model, Transform visual)
            {
                Model = model;
                Visual = visual;
            }
        }
    }
}
