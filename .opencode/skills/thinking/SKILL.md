---
name: thinking
description: Use when the user asks for analysis, reasoning, trade-off evaluation, system decomposition, architecture review, or needs to think through a problem before coding. Use BEFORE writing code for non-trivial features.
---

# Thinking & Analysis

## System Decomposition Frame

1. **Inputs**: What data enters this system?
2. **Processes**: What transformations happen?
3. **Outputs**: What data leaves this system?
4. **Side effects**: What state changes occur externally?
5. **Dependencies**: What other systems does this rely on?

## Decision Matrix

| Criteria | Option A | Option B |
|---|---|---|
| Complexity | High | Low |
| Performance | Fast | Medium |
| Flexibility | Rigid | Modular |
| Dev time | 2 days | 1 day |

Evaluate trade-offs against project scope (MVP constraints).

## First-Principles Breakdown

1. What are we building? (one clear sentence)
2. What is the minimum version of this that works?
3. What can we cut and still have something playable?
4. What is the riskiest unknown? (test that first)

## Architecture Review Checklist

- [ ] Does each system have a single responsibility?
- [ ] Can systems be tested in isolation?
- [ ] Are dependencies acyclic? (A → B → C, not A → B → A)
- [ ] Can each system be replaced without rewriting others?
- [ ] Does the data flow match the problem domain?

## Prior Art Search

Before designing a novel solution, ask:
- "How does [GAME/TFT/Langrisser/DarkestDungeon] handle this?"
- "What patterns exist for [auto-battle/hero-rostering/tower-climbing]?"
- "Is there a Unity asset or package that solves this?"

## When To Stop Thinking

- You have a clear design for the next 2-3 features.
- You have identified the riskiest unknown and a plan to test it.
- You have a working prototype path that cuts scope before cutting quality.
