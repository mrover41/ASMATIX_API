using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;

namespace TestPlugin.GoodMode {
    public static class GMode {
        public static void _ChaingRole(ChangingRoleEventArgs ev) { 
            if (ev.NewRole == RoleTypeId.Tutorial) { 
                ev.Player.IsGodModeEnabled = true;
            } else if (ev.Player != null){
                ev.Player.IsGodModeEnabled = false;
            }
        }
    }
}
