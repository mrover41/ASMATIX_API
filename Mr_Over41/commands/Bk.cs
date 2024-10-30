using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestPlugin.Configs {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class command : ICommand {
        public string Command => "Bstart";
        public string[] Aliases => new string[] { "Bstart" };
        public string Description => "Ивент Бэгрумс";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Timing.RunCoroutine(Delay(14));
            response = "Done";
            return true;
        }
        private IEnumerator<float> Delay(float delayInSeconds) {
            foreach (Player player in Player.List) {
                player.EnableEffect(EffectType.Flashed);
                player.EnableEffect(EffectType.Blinded);
                player.EnableEffect(EffectType.Invisible);
                if (player.Role != RoleTypeId.Tutorial) {
                    player.Teleport(new Vector3(20, 1037.667f, -33));
                }
                player.Broadcast(5, "<color=#000000>Темрява… нічого не бачу!</color>");
                player.Broadcast(10, "<color=#000000>Що це? Все пливе…</color>");
                player.Broadcast(15, "<color=#000000>Світло зникає... Що зі мною?</color>");
            }
            yield return Timing.WaitForSeconds(delayInSeconds);
            foreach (Player player in Player.List) {
                player.DisableAllEffects();
                player.EnableEffect(EffectType.Invisible);
            }
            yield return Timing.WaitForSeconds(delayInSeconds + 7);
            foreach (Player player in Player.List)
            {
                player.DisableAllEffects();
                player.AddItem(ItemType.Lantern);
            }
        }
    }
    
}