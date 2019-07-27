namespace DesignPatterns.State.SystemPermissionExample
{
    public class PermissionRequested : PermissionState
    {
        public PermissionRequested()
        {
            name = "REQUESTED";
        }

        public override void ClaimedBy(SystemAdmin admin, SystemPermission permission)
        {            
            permission.WillBeHandledBy(admin);
            permission.SetState(PermissionState.CLAIMED);          
        }
    }
}
