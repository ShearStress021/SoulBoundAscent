---
name: unity-csharp
description: Use when writing or reviewing Unity-specific C# for Soulbound Ascent, including MonoBehaviours, ScriptableObjects, prefabs, scenes, UI wiring, editor tools, pooling, Unity lifecycle, save paths, and performance in Unity.
---

# Soulbound Ascent Unity C#

## Unity Role

Use Unity behaviours as scene adapters and presentation glue. Keep reusable combat, targeting, damage, and save rules in plain C# classes when practical.

## MonoBehaviour Conventions

- Use `[SerializeField] private` for Inspector references.
- Cache component references in `Awake`.
- Initialize cross-object state in `Start` or explicit setup methods.
- Subscribe in `OnEnable`; unsubscribe in `OnDisable` or `OnDestroy`.
- Keep `Update` lean. Prefer events, coroutines, or explicit battle ticks.
- Use `[Header]`, `[Tooltip]`, and `[Range]` for Inspector clarity.

```csharp
public sealed class UnitView : MonoBehaviour
{
    [SerializeField] private Transform modelRoot;
    [SerializeField] private HealthBar healthBar;

    private UnitRuntime unit;

    public void Bind(UnitRuntime unitRuntime)
    {
        unit = unitRuntime;
        healthBar.SetValue(unit.CurrentHp, unit.MaxHp);
    }
}
```

## ScriptableObjects

Use ScriptableObjects for data the team will tune:

- Unit configs.
- Enemy configs.
- Floor wave configs.
- Job and squad definitions.
- Item and meal configs.
- VFX/audio lookup data.

Do not mutate ScriptableObject assets during battle. Copy asset data into runtime state objects.

## Scene And Prefab Guidance

- Keep the first combat scene simple: camera, grid root, unit spawn roots, canvas, battle bootstrap.
- Prefer prefab variants for hero/enemy visuals.
- Keep grid cells clickable in preparation mode only.
- Block deployment input after battle starts except pause/inspection.
- Use TextMeshPro for combat log, damage numbers, and placeholder labels.

## Unity Save/Load

- Use `Application.persistentDataPath` for local saves.
- Keep save data as serializable DTOs, not live MonoBehaviours.
- Version save data from the first implementation.

## Performance Baseline

- Pool damage numbers, hit flashes, and frequently spawned VFX.
- Avoid `FindObjectOfType` and broad scene searches in runtime loops.
- Avoid physics queries for grid truth; use grid occupancy data.
- Profile before adding DOTS, jobs, Addressables, or complex pooling frameworks.
