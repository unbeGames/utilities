using System.Collections.Generic;

namespace Unbegames.Services.Pool {
  /// <summary>
  /// List Pool.
  /// </summary>
  /// <typeparam name="T">Type of the objects in the pooled lists.</typeparam>
  public static class ListPool<T> {
    // Object pool to avoid allocations.
    private static readonly ObjectPool<List<T>> pool = new ObjectPool<List<T>>(null, l => l.Clear());

    /// <summary>
    /// Get a new List
    /// </summary>
    /// <returns>A new List</returns>
    public static List<T> Get() => pool.Get();

    /// <summary>
    /// Get a new list PooledObject.
    /// </summary>
    /// <param name="value">Output typed List.</param>
    /// <returns>A new List PooledObject.</returns>
    public static ObjectPool<List<T>>.PooledObject Get(out List<T> value) => pool.Get(out value);

    /// <summary>
    /// Release an object to the pool.
    /// </summary>
    /// <param name="toRelease">List to release.</param>
    public static void Release(List<T> toRelease) => pool.Release(toRelease);
  }
}
