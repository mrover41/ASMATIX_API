using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.Permissions.Extensions;
using MEC;

namespace TestPlugin.commands {
     [CommandHandler(typeof(ClientCommandHandler))]
    internal class Call : ICommand {
        public string Command => "extracall";
        public string[] Aliases => new string[] { "ec" };
        public string Description => "Призвать дявола(админа)";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player send = Player.Get(sender);
            Player admin = Player.List.Where(x => x.Role.Type == RoleTypeId.Overwatch && x.CheckPermission("Ex_Call")).GetRandomValue();
            if (admin == null) {
                response = "Вибачаємося, але поки що Адміністрація не спостерігає, тому залиште свою скаргу через SCP:SL натиснувши на англіську кнопку \"N\" і очікуйте на Адміністрацію! З повагою ©ASMATIX SCP:SL UKR.";
                return false;
            } if (send.Role == RoleTypeId.Spectator || send.Role.Type == RoleTypeId.Overwatch) {
                response = "Ви не можете використовувати цю команду";
                return false;
            }
            admin.Broadcast(5, $"<b><color=#E6E8FC>Вас покликав гравець</color> <color=#4B88FE>{send.Nickname} (ID: {send.Id})</color><color=#E6E8FC>, для того щоб</color> <color=#FF9550>ви прослідкували</color> <color=#E6E8FC>за гравцями в цьому місці</color></b>");
            admin.Role.Set(RoleTypeId.Tutorial);
            admin.EnableEffect(EffectType.Invisible);
            Timing.CallDelayed(1, () => admin.Position = send.Position);
            admin.Broadcast(5, "Этот игрок призвал вас");
            response = "Адміністратор вже слідкує за вами! Дякуємо за виклик! Просимо невзловживати цією командую просто так, в іншому випадку вам може видатися бан на 3 дні, а в найгіршому на місяць! З повагою ©ASMATIX SCP:SL UKR.";
            return true;
        }
    }
}
