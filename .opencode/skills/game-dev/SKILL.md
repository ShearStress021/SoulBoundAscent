---
name: game-dev
description: Use for Soulbound Ascent game architecture, Unity project structure, scene/system boundaries, MVP scope review, asset workflow, build pipeline, and cross-cutting game-development decisions. Use when deciding how systems should fit together, not for narrow C# syntax or Unity API details.
---

# Soulbound Ascent Game Development

## Architecture North Star

Build the semester vertical slice before expanding the game. The first proof is a playable combat scene: 5x6 grid, 4 deployed heroes, 1 enemy floor, auto movement, targeting, damage, win/loss, pause inspection, and combat log.

Prefer simple, observable systems:

- `BattleManager` owns battle phase flow.
- Grid/occupancy owns legal positions.
- Unit logic owns state and intent.
- Combat calculation owns damage/healing math.
- UI listens to events and displays state.
- ScriptableObjects hold balance/config data.

## Folder Shape

When the Unity project exists, prefer this high-level layout:

```text
Assets/
  SoulboundAscent/
    Scenes/
    Scripts/
      Core/
      Battle/
      Grid/
      Units/
      UI/
      Data/
      Save/
      Tools/
    Prefabs/
    ScriptableObjects/
      Units/
      Floors/
      Jobs/
      Squads/
      Items/
    Art/
    Audio/
```

Keep prototype-only assets under clearly named folders such as `Art/Placeholder`.

## Scope Guardrails

- Recommend vertical slices over broad unfinished systems.
- Prefer managers/services over heavy ECS for this MVP.
- Do not design for multiplayer, procedural content, deep economies, or large hero pools yet.
- Treat town systems as dependent on proven combat.
- Cut restaurant, invasion, advanced equipment, and complex bosses before cutting combat readability.

## Architecture Review Checklist

- Can the current feature be demonstrated in play mode?
- Does it support the first combat slice?
- Is configuration data outside code where designers need to tune it?
- Can UI be changed without rewriting combat rules?
- Can combat rules be tested without relying on scene timing?
- Is there one obvious owner for each state transition?

## Build And Asset Workflow

- Add a Unity `.gitignore` and Git LFS before committing binary assets.
- Keep scenes small and avoid multiple people editing the same scene when prefabs can isolate work.
- Use placeholder art until system behavior is clear.
- Document required Unity version once the project is created.
