using System.Collections.Generic;

namespace Unbegames.Services {
  /// <summary>
  /// Dictionary Pool.
  /// </summary>
  /// <typeparam name="TKey">Key type.</typeparam>
  /// <typeparam name="TValue">Value type.</typeparam>
  public static class DictionaryPool<TKey, TValue> {
    // Object pool to avoid allocations.
    static readonly ObjectPool<Dictionary<TKey, TValue>> pool
        = new ObjectPool<Dictionary<TKey, TValue>>(null, d => d.Clear());

    /// <summary>
    /// Get a new Dictionary
    /// </summary>
    /// <returns>A new Dictionary</returns>
    public static Dictionary<TKey, TValue> Get() => pool.Get();

    /// <summary>
    /// Get a new dictionary PooledObject.
    /// </summary>
    /// <param name="value">Output typed Dictionary.</param>
    /// <returns>A new Dictionary PooledObject.</returns>
    public static ObjectPool<Dictionary<TKey, TValue>>.PooledObject Get(out Dictionary<TKey, TValue> value)
        => pool.Get(out value);

    /// <summary>
    /// Release the dictionary to the pool.
    /// </summary>
    /// <param name="toRelease">Dictionary to release.</param>
    public static void Release(Dictionary<TKey, TValue> toRelease) => pool.Release(toRelease);
  }
}
