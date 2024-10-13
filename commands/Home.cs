using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using UnityEngine;

namespace TestPlugin.commands {
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Home : ICommand {
        public string Command => "Home";
        public string[] Aliases => new string[] { "H" };
        public string Description => "Дом";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player send = Player.Get(sender);
            if (send.Role.Type == RoleTypeId.Spectator) {
                send.Role.Set(RoleTypeId.Tutorial);
                send.Teleport(new Vector3(-122.890f, 1095.255f, 115.723f));
                response = "Done";
                return true;
            } else if (send.Role.Type == RoleTypeId.Tutorial) { 
                send.Role.Set(RoleTypeId.Spectator);
                response = "Done";
                return true;
            }
            response = "Егок";
            return false;
        }
    }
}
