using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.SystemPermissionExample
{

    public class SystemPermission
    {
        public readonly SystemProfile profile;
        private SystemUser requestor;
        private SystemAdmin admin;
        public bool IsGranted { get; set; }
        public bool IsUnixPermissionGranted { get; set; }
        public PermissionState permissionState { get; private set; }


        public static string REQUESTED = "REQUESTED";
        public static string UNIX_REQUESTED = "UNIX_REQUESTED";
        public static string CLAIMED = "CLAIMED";
        public static string UNIX_CLAIMED = "UNIX_CLAIMED";
        public static string GRANTED = "GRANTED";
        public static string DENIED = "DENIED";

        public SystemPermission(SystemUser requestor, SystemProfile profile)
        {
            this.requestor = requestor;
            this.profile = profile;
            SetState(PermissionState.REQUESTED);
            IsGranted = false;
            NotifyAdminOfPermissionRequest();
        }

        public void NotifyAdminOfPermissionRequest()
        {
            Console.WriteLine($"Usuario {nameof(requestor)} ({nameof(profile)}) solicita permissão");
        }
        public bool IsTheSameAdmin(SystemAdmin systemAdmin)
        {
            return admin.Equals(systemAdmin);
        }
        public void ClaimedBy(SystemAdmin admin)
        {
            permissionState.ClaimedBy(admin, this);
        }

        public void WillBeHandledBy(SystemAdmin admin)
        {
            this.admin = admin;
            Console.WriteLine($"Admin avalia a solicitação");
        }

        public void DeniedBy(SystemAdmin admin)
        {
            permissionState.DeniedBy(admin, this);
        }

        public void NotifyUserOfPermissionRequestResult()
        {
            Console.WriteLine($"A Permissão foi {permissionState}");
        }

        public void GrantedBy(SystemAdmin admin)
        {
            permissionState.GrantedBy(admin, this);
        }

        public void NotifyUnixAdminOfPermissionRequest()
        {
            Console.WriteLine($"Usuario solicita permissão do Admin Unix");
        }

        public void SetState(PermissionState state)
        {
            permissionState = state;
        }
    }
}
