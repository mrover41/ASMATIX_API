using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin.commands {
     [CommandHandler(typeof(ClientCommandHandler))]
    internal class Call : ICommand {
        public string Command => "extracall";
        public string[] Aliases => new string[] { "ec" };
        public string Description => "Призвать дявола(админа)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player send = Player.Get(sender);
            Player admin = Player.List.Where(x => x.Role.Type == RoleTypeId.Overwatch && x.RemoteAdminPermissions == PlayerPermissions.Overwatch).GetRandomValue();
            if (send == null) {
                response = "Немає адмінів";
                return false;
            }
            admin.Role.Set(RoleTypeId.Tutorial);
            admin.EnableEffect(EffectType.Invisible);
            admin.Position = send.Position;
            admin.Broadcast(5, "Этот игрок призвал вас");
            response = "Вы призали дявола(админа), теперь он следит за вами";
            return true;
        }
    }
}
