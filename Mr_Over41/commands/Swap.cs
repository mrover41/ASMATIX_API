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
using Exiled.API.Extensions;
using Utils.NonAllocLINQ;
using MapEditorReborn.API.Extensions;
using TestPlugin.Roles;

namespace TestPlugin
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Swap : ICommand {
        public string Command => "CustomSwap";
        public string[] Aliases => new string[] { "CS" };
        public string Description => "Дозволяє змінити собі роль";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player player = Player.Get(sender);
            response = L.Logi(player, arguments.ToList());
            return true;
        }
    }
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Admin_Swap : ICommand {
        public string Command => "CustomSwap";
        public string[] Aliases => new string[] { "CS" };
        public string Description => "Дозволяє змінити собі роль";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player player = Player.Get(sender);
            response = L.Logi(player, arguments.ToList(), true);
            return true;
        }
    }
    public static class L { 
        public static string Logi(Player send, List<string> arguments ,bool isAdmin = false) {
            string response;
            if (send.Team != Team.SCPs && !isAdmin) {
                response = "Ви не SCP";
                return response;
            } if (arguments.Count < 1) {
                response = "Такого SCP не існує";
                return response;
            } if (Round.Duration.Seconds < 30 || isAdmin) {
                switch (arguments.First()) {
                    case "035":
                        if (!Global.Player_Role.ContainsKey("035")) {
                            send.GameObject.AddComponent<SCP035>();
                        } else {
                            response = "035 уже есть";
                            return response;
                        }
                        break;
                    case "689":
                        if (!Global.Player_Role.ContainsKey("689")) {
                                send.GameObject.AddComponent<SCP689>();
                        } else {
                            response = "035 уже есть";
                            return response;
                        }
                        break;
                }
            } else {
                response = "Время вышло";
                return response;
            }
            response = "Done";
            return response;
        }
    }
}
