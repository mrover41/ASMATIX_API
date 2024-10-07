using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestPlugin {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Run_I : ICommand {
        public string Command => "runI";
        public string[] Aliases => new string[] { "run" };
        public string Description => "Ивент";
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            System.Random random = new System.Random();
            foreach (Player player in Player.List) {
                Global.Player_Oboron.Add(player, 2);
                Timing.RunCoroutine(Ef());
                if (player.Role == RoleTypeId.Scientist) {
                    player.Health = 200;
                    player.ClearInventory();
                    player.AddItem(ItemType.SCP500);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.GunFRMG0);
                    player.AddItem(ItemType.Adrenaline);
                    player.AddItem(ItemType.GrenadeFlash);
                    player.AddItem(ItemType.ArmorHeavy);
                    player.AddItem(ItemType.Ammo556x45, 30);
                    player.Teleport(RoomType.Surface);
                } else if (player.Role == RoleTypeId.ClassD) {
                    player.ClearInventory();
                    player.AddItem(ItemType.GunE11SR);
                    player.AddItem(ItemType.ArmorCombat);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.Ammo556x45, 17);
                    player.Teleport(RoomType.Surface);
                    if (random.Next(0, 2) == 0) { 
                        player.AddItem(ItemType.GrenadeHE);
                    }
                }
            }
            Global.Run_ob?.Invoke();
            response = "Done";
            return true;
        }
        private IEnumerator<float> Ef() {
            foreach (Player p in Player.List) {
                p.EnableEffect(EffectType.Ensnared);
            }
            yield return Timing.WaitForSeconds(60);
            foreach (Player p in Player.List) {
                p.DisableAllEffects();
                p.Broadcast(5, "Гра почалася");
            }
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class _Stop : ICommand
    {
        public string Command => "stop";
        public string[] Aliases => new string[] { "stop" };
        public string Description => "Ивент";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Global.Stop_ob?.Invoke();
            Global.Player_Oboron.Clear();
            response = "Done";
            return true;
        }
    }

    class Oboron_Log {
        public void OnEnabled() {
            Exiled.Events.Handlers.Player.Died += Die;
            Global.Stop_ob += Stop;
            Global.Run_ob += Ru;
        }
        public void OnDisabled() {
            Exiled.Events.Handlers.Player.Died -= Die;
            Global.Stop_ob -= Stop;
            Global.Run_ob -= Ru;
        }
        bool isRun = false;
        System.Random random = new System.Random();
        void Die(DiedEventArgs ev) {
            if (isRun) {
                if (Global.Player_Oboron[ev.Player] > 0)
                {
                    if (ev.TargetOldRole == RoleTypeId.ClassD)
                    {
                        ev.Player.RoleManager.ServerSetRole(PlayerRoles.RoleTypeId.ClassD, RoleChangeReason.None);
                        ev.Player.Teleport(new Vector3(204.526f, 1019, -128));
                        ev.Player.ClearInventory();
                        Giving_Item(ev.Player);
                        Info_Output(ev.Player);
                        if (random.Next(0, 2) == 0)
                        {
                            ev.Player.AddItem(ItemType.GrenadeHE);
                        }
                    }
                    else
                    {
                        Log.Info($"игрок {ev.Player.Nickname} заспавнен за {ev.Player.Role}");
                    }
                    Global.Player_Oboron[ev.Player]--;
                    Log.Info($"Игрокк {ev.Player.Nickname} может возврадится ещё {Global.Player_Oboron[ev.Player]} раз");
                }
            }
        }

        void Info_Output(Player player) {
            //вывод количества жизней
            if (Global.Player_Oboron[player] == 0) {
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast($"Ви маєте останне життя");
                player.Broadcast(b);
            } else {
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast($"Ви маєте {Global.Player_Oboron[player] + 1} життя");
                player.Broadcast(b);
            }
        }

        void Giving_Item(Player player) {
            player.AddItem(ItemType.GunE11SR);
            player.AddItem(ItemType.ArmorCombat);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.Ammo556x45, 17);
        }

        void Ru() {
            isRun = true;
        }

        void Stop() {
            isRun = false;
        }


    }

}

