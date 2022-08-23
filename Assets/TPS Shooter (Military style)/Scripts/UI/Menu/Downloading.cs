using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

using LightDev;
using LightDev.UI;

namespace TPSShooter.UI.Menu
{
  public class Downloading : CanvasElement
  {
    [Header("References")]
    public Slider progressBar;
    public Text progressText;

    public override void Subscribe()
    {
      Events.RequestMenuDownloading += OnRequestMenuDownloading;
    }

    public override void Unsubscribe()
    {
      Events.RequestMenuDownloading -= OnRequestMenuDownloading;
    }

    private void OnRequestMenuDownloading(int sceneIndex)
    {
      Show();
      StartCoroutine(AnimateProgressBar());
      StartCoroutine(LoadScene(sceneIndex));
    }

    private IEnumerator AnimateProgressBar()
    {
      const float fullTime = 1;
      float currentTime = 0;

      while (currentTime < fullTime)
      {
        int progress = (int)(currentTime / fullTime * 100);
        progressText.text = $"{progress}%";
        progressBar.value = progress;
        currentTime += Time.deltaTime;
        yield return null;
      }
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
      yield return new WaitForSeconds(0.1f);

      AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
      operation.allowSceneActivation = false;

      while (!operation.isDone)
      {
        if (operation.progress >= 0.9f)
        {
          operation.allowSceneActivation = true;
        }
        yield return null;
      }
    }
  }
}
