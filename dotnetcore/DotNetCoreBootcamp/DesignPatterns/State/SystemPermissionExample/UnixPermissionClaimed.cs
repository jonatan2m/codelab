namespace DesignPatterns.State.SystemPermissionExample
{
    public class UnixPermissionClaimed : PermissionState
    {
        public UnixPermissionClaimed()
        {
            name = "UNIX_CLAIMED";
        }

        public override void DeniedBy(SystemAdmin admin, SystemPermission permission)
        {
            if (permission.IsTheSameAdmin(admin) == false) return;

            permission.IsGranted = false;
            permission.IsUnixPermissionGranted = false;
            permission.SetState(PermissionState.DENIED);
            permission.NotifyUserOfPermissionRequestResult();
        }

        public override void GrantedBy(SystemAdmin admin, SystemPermission permission)
        {
            if (permission.IsTheSameAdmin(admin) == false) return;

            permission.IsUnixPermissionGranted = true;
            permission.IsGranted = true;
            permission.SetState(PermissionState.GRANTED);
            permission.NotifyUserOfPermissionRequestResult();

        }
    }
}
