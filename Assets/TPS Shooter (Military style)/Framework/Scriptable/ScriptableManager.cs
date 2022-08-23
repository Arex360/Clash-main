using System;
using System.Collections.Generic;

using LightDev.Core;

using UnityEngine;

namespace LightDev.Scriptable
{
  /// <summary>
  /// Load all ScriptableObjects that placed in Resources folder and derived from AutoLoadableScriptable.
  /// </summary>
  public class ScriptableManager : IAutoLoadable
  {
    private static readonly Dictionary<Type, AutoLoadableScriptable> scriptables = new Dictionary<Type, AutoLoadableScriptable>();

    static ScriptableManager()
    {
      foreach (AutoLoadableScriptable settings in Resources.LoadAll<AutoLoadableScriptable>(""))
      {
        scriptables.Add(settings.GetType(), settings);
      }
    }

    /// <summary>
    /// Returns ScriptableObject that derived from AutoLoadableScriptable and placed in Resources folder.
    /// </summary>
    public static T GetScriptableObject<T>() where T : AutoLoadableScriptable
    {
      return scriptables[typeof(T)] as T;
    }
  }
}
