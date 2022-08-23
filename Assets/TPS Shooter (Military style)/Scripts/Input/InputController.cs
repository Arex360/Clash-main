namespace TPSShooter
{
    public static class InputController
    {
        public static float VerticalMovement { get; set; }
        public static float HorizontalMovement { get; set; }
        public static float VerticalRotation { get; set; }
        public static float HorizontalRotation { get; set; }

        public static bool IsRun { get; set; }
        
        public static bool IsBrakePressed { get; set; }
    }
}
