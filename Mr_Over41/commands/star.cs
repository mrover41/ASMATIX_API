using CommandSystem;
using Cryptography;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestPlugin {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class command : ICommand {
        public string Command => "star";
        public string[] Aliases => new string[] { "star" };
        public string Description => "Ивент";

        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            //List<Player> players = new List<Player>(Player.List);
            int arg1 = 0;
            //arg1 = int.Parse(arguments.Array[0].ToString());
            arg1 = Player.List.Count / 3;
            int count = 0;
            foreach (Player p in Player.List) {
                count++;
                if (count <= arg1) {
                    p.RoleManager.ServerSetRole(RoleTypeId.Scp173, RoleChangeReason.RemoteAdmin);
                    p.Broadcast(10, "вы SCP173");
                    Log.Info($"Назначена роль SCP173 игроку: {p.Nickname}");
                    p.Teleport(new Vector3(51.8f, -921.6f, 60f));
                } else {
                    p.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin);
                    p.Broadcast(10, "вы Био мусор");
                    Log.Info($"Назначена роль ClassD игроку: {p.Nickname}");
                    p.Teleport(new Vector3(1f, -921.6f, 110.3f));
                }
            }
            Timing.RunCoroutine(Ef());

            Log.Info(arg1);
            response = "Done";
            return true;
        }
        private IEnumerator<float> Ef()
        {
            foreach (Player p in Player.List)
            {
                if (p.Role == RoleTypeId.Scp173)
                {
                    p.EnableEffect(EffectType.Ensnared);
                    p.EnableEffect(EffectType.FogControl, 200);
                    yield return Timing.WaitForSeconds(30);
                } else if (p.Role == RoleTypeId.ClassD)
                {
                    p.AddItem(ItemType.Lantern);
                    p.EnableEffect(EffectType.Burned, 50);
                    p.EnableEffect(EffectType.Deafened, 50);
                    p.EnableEffect(EffectType.MovementBoost, 10);
                }
            }
        }   
    }


}