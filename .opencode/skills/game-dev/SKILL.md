---
name: game-dev
description: Use when discussing game architecture, engine choice, project structure, game design patterns, build pipelines, or asset workflows. Use ONLY for cross-cutting game development concerns, not for language-specific or engine-specific topics.
---

# Game Development

## Architecture Patterns

- **ECS (Entity-Component-System)**: Decouple data from behavior. Prefer composition over inheritance.
- **Service Locator**: Register global services (audio, input, save) via a locator rather than singletons.
- **State Machine**: Model game states (Menu, Playing, Paused, GameOver) with a finite state machine.
- **Observer / Event Bus**: Decouple systems with events (OnEnemyDeath, OnItemPickup, etc.).
- **Command Pattern**: Encapsulate player actions for undo/replay/netcode.

## Project Organization

```
src/
  Core/         # Engine-agnostic base classes, interfaces, math
  Game/         # Game-specific logic (systems, factories, managers)
  UI/           # Menu, HUD, inventory screens
  Data/         # ScriptableObjects, configs, balance tables
  FX/           # VFX, audio, animation controllers
  Tools/        # Editor scripts, build automation
```

## Build Pipeline

- Scriptable Build Pipeline for Unity (asset bundles, addressables).
- Automated builds via CLI + CI (GitHub Actions, Jenkins).
- Version your builds (Major.Minor.Patch + build number).

## Common Pitfalls

- **Premature optimization**: Profile first, optimize second.
- **Tight coupling**: Systems should communicate through events, not direct references.
- **Magic numbers**: Use ScriptableObjects for all balance/configuration values.
- **Hollow scope**: Build a vertical slice (one complete feature) before spreading horizontally.
