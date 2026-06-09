# Soulbound Ascent Agent Guide

## Project Goal

Soulbound Ascent is a Unity PC semester vertical slice, not a full commercial game. The player is the Master: they win through preparation, deployment, roster decisions, and readable auto-battle outcomes.

Keep every implementation decision pointed at the guaranteed demo path:

- 5x6 grid.
- 4 deployable heroes.
- At least 3 playable floors.
- One town screen for summon/train/deploy.
- Auto movement, targeting, damage, death, and win/loss.
- Pause inspection and combat log.
- Save/load.
- At least one hero death/revival moment.

## Demo-First Build Gates

Build in gates, not feature piles:

- **Build 0: Unity Setup And First Scene** — the team can open the Unity project, run the first scene, and produce a Windows test build.
- **Build 1: Combat Core** — one floor is playable from deployment to win/loss, and the player can explain what happened.
- **Build 1.5: Combat Hardening** — two floors run from data, and combat can accept external hero/floor inputs.
- **Build 2: Preparation Loop** — player can summon, train, deploy real roster heroes, clear a floor, gain XP, save/load, and return to town.
- **Build 3: Consequence Loop And Final Demo** — a new player can complete the demo without developer explanation.

## Scope Rules

- Build playable combat before town systems.
- Use placeholder cubes, colors, text, and simple VFX before final art.
- Prefer ScriptableObjects for balance/config data such as units, jobs, enemies, floors, items, and squads.
- Keep gameplay logic testable outside scene glue where practical.
- Choose deterministic, readable combat over clever simulation.
- Cut restaurant, invasion, advanced equipment, multiple consumables, large hero pools, and boss complexity before cutting combat clarity.
- Do not add broad systems unless they serve the current vertical slice.
- If Build 1 is late, cut all town work until combat is playable.
- If Build 2 is late, cut squads, synergies, equipment, and items.
- If Build 3 is late, cut Floor 5 before cutting polish/readability.
- If save/load is unstable, simplify save data instead of adding new systems.
- No new systems after final playtest starts.

## Unity Repository Expectations

The Unity project should eventually include `Assets/`, `Packages/`, and `ProjectSettings/`.

Before committing Unity assets, set up:

- A Unity-focused `.gitignore`.
- Git LFS for binary assets such as `.png`, `.psd`, `.wav`, `.fbx`, `.blend`, and large `.unitypackage` files.

Never commit generated Unity folders such as `Library/`, `Temp/`, `Obj/`, `Build/`, `Builds/`, `Logs/`, or `UserSettings/`.

## Code And Architecture Preferences

- Use simple managers/services for the MVP. Avoid heavy ECS/DOTS unless profiling proves the need.
- Treat grid coordinates as simulation truth. World positions are presentation.
- Separate Unity behaviours from pure gameplay rules when the separation is cheap.
- Use events for cross-system notifications, but keep event ownership obvious.
- Avoid global static state except for small, deliberate bootstrap services.
- Prefer small, inspectable classes over abstract frameworks.

## First Slice Done Means

A new player can open the combat scene, deploy four placeholder heroes, start the battle, watch units move and attack, pause to inspect a unit, read the combat log, and reach victory or defeat without console errors.

## Final Demo Minimum

The smallest acceptable capstone demo includes one town screen, summon/train, 4-hero deployment, 3 clearable floors, one death/revival moment, save/load, readable UI, and no softlocks.
