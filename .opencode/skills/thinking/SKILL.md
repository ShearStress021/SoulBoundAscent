---
name: thinking
description: Use when analyzing Soulbound Ascent design, evaluating trade-offs, reviewing architecture, reducing scope, identifying risks, or deciding the smallest playable version before coding non-trivial features.
---

# Soulbound Ascent Thinking

## Default Frame

Before designing a non-trivial feature, answer:

1. What playable outcome does this support?
2. What is the smallest version that proves it?
3. What data enters and leaves the system?
4. What state changes happen?
5. What can be cut without harming combat clarity?
6. What is the riskiest unknown to test first?

## Project Bias

Prefer decisions that:

- Make the combat slice playable sooner.
- Improve battle readability.
- Reduce scene merge conflicts.
- Keep data tunable through ScriptableObjects.
- Keep logic deterministic and testable.
- Avoid adding town breadth before combat works.

## Trade-Off Format

Compare 2-3 realistic options:

```text
Option:
Effort:
Benefit:
Risk:
Cut path:
Recommendation:
```

End with one recommendation, not a menu of equal choices.

## Scope Questions

Ask these before adding a system:

- Does the player need this to understand the first battle?
- Can this be represented by placeholder UI/data for now?
- Is this a critical path dependency or parallel polish?
- Can it be delayed until after win/loss works?
- Does it create permanent architecture debt if simplified?

## Stop Thinking When

- The next playable checkpoint is clear.
- The smallest implementation is identified.
- The major risk has a test or spike.
- The cut path is explicit.
