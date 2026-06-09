---
name: planning
description: Use when planning Soulbound Ascent milestones, sprints, feature breakdowns, task ownership, dependencies, effort estimates, risk registers, cut lines, and vertical-slice checkpoints.
---

# Soulbound Ascent Planning

## Planning Priority

Month 1 must prove combat. Do not plan town, death, invasion, restaurant, equipment depth, or final polish as active work until the combat loop can be played.

First playable checkpoint:

```text
Deploy 4 heroes on a 5x6 grid -> start auto-battle -> units move/target/attack -> deaths resolve -> victory/defeat screen -> pause inspection and combat log work.
```

## Task Shape

Each task should be:

- Owned by one person.
- Sized to 0.5-2 days when possible.
- Attached to a dependency.
- Verified with visible play-mode behavior or a test.

Use this format:

```text
[ ] Task name (effort: S/M/L) - owner: Name
    Depends on: task or "none"
    Success criteria: observable result
```

## Critical Path For Month 1

```text
Unity scaffold
-> grid cells and coordinates
-> unit runtime model
-> deployment
-> movement
-> targeting
-> damage
-> death
-> win/loss
-> integration/balance pass
```

UI, placeholder art, logs, and VFX should run in parallel after their dependencies are ready.

## Effort Scale

- `S`: half day, known implementation.
- `M`: 1-2 days, clear but needs wiring.
- `L`: 3-5 days, uncertain or cross-system.
- `XL`: too large for a sprint task; split or spike first.

## Cut Order

Cut in this order before reducing combat clarity:

1. Restaurant.
2. Multiple consumable types.
3. Invasion/protection.
4. Advanced equipment states.
5. Complex boss mechanics.
6. Synergy UI polish.
7. Audio/music.

## Risk Register Prompts

Track:

- Risk.
- Likelihood.
- Impact.
- Mitigation.
- Owner.
- Date to reassess.

Common risks: over-scoping, unclear combat, scene merge conflicts, save/load delay, UI flow growth, and placeholder content becoming permanent without intent.
