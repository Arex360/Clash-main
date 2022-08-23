namespace TPSShooter
{
  public partial class TPSCamera
  {
    private class NoMovementState : CameraState
    {
      public NoMovementState(TPSCamera tpsCamera) : base(tpsCamera)
      {
      }
    }
  }
}
