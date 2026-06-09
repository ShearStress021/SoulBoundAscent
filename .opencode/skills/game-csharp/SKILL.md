---
name: game-csharp
description: Use when writing game-specific C# logic for Soulbound Ascent: combat rules, targeting, AI decisions, state machines, stats, modifiers, save/load data models, deterministic simulations, and pure C# systems separate from Unity scene glue.
---

# Soulbound Ascent Game C#

## Boundary

Use this skill for gameplay rules. Keep Unity-specific rendering, prefab, scene, and Inspector work in `unity-csharp`.

Good pure C# candidates:

- Grid coordinates and occupancy rules.
- Unit runtime state.
- Target selection.
- Damage and healing calculation.
- Type advantage and synergy modifiers.
- Battle phase state.
- Save data DTOs.

## Combat Slice Rules

Implement the first combat slice before deeper systems:

- 4 heroes vs one floor config.
- Nearest valid target selection.
- Cell-by-cell movement.
- Adjacent melee range for first pass.
- Fixed attack timers from attack speed.
- Damage, death, retargeting, win/loss.

## State And Data

Prefer explicit state over boolean piles:

```csharp
public enum UnitState
{
    Idle,
    Moving,
    Attacking,
    Dead
}
```

Runtime objects should copy from config:

```csharp
public sealed class UnitRuntime
{
    public string Id { get; }
    public GridPosition Position { get; private set; }
    public UnitStats Stats { get; }
    public int CurrentHp { get; private set; }
}
```

## Damage Pipeline

Keep the MVP formula readable:

```text
reduced = incomingDamage * (1 - relevantArmorPercent)
final = max(1, reduced - relevantDefense)
advantage: +20% damage
disadvantage: -10% damage
```

Apply modifiers in an obvious order:

1. Base stat and skill value.
2. Type advantage.
3. Synergy or buff modifiers.
4. Armor percentage.
5. Flat defense.
6. Clamp and apply.

## Targeting And AI

- Pick nearest living enemy by Manhattan distance.
- Retarget when the current target dies.
- If tied, use deterministic ordering such as lowest row, then column, then stable unit id.
- Keep AI decision-making separate from movement execution.

## Save Data

- Save serializable data only: roster, town state, floor progress, resources, revive timers.
- Do not save scene object references.
- Include a save version field immediately.

## Testing Bias

For pure logic, prefer tests or small console-style harnesses that verify:

- Damage formula.
- Type advantage.
- Target selection.
- Movement legality.
- Win/loss detection.
- Save DTO round trip.
