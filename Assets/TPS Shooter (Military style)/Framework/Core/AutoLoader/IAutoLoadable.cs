namespace LightDev.Core
{
  /// <summary>
  /// Marks object where static constructor automatically called before Unity scene loaded.
  /// 
  /// Note: do not create object that impelements IAutoLoadable by yourself. Everything done by AutoLoader.
  ///
  /// Remember: static constuctor called only once in whole Game.
  /// </summary>
  public interface IAutoLoadable
  {
  }
}
