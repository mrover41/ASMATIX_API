using CommandSystem;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utf8Json.Internal.DoubleConversion;
using Exiled.CustomRoles.API.Features;

namespace TestPlugin.Configs
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Swap : ICommand {
        public string Command => "CustomSwap";
        public string[] Aliases => new string[] { "CS" };
        public string Description => "Дозволяє змінити собі роль";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player send = Player.Get(sender);
            if (send.Team != Team.SCPs) {
                response = "Ви не SCP";
                return false;
            } if (arguments.Count < 1) {
                response = "Такого SCP не існує";
                return false;
            }
            if (DateTime.Now.Second - API.RoundTime < 30) {
                switch (arguments.First()) {
                    case "035":
                        if (Global.SCP035) {
                            CustomRole.Get((uint)35).AddRole(send);
                            Global.SCP035 = false;
                        } else {
                            response = "035 уже есть";
                            return true;
                        }
                        break;
                }
            } else {
                response = "Время вышло";
                return true;
            }
            response = "Done";
            return true;
        }
    }
}
