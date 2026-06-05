---
name: game-physics
description: Use for Soulbound Ascent grid movement, cell occupancy, range checks, pathing simplification, battle timing, collision decisions, movement feel, and combat simulation rules. Use when deciding whether to use Unity physics versus deterministic grid logic.
---

# Soulbound Ascent Game Physics

## Core Rule

Grid coordinates are the source of truth. World positions are only visual presentation.

Avoid Rigidbody-driven combat for the MVP unless it is purely visual. Use deterministic grid movement, occupancy maps, and explicit range checks.

## Board Model

MVP board:

- 5 columns x 6 rows.
- Enemy zone: top rows.
- Neutral zone: middle rows.
- Player deployment zone: bottom rows.
- One unit per occupied cell.

Represent positions with integer coordinates:

```csharp
public readonly struct GridPosition
{
    public int X { get; }
    public int Y { get; }
}
```

## Movement

- Move cell-by-cell toward the selected target.
- Use Manhattan distance for MVP.
- Stop when in attack range.
- Do not move through occupied cells unless a later feature explicitly allows it.
- If blocked, try a deterministic alternate neighbor order.
- If no legal step exists, wait and retry on the next movement decision.

Suggested neighbor priority should be stable so bugs are reproducible:

```text
toward target on strongest axis -> alternate axis -> other legal adjacent cells
```

## Timing

- Battle simulation should advance from explicit timers or ticks.
- Movement speed controls time between cell steps.
- Attack speed controls time between attacks.
- Battle speed buttons can scale Unity time for presentation, but core rules should remain deterministic.

## Range

- First melee range: Manhattan distance `<= 1`.
- Ranged units can use Manhattan distance with a configured max range.
- Line of sight, hazards, and diagonal rules are stretch features.

## Collision And Clicks

- Use colliders on grid cells for mouse selection if helpful.
- Do not use physics collisions to decide legal movement, target range, or occupancy.
- During battle, block deployment clicks and allow only pause/inspection.

## Movement Feel

- Animate from current cell world position to next cell world position.
- Use `Vector3.MoveTowards` for constant-speed cell travel or a coroutine over fixed duration.
- Keep arrival snapping exact to avoid drift.
- Trigger attack/VFX from simulation events, not collision impacts.

## Debug Requirements

For movement bugs, log or display:

- Unit id.
- Current cell.
- Target id and target cell.
- Chosen next cell.
- Reason movement failed.

Keep a simple grid debug overlay available during early development.
