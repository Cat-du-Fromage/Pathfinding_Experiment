using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object : Data containers (similar to prefab in their behavior)
/// or similar to ECS's archetype (multiple IComponent in one entity)
/// </summary>
public abstract class FlockBehavior : ScriptableObject
{
    public abstract Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
}
