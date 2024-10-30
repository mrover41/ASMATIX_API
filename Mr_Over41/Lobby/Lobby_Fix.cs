using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using UnityEngine;

namespace TestPlugin.Lobby
{
    internal class Lobby_Fix {
        void OnEnabled() {
            Exiled.Events.Handlers.Player.Joined += Joined;
        }
        void OnDisabled() { 
            Exiled.Events.Handlers.Player.Joined -= Joined;
        }
        void Joined(JoinedEventArgs ev) { 
            if (!Round.IsStarted) {
                ev.Player.Role.Set(RoleTypeId.Tutorial);
                ev.Player.Teleport(new Vector3(25.108f, 967.797f, -43.058f));
            }
        }
    }
}
