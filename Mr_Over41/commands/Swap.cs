using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin.Roles;

namespace TestPlugin {
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
        public static string Logi(Player send, List<string> arguments, bool isAdmin = false) {
            string response;
            if (send.Team != Team.SCPs && !isAdmin) {
                response = "Ви не SCP";
                return response;
            } if (arguments.Count < 1) {
                response = "Такого SCP не існує";
                return response;
            } if (Round.Duration.TotalSeconds < 30 && !isAdmin) {
                switch (arguments.First()) {
                    case "035":
                        if (!Global.Player_Role.ContainsKey("035")) {
                            send.GameObject.AddComponent<SCP035>();
                        } else {
                            response = "035 вже есть";
                            return response;
                        }
                        break;
                }
            } if (isAdmin) { 
                switch (arguments.First()) {
                    case "035":
                        if (!Global.Player_Role.ContainsKey("035")) {
                            send.GameObject.AddComponent<SCP035>();
                        } else {
                            response = "035 вже есть";
                            return response;
                        }
                        break;
                    case "689":
                        if (!Global.Player_Role.ContainsKey("689")) {
                            send.GameObject.AddComponent<SCP689>();
                        } else {
                            response = "689 вже есть";
                            return response;
                        }
                        break;
                    case "080":
                        if (!Global.Player_Role.ContainsKey("080")) {
                            send.GameObject.AddComponent<SCP080>();
                        } else {
                            response = "080 вже есть";
                            return response;
                        }
                        break;
                }
            } else {
                response = "Час вийшов";
                return response;
            }
            response = "Done";
            return response;
        }
    }
}
