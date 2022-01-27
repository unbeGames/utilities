using System.Collections.Generic;

namespace Unbegames.Services.Pool {
  /// <summary>
  /// HashSet Pool.
  /// </summary>
  /// <typeparam name="T">Type of the objects in the pooled hashsets.</typeparam>
  public static class HashSetPool<T> {
    // Object pool to avoid allocations.
    static readonly ObjectPool<HashSet<T>> pool = new ObjectPool<HashSet<T>>(null, h => h.Clear());

    /// <summary>
    /// Get a new HashSet
    /// </summary>
    /// <returns>A new HashSet</returns>
    public static HashSet<T> Get() => pool.Get();

    /// <summary>
    /// Get a new HashSet PooledObject.
    /// </summary>
    /// <param name="value">Output typed HashSet.</param>
    /// <returns>A new HashSet PooledObject.</returns>
    public static ObjectPool<HashSet<T>>.PooledObject Get(out HashSet<T> value) => pool.Get(out value);

    /// <summary>
    /// Release an HashSet to the pool.
    /// </summary>
    /// <param name="toRelease">hashSet to release.</param>
    public static void Release(HashSet<T> toRelease) => pool.Release(toRelease);
  }
}
