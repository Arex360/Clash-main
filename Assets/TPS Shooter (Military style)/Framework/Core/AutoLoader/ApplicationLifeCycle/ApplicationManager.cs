using UnityEngine;
using UnityEngine.SceneManagement;

namespace LightDev.Core
{
  public sealed class ApplicationManager : MonoBehaviour
  {
    private bool isNeedToChangeTimeScale = false;

    private void Awake()
    {
      DontDestroyOnLoad(this);

      SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      Events.SceneLoaded.Call();
    }

    private void OnApplicationFocus(bool focus)
    {
      if (focus)
      {
        if (isNeedToChangeTimeScale)
          Time.timeScale = 1;
        Events.ApplicationResumed.Call();
      }
      else
      {
        if (isNeedToChangeTimeScale)
          Time.timeScale = 0;
        Events.ApplicationPaused.Call();
      }
    }

    private void OnApplicationPause(bool pause)
    {
      if (pause)
      {
        if (isNeedToChangeTimeScale)
          Time.timeScale = 0;
        Events.ApplicationPaused.Call();
      }
      else
      {
        if (isNeedToChangeTimeScale)
          Time.timeScale = 1;
        Events.ApplicationResumed.Call();
      }
    }
  }
}
