---
name: unity-csharp
description: Use when writing or reviewing Unity C# code, including MonoBehaviours, ScriptableObjects, Editor scripting, Unity performance optimization, or Unity-specific API usage.
---

# Unity C# Development

## MonoBehaviour Lifecycle

```csharp
Awake()    → OnEnable() → Start() → FixedUpdate() → Update() → LateUpdate() → OnDisable() → OnDestroy()
```

- `Awake`: Initialize references, no assumptions about other objects.
- `Start`: Initialize state that depends on other objects being awake.
- Avoid `Update()` loops when possible — use events, coroutines, or DOTween.

## ScriptableObject Best Practices

- Use ScriptableObjects for all configurable data (stats, items, enemy definitions).
- Never mutate ScriptableObjects at runtime unless intentional (localization, save data).
- Use `CreateAssetMenu` attribute for editor-friendly creation.

## Performance

- **Object Pooling**: Reuse GameObjects instead of Instantiate/Destroy.
- **Avoid `Find()` / `FindObjectOfType()`**: Cache references via Inspector or dependency injection.
- **Use `TryGetComponent`** over `GetComponent` when component may not exist.
- **Burst Compiler**: Mark job structs with `[BurstCompile]`.
- **Job System**: Use `IJobParallelFor` for data-parallel work.
- **ECS / DOTS**: Consider for large-scale entity counts.

## Editor Scripting

```csharp
[CustomEditor(typeof(MyComponent))]
public class MyComponentEditor : Editor { ... }
```

- `[Range(min, max)]`, `[Header("...")]`, `[Tooltip("...")]` for Inspector UX.
- `[ExecuteAlways]` for editor-time behaviors.

## Coroutines vs Async

| Approach | Use Case |
|---|---|
| `StartCoroutine` | Time delays, frame waits, sequences |
| `async/await` | Web requests, file I/O, any .NET Task |
| `UniTask` | Zero-allocation async for Unity (preferred) |

## Unity Addressables

- Use Addressables for dynamic asset loading.
- `Addressables.LoadAssetAsync<T>()` / `Addressables.InstantiateAsync()`.
- Manage dependencies via labels, not direct references.
