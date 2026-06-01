---
name: planning
description: Use when breaking down features into tasks, creating milestones, estimating effort, managing dependencies, sprint planning, or risk assessment for game development projects. Use BEFORE starting a new sprint or milestone.
---

# Planning

## Milestone Structure

```
Month 1: Core Combat Prototype
  Week 1: Grid deployment + basic movement
  Week 2: Auto-targeting + damage system
  Week 3: Win/loss conditions + 1 test floor
  Week 4: Pause inspection + combat log
  Exit: Player can deploy, battle, and resolve win/loss.
```

Each milestone must have an **exit goal** (one sentence) that is testable.

## Task Breakdown

Each task should be:
- **Independent**: Can be worked on without waiting for others (where possible)
- **Testable**: Has a clear success criteria
- **Sized**: 1-3 days of work (split if larger)
- **Owned**: One person responsible

## Dependency Mapping

```
[Arena Grid] → [Unit Movement] → [Auto-Targeting] → [Damage System]
                                                      ↓
                                              [Win/Loss Conditions]
```

Identify the **critical path** — the longest chain of dependencies. Shorten it by parallelizing independent tasks.

## Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Combat feels random | Medium | High | Pause inspection + combat log |
| Scope too large | High | High | Locked MVP feature table |
| Unity learning curve | Medium | Medium | Simple scenes, no physics |

## Sprint Cadence

- **Week sprint** (1 week): Too short for game dev, forces micro-planning.
- **Two-week sprint**: Balanced for game features.
- **Month sprint**: Better aligned with school semester milestones.

For this project:
- Month 1: Prove combat works
- Month 2: Build hero/town systems
- Month 3: Add content + consequences
- Month 4: Polish + presentation

## Effort Estimation

- **S**: Half day, well-understood
- **M**: 1-2 days, clear approach
- **L**: 3-5 days, some unknowns
- **XL**: 1-2 weeks, significant unknowns (spike first)

Sum estimates per milestone. If total exceeds available time, cut scope (not quality).
