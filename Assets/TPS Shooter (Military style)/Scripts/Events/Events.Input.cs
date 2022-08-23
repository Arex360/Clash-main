namespace LightDev
{
  public partial class Events
  {
    public static Event JumpRequested;
    public static Event CrouchRequested;

    public static Event FireRequested;
    public static Event ReloadRequested;
    public static Event AimActivateRequested;

    public static Event WeaponChooseStartRequest;
    public static Event WeaponChooseFinishRequest;

    public static Event<int> SwapWeaponRequested;
    public static Event<int> DropWeaponRequested;

    public static Event UseVehicleRequested;

    public static Event GrenadeStartThrowRequest;
    public static Event GrenadeFinishThrowRequest;

    public static Event GamePauseRequested;
    public static Event GameResumeRequested;
    public static Event GameReplayRequested;
    public static Event GameLoadHomeSceneRequested;
  }
}
