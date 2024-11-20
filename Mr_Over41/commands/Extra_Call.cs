using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.Permissions.Extensions;

namespace TestPlugin.commands {
     [CommandHandler(typeof(ClientCommandHandler))]
    internal class Call : ICommand {
        public string Command => "extracall";
        public string[] Aliases => new string[] { "ec" };
        public string Description => "Призвать дявола(админа)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player send = Player.Get(sender);
            Player admin = Player.List.Where(x => x.Role.Type == RoleTypeId.Overwatch && x.CheckPermission("Ex_Call")).GetRandomValue();
            foreach (Player ad in Player.List.Where(x => x.AdminChatAccess)) { 
                ad.Broadcast(5, $"Вас покликав гравець {send.Nickname}, для того щоб ви прослідкували за гравцями в цьому місці, ID: {send.Id}");
            }
            if (admin == null) {
                response = "Вибачаємося,  але поки що Адміністрації на цьому сервері немає, тому залиште свою скаргу через SCP:SL натиснувши на англіську кнопку 'N' і чекайте на Адміністрацію!";
                return false;
            }
            admin.Role.Set(RoleTypeId.Tutorial);
            admin.EnableEffect(EffectType.Invisible);
            admin.Position = send.Position;
            admin.Broadcast(5, "Этот игрок призвал вас");
            response = "Адміністратор вже слідкує за вами! Дякуємо за виклик!";
            return true;
        }
    }
}
