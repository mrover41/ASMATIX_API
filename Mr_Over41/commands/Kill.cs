using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;

namespace TestPlugin.commands {
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Kill : ICommand {
        public string Command => "suicide";
        public string[] Aliases => new string[] { "s" };
        public string Description => "F";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player send = Player.Get(sender);
            if (send.Role.Type != RoleTypeId.Tutorial && send.Role.Type != RoleTypeId.Spectator) send.Kill("Suicided");
            response = ")";
            return true;
        }
    }
}
