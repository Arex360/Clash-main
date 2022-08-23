using LightDev.Core;

namespace LightDev.UI
{
  /// <summary>
  /// Attach this script to Canvas GameObject in order to control CanvasElements.
  /// </summary>
  
  public class CanvasManager : Base
  {
    protected CanvasElement[] canvasElements;

    protected virtual void Awake()
    {
      canvasElements = GetComponentsInChildren<CanvasElement>(true);
      foreach(CanvasElement element in canvasElements)
      {
        element.Activate();
        element.Subscribe();
        element.Deactivate();
      }
    }

    protected virtual void OnDestroy()
    {
      foreach (CanvasElement element in canvasElements)
      {
        element.Unsubscribe();
      }
    }
  }
}
