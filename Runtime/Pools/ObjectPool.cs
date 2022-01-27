using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unbegames.Services.Pool {
  /// <summary>                              
  /// Generic object pool.
  /// </summary>
  /// <typeparam name="T">Type of the object pool.</typeparam>
  public class ObjectPool<T> where T : new() {
    private readonly Stack<T> stack = new Stack<T>();
    private readonly Action<T> onGet;
    private readonly Action<T> onRelease;
    private readonly bool collectionCheck = true;

    /// <summary>
    /// Number of objects in the pool.
    /// </summary>
    public int countAll { get; private set; }
    /// <summary>
    /// Number of active objects in the pool.
    /// </summary>
    public int countActive { get { return countAll - countInactive; } }
    /// <summary>
    /// Number of inactive objects in the pool.
    /// </summary>
    public int countInactive { get { return stack.Count; } }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="onGet">Action on get.</param>
    /// <param name="onRelease">Action on release.</param>
    /// <param name="collectionCheck">True if collection integrity should be checked.</param>
    public ObjectPool(Action<T> onGet, Action<T> onRelease, bool collectionCheck = true) {
      this.onGet = onGet;
      this.onRelease = onRelease;
      this.collectionCheck = collectionCheck;
    }

    /// <summary>
    /// Get an object from the pool.
    /// </summary>
    /// <returns>A new object from the pool.</returns>
    public T Get() {
      T element;
      if (stack.Count == 0) {
        element = new T();
        countAll++;
      } else {
        element = stack.Pop();
      }
      onGet?.Invoke(element);
      return element;
    }

    /// <summary>
    /// Pooled object.
    /// </summary>
    public struct PooledObject : IDisposable {
      readonly T m_ToReturn;
      readonly ObjectPool<T> m_Pool;

      internal PooledObject(T value, ObjectPool<T> pool) {
        m_ToReturn = value;
        m_Pool = pool;
      }

      /// <summary>
      /// Disposable pattern implementation.
      /// </summary>
      void IDisposable.Dispose() => m_Pool.Release(m_ToReturn);
    }

    /// <summary>
    /// Get et new PooledObject.
    /// </summary>
    /// <param name="v">Output new typed object.</param>
    /// <returns>New PooledObject</returns>
    public PooledObject Get(out T v) => new PooledObject(v = Get(), this);

    /// <summary>
    /// Release an object to the pool.
    /// </summary>
    /// <param name="element">Object to release.</param>
    public void Release(T element) {
#if UNITY_EDITOR // keep heavy checks in editor
      if (collectionCheck && stack.Count > 0) {
        if (stack.Contains(element))
          Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
      }
#endif
      onRelease?.Invoke(element);
      stack.Push(element);
    }
  }
}
