using System;

namespace DesignPatterns.State.SystemPermissionExample
{
    public abstract class PermissionState
    {
        protected string name;

        public static PermissionState REQUESTED = new PermissionRequested();
        public static PermissionState UNIX_REQUESTED = new UnixPermissionRequested();
        public static PermissionState CLAIMED = new PermissionClaimed();
        public static PermissionState UNIX_CLAIMED = new UnixPermissionClaimed();
        public static PermissionState GRANTED = new PermissionGranted();
        public static PermissionState DENIED = new PermissionDenied();

        public virtual void ClaimedBy(SystemAdmin admin, SystemPermission permission) { }

        public virtual void DeniedBy(SystemAdmin admin, SystemPermission permission) { }

        public virtual void GrantedBy(SystemAdmin admin, SystemPermission permission) { }
        
        public override string ToString()
        {
            return name;
        }
    }
}
