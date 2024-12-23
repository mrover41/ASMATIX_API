using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestPlugin.Roles.Fixed_Roles {
    public static class Scp3114Fix {
        public static void Enable() {
            Exiled.Events.Handlers.Player.Spawned += Spawn;
        }
        public static void Disable() {
            Exiled.Events.Handlers.Player.Spawned -= Spawn;
        }
        static void Spawn(SpawnedEventArgs ev) { 
            if (ev.Player.Role == RoleTypeId.Scp3114) {
                ev.Player.MaxHealth = 750;
                ev.Player.Health = 750;
            }
        }
    }
}
