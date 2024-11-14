using CommandSystem;
using PluginAPI.Core;
using System;
using UnityEngine;

namespace TestPlugin.commands {
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Hud_Controll : ICommand {
        public string Command => "Hud";
        public string[] Aliases => new string[] { "Hd" };
        public string Description => "Дом";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player send = Player.Get(sender);
            Human_HUD human_HUD = send.GameObject.GetComponent<Human_HUD>();
            SCP_HUD scp_HUD = send.GameObject.GetComponent<SCP_HUD>();
            if (human_HUD != null) {
                MonoBehaviour.Destroy(human_HUD);
                response = "Done";
                return true;
            } if (scp_HUD != null) {
                MonoBehaviour.Destroy(scp_HUD);
                response = "Done";
                return true;
            } if (send.IsHuman) { 
                send.GameObject.AddComponent<Human_HUD>();
                response = "Done";
                return true;
            } if (send.IsSCP) {
                send.GameObject.AddComponent<SCP_HUD>();
                send.GameObject.AddComponent<Human_HUD>();
                response = "Done";
                return true;
            }
            response = "Error";
            return false;
        }
    }
}
