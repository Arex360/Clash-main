using UnityEngine;

namespace LightDev.Core
{
  public class ApplicationManagerLoader : IAutoLoadable
  {
    static ApplicationManagerLoader()
    {
      new GameObject(typeof(ApplicationManager).Name, typeof(ApplicationManager));
    }
  }
}
