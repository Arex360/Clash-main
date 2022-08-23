using System;
using System.Collections.Generic;

using UnityEngine;

using LightDev.Core;

namespace LightDev.Pool
{
  /// <summary>
  /// Works with all objects that derived from PoolableElement.
  /// These objects must be placed in Resources folder.
  /// </summary>
  public class PoolsManager : IAutoLoadable
  {
    private static readonly Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

    static PoolsManager()
    {
      CreatePools();
    }

    private static void CreatePools()
    {
      foreach (PoolableElement element in Resources.LoadAll<PoolableElement>(""))
      {
        Type type = element.GetType();
        if (!pools.ContainsKey(type)) pools.Add(type, new Pool(type));
        pools[type].AddElementPrefab(element);
      }
    }

    /// <summary>
    /// Retrieves element of type T from Pool.
    ///
    /// PreCreate called before element's GameObject become active, and Subscribe() and OnRetrieved() called.
    /// For example, PreCreate can be used for setup Position and Rotation, Trail problem.
    /// </summary>
    public static T RetrieveElement<T>(Action<T> preCreate = null) where T : PoolableElement
    {
      return pools[typeof(T)].RetrieveElement<T>(preCreate);
    }

    /// <summary>
    /// Returns element to Pool.
    /// </summary>
    public static void ReturnElement(PoolableElement element)
    {
      pools[element.GetType()].ReturnElement(element);
    }

    /// <summary>
    /// Returns all active elements of type T to Pool.
    /// </summary>
    public static void ReturnElements<T>() where T : PoolableElement
    {
      pools[typeof(T)].ReturnAllElements();
    }

    /// <summary>
    /// Get all active elements of type T.
    /// </summary>
    public static List<T> GetActiveElements<T>() where T : PoolableElement
    {
      return pools[typeof(T)].GetActiveElements<T>();
    }

    /// <summary>
    /// Destroy all elements in Pool.
    ///
    /// Can be used to free memory.
    /// </summary>
    public static void DestroyElements<T>() where T : PoolableElement
    {
      pools[typeof(T)].DestroyElements();
    }

    private class Pool
    {
      private List<PoolableElement> elementPrefabs = new List<PoolableElement>();
      private List<PoolableElement> activeElements = new List<PoolableElement>();
      private List<PoolableElement> inactiveElements = new List<PoolableElement>();

      private GameObject holder;

      public Pool(Type type)
      {
        holder = new GameObject("pool." + type.Name);
        GameObject.DontDestroyOnLoad(holder);
      }

      public void AddElementPrefab(PoolableElement element)
      {
        elementPrefabs.Add(element);
      }

      public T RetrieveElement<T>(Action<T> preCreate = null) where T : PoolableElement
      {
        PoolableElement element = (inactiveElements.Count == 0) ? Instantiate() : GetFromInactiveElements();
        RetrieveElementActions(element as T, preCreate);

        return element as T;
      }

      public void ReturnElement(PoolableElement element)
      {
        ReturnElementActions(element);
        activeElements.Remove(element);
        inactiveElements.Add(element);
      }

      public void ReturnAllElements()
      {
        foreach (PoolableElement element in activeElements)
        {
          ReturnElementActions(element);
        }

        inactiveElements.AddRange(activeElements);
        activeElements.Clear();
      }

      public List<T> GetActiveElements<T>() where T : PoolableElement
      { 
        List<T> elements = new List<T>();
        for(int i = 0; i < activeElements.Count; i++)
        {
          elements.Add(activeElements[i] as T);
        }
        return elements; 
      }

      public void DestroyElements()
      {
        foreach(PoolableElement element in activeElements)
        {
          UnityEngine.GameObject.Destroy(element.gameObject);
        }
        foreach(PoolableElement element in inactiveElements)
        {
          UnityEngine.GameObject.Destroy(element.gameObject);
        }

        activeElements.Clear();
        inactiveElements.Clear();
      }

      private PoolableElement Instantiate()
      {
        var randomPrefabIndex = UnityEngine.Random.Range(0, elementPrefabs.Count);
        var element = UnityEngine.Object.Instantiate(elementPrefabs[randomPrefabIndex]);
        MonoBehaviour.DontDestroyOnLoad(element);
        element.name = elementPrefabs[randomPrefabIndex].name;
        element.transform.SetParent(holder.transform, false);
        element.Deactivate();

        return element;
      }

      private PoolableElement GetFromInactiveElements()
      {
        var randomInactiveElementIndex = UnityEngine.Random.Range(0, inactiveElements.Count);
        var element = inactiveElements[randomInactiveElementIndex];
        inactiveElements.RemoveAt(randomInactiveElementIndex);

        return element;
      }

      private void RetrieveElementActions<T>(T element, Action<T> preCreate = null) where T : PoolableElement
      {
        preCreate?.Invoke(element);
        activeElements.Add(element);
        element.Activate();
        element.Subscribe();
        element.OnRetrieved();
      }

      private void ReturnElementActions(PoolableElement element)
      {
        element.Unsubscribe();
        element.OnReturned();
        element.Deactivate();
        element.transform.SetParent(holder.transform, false);
      }
    }
  }
}
