using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unbegames.Services {
  /// <summary>
  /// Queue Pool.
  /// </summary>
  /// <typeparam name="T">Type of the objects in the pooled hashsets.</typeparam>
  public static class QueuePool<T> {
    // Object pool to avoid allocations.
    static readonly ObjectPool<Queue<T>> pool = new ObjectPool<Queue<T>>(null, q => q.Clear());

    /// <summary>
    /// Get a new Queue
    /// </summary>
    /// <returns>A new HashSet</returns>
    public static Queue<T> Get() => pool.Get();

    /// <summary>
    /// Get a new queue PooledObject.
    /// </summary>
    /// <param name="value">Output typed HashSet.</param>
    /// <returns>A new HashSet PooledObject.</returns>
    public static ObjectPool<Queue<T>>.PooledObject Get(out Queue<T> value) => pool.Get(out value);

    /// <summary>
    /// Release the queue to the pool.
    /// </summary>
    /// <param name="toRelease">hashSet to release.</param>
    public static void Release(Queue<T> toRelease) => pool.Release(toRelease);
  }
}
