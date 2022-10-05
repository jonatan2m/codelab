namespace DesignPatterns.State.SystemPermissionExample
{
    public class UnixPermissionRequested : PermissionState
    {
        public UnixPermissionRequested()
        {
            name = "UNIX_REQUESTED";
        }

        public override void ClaimedBy(SystemAdmin admin, SystemPermission permission)
        {
            permission.WillBeHandledBy(admin);
            permission.SetState(PermissionState.UNIX_CLAIMED);
        }
    }
}
