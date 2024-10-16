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
            Oboron_Log._OnEnabled();
            foreach (Player player in Player.List) {
                Timing.RunCoroutine(Ef());
                if (player.Role == RoleTypeId.Scientist) {
                    player.Broadcast(10, "<b><#FCBA62>Ви - захисники, ваше завдання захистити вежу від загарбників і самим вижити (У вас тільки 1 життя) </color></b></b>.");
                    player.MaxHealth = 250;
                    player.Health = 250;
                    player.ClearInventory();
                    player.EnableEffect(EffectType.DamageReduction, 75);
                    player.AddItem(ItemType.SCP500);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.GunFRMG0);
                    player.AddItem(ItemType.Adrenaline);
                    player.AddItem(ItemType.GrenadeFlash);
                    player.AddItem(ItemType.ArmorHeavy);
                    player.AddItem(ItemType.Ammo556x45, 30);
                    switch (API.random.Next(0, 3)) {
                        case 0:
                            player.Teleport(new Vector3(261.907f, 1018.754f, -124.904f));
                            break;
                        case 1:
                            player.Teleport(new Vector3(252.303f, 1022.619f, -130.295f));
                            break;
                        case 2:
                            player.Teleport(new Vector3(253.599f, 1028.469f, -128.545f));
                            break;
                    }
                } else if (player.Role == RoleTypeId.ClassD) {
                    Global.Player_Oboron.Add(player, 3);
                    player.Broadcast(10, "<b><#FC6962> Ви - Загарбники, ваше завдання захопити вежу захисників і самим вижити ( У вас тільки 3 життя ) </color></b>");
                    player.ClearInventory();
                    player.AddItem(ItemType.GunE11SR);
                    player.AddItem(ItemType.ArmorCombat);
                    player.AddItem(ItemType.Medkit);
                    player.AddItem(ItemType.Ammo556x45, 17);
                    player.Teleport(new Vector3(204.526f, 1019, -128) + new Vector3(API.random.Next(0, 5), 0, API.random.Next(-5, 0)));
                    if (API.random.Next(0, 2) == 0) { 
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
                p.DisableEffect(EffectType.Ensnared);
                p.Broadcast(3, "Гра почалася");
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
            Oboron_Log._OnDisabled();
            Global.Stop_ob?.Invoke();
            Global.Player_Oboron.Clear();
            response = "Done";
            return true;
        }
    }

    class Oboron_Log {
        public static void _OnEnabled() {
            Exiled.Events.Handlers.Player.Died += Die;
            Global.Stop_ob += Stop;
            Global.Run_ob += Ru;
        }
        public static void _OnDisabled() {
            Exiled.Events.Handlers.Player.Died -= Die;
            Global.Stop_ob -= Stop;
            Global.Run_ob -= Ru;
        }
        static bool isRun = false;
        System.Random random = new System.Random();
        static void Die(DiedEventArgs ev) {
            Timing.CallDelayed(4, () => {
                if (isRun) {
                    if (Global.Player_Oboron.ContainsKey(ev.Player)) {
                        if (Global.Player_Oboron[ev.Player] > 0) {
                            ev.Player.Role.Set(RoleTypeId.ClassD);
                            ev.Player.Teleport(new Vector3(204.526f, 1019, -128));
                            ev.Player.ClearInventory();
                            Giving_Item(ev.Player);
                            Info_Output(ev.Player);
                            if (API.random.Next(0, 2) == 0)
                            {
                                ev.Player.AddItem(ItemType.GrenadeHE);
                            }
                        } else {
                            Log.Info($"игрок {ev.Player.Nickname} заспавнен за {ev.Player.Role}");
                        }
                        Global.Player_Oboron[ev.Player]--;
                        Log.Info($"Игрокк {ev.Player.Nickname} может возврадится ещё {Global.Player_Oboron[ev.Player]} раз");
                    }
                }
            });
        }

        static void Info_Output(Player player) {
            //вывод количества жизней
            if (Global.Player_Oboron[player] == 0) {
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast($"Ви маєте останне життя");
                player.Broadcast(b);
            } else {
                Exiled.API.Features.Broadcast b = new Exiled.API.Features.Broadcast($"Ви маєте {Global.Player_Oboron[player] + 1} життя");
                player.Broadcast(b);
            }
        }

        static void Giving_Item(Player player) {
            player.AddItem(ItemType.GunE11SR);
            player.AddItem(ItemType.ArmorCombat);
            player.AddItem(ItemType.Medkit);
            player.AddItem(ItemType.Ammo556x45, 17);
        }

        static void Ru() {
            isRun = true;
        }

        static void Stop() {
            isRun = false;
        }


    }

}

