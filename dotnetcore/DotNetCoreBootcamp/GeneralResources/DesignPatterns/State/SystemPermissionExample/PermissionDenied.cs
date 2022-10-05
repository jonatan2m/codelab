namespace DesignPatterns.State.SystemPermissionExample
{
    public class PermissionDenied : PermissionState
    {
        public PermissionDenied()
        {
            name = "DENIED";
        }
    }
}
