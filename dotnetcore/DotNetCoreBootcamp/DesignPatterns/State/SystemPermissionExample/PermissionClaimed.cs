namespace DesignPatterns.State.SystemPermissionExample
{
    public class PermissionClaimed : PermissionState
    {
        public PermissionClaimed()
        {
            name = "CLAIMED";
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

            if (permission.profile.IsUnixPermissionRequired &&
                permission.IsUnixPermissionGranted == false)
            {
                permission.SetState(PermissionState.UNIX_REQUESTED);
                permission.NotifyUnixAdminOfPermissionRequest();
                return;
            }
            permission.IsGranted = true;
            permission.SetState(PermissionState.GRANTED);
            permission.NotifyUserOfPermissionRequestResult();
        }
    }
}
