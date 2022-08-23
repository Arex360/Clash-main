using LightDev.Core;

namespace LightDev.Pool
{
  /// <summary>
  /// Object that can be used by PoolsManager.
  /// If you want to use it by PoolsManager, place prefab to Recources folder.
  ///
  /// Warning: Awake() and Start() methods will be called only once, as object Instantiated only once.
  /// Instead, use OnRetrieved() method.
  /// </summary>
  public abstract class PoolableElement : Base
  {
    /// <summary>
    /// Called when the object is taken from pool.
    /// Used for subscribing to Events in the game.
    /// Warning: do not forget to Unsubcribe.
    /// </summary>
    public virtual void Subscribe()
    {
    }

    /// <summary>
    /// Called when the object returned to pool.
    /// Used for unsubscribing from Events in the game.
    /// </summary>
    public virtual void Unsubscribe()
    {
    }

    /// <summary>
    /// Called when the object is taken from pool.
    /// </summary>
    public virtual void OnRetrieved()
    {
    }

    /// <summary>
    /// Called when the object returned to pool.
    /// </summary>
    public virtual void OnReturned()
    {
    }
  }
}
