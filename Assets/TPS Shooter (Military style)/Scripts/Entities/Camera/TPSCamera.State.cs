namespace TPSShooter
{
  public partial class TPSCamera
  {
    private abstract class CameraState
    {
      protected TPSCamera tpsCamera;

      public CameraState(TPSCamera tpsCamera)
      {
        this.tpsCamera = tpsCamera;
      }

      public virtual void OnEnter()
      {
      }

      public virtual void OnExit()
      {
      }

      public virtual void OnUpdate()
      {
      }
    }
  }
}
