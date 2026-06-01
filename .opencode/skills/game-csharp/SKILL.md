---
name: game-csharp
description: Use when writing game-specific C# logic — state machines, object pooling, event systems, serialization, game math, combat systems, AI behavior trees, or save/load. Differentiate from unity-csharp: this is for game logic patterns, not Unity API usage.
---

# Game C# Programming

## State Machines

```csharp
public interface IState { void Enter(); void Tick(float dt); void Exit(); }
public class StateMachine
{
    private Dictionary<Type, IState> _states;
    private IState _current;
    public void Change<T>() where T : IState { ... }
}
```

- Use hierarchical state machines for complex AI (Idle → Patrol → Alert → Combat).
- State machines > booleans for character/UI/enemy behavior.

## Event System

```csharp
public static class Events
{
    public static event Action<Unit, Unit> OnUnitDied;
    public static void UnitDied(Unit victim, Unit killer) => OnUnitDied?.Invoke(victim, killer);
}
```

- Or use a message bus with interfaces (`IUnitDiedHandler`) for type safety.
- Always unsubscribe in `OnDisable` / `Dispose`.

## Object Pooling

```csharp
public class ObjectPool<T> where T : Component
{
    private Stack<T> _pool = new();
    public T Get() => _pool.Count > 0 ? _pool.Pop() : Create();
    public void Return(T obj) { obj.gameObject.SetActive(false); _pool.Push(obj); }
}
```

## Game Math

- **Lerp**: `Mathf.Lerp(current, target, Time.deltaTime * speed)` for smooth interpolation.
- **MoveTowards**: `Vector3.MoveTowards` for constant-speed movement.
- **SmoothDamp**: `Vector3.SmoothDamp` for camera follow, UI animation.
- **Damage formula**: `damage = baseDamage * (1 - armor / (armor + constant))` for diminishing returns.

## Save / Load

```csharp
[Serializable]
public class SaveData
{
    public List<HeroData> Heroes;
    public TownData Town;
    public int CurrentFloor;
}
```

- Use `JsonUtility` or `Newtonsoft.Json` for serialization.
- Write to `Application.persistentDataPath`.
- Version your save data for forward compatibility.

## Combat System

- Separate damage calculation into a `DamageCalculator` with modifiers/debuffs/buffs applied as a pipeline.
- Use `ScriptableObject` for all ability/stat definitions.
- Combat resolution: gather modifiers → compute hit → apply damage → trigger events.

## AI Behavior Trees

- Root → Selector → Sequence → Action/Condition.
- Use `ScriptableObject` trees for designer-friendly AI.
- Keep leaf Actions small and testable.
