using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.SystemPermissionExample
{
    /// <summary>
    /// Lógica de transição de estados simples demais pra justificar utilizar o State.
    /// Mas caso mais comportamentos sejam adicionados a essa classe para gerenciar o estado
    /// vai valer a pena fazer esse tratamento.
    /// </summary>
    public class SystemPermissionNoNeedState
    {
        private SystemProfile profile;
        private SystemUser requestor;
        private SystemAdmin admin;
        public bool IsGranted { get; private set; }
        public string State { get; private set; }

        public static string REQUESTED = "REQUESTED";
        public static string CLAIMED = "CLAIMED";
        public static string GRANTED = "GRANTED";
        public static string DENIED = "DENIED";

        public SystemPermissionNoNeedState(SystemUser requestor, SystemProfile profile)
        {
            this.requestor = requestor;
            this.profile = profile;
            State = REQUESTED;
            IsGranted = false;
            NotifyAdminOfPermissionRequest();
        }

        private void NotifyAdminOfPermissionRequest()
        {
            Console.WriteLine($"Usuario {nameof(requestor)} ({nameof(profile)}) solicita permissão");
        }

        public void ClaimedBy(SystemAdmin admin)
        {
            if (State.Equals(REQUESTED) == false)
                return;
            WillBeHandledBy(admin);
            State = CLAIMED;
        }

        private void WillBeHandledBy(SystemAdmin admin)
        {
            this.admin = admin;
            Console.WriteLine($"Admin avalia a solicitação");
        }

        public void DeniedBy(SystemAdmin admin)
        {
            if (State.Equals(CLAIMED) == false) return;
            if (this.admin.Equals(admin) == false) return;
            IsGranted = false;
            State = DENIED;
            NotifyUserOfPermissionRequestResult();
        }

        private void NotifyUserOfPermissionRequestResult()
        {
            Console.WriteLine($"A Permissão foi {State}");
        }

        public void GrantedBy(SystemAdmin admin)
        {
            if (State.Equals(CLAIMED) == false) return;
            if (this.admin.Equals(admin) == false) return;
            IsGranted = true;
            State = GRANTED;
            NotifyUserOfPermissionRequestResult();
        }
    }
}
