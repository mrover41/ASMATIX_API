using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPlugin.Fans.Offococoe;
using UnityEngine;

namespace TestPlugin {
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class OffEventCommand {
        public string Command => "Weeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee";
        public string[] Aliases => new string[] { "we1" };
        public string Description => "Ивент где стая гусей хуярит дКласс";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            GameObject obj = new GameObject("Event_Controller");
            obj.name = "Event_Controller";
            GameObject gameObject = GameObject.Instantiate(obj);
            gameObject.AddComponent<OffEvent>();
            response = "Done";
            return true;
        }
    }
}
