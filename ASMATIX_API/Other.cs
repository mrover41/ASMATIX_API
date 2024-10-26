using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestPlugin
{
    public static class Global
    {
        //Action
        public static Action Run_ob;
        public static Action Stop_ob;
        //bpr
        public static bool d = false;
        public static bool f = true;
        public static bool SCP035 = true;
        //Information
        public static Dictionary<Exiled.API.Features.Items.Item, int> it = new Dictionary<Exiled.API.Features.Items.Item, int>();
        public static Dictionary<Player, int> Player_Oboron = new Dictionary<Player, int>();
        public static Dictionary<string, Player> Player_Role = new Dictionary<string, Player>();
    }
    public static class Spawn_System
    {
        public static void Spawn(Player player, uint ID)
        {
            CustomRole.Get(ID).AddRole(player);
        }
        public static void RoundSt() {
            //SPAWN
            //343
            if (Exiled.API.Features.Player.List.Count() >= 8) {
                if (API.random.Next(0, 100) < 1) {
                    Spawn_System.Spawn(Exiled.API.Features.Player.List.Where(x => x.Role.Type == RoleTypeId.ClassD)?.ToList().RandomItem(), 343);
                }
            }
            //035
            if (Exiled.API.Features.Player.List.Count() >= 8) {
                if (API.random.Next(0, 100) < 50) {
                    Global.SCP035 = false;
                    Exiled.API.Features.Player pl = Exiled.API.Features.Player.List.GetRandomValue();
                    Global.Player_Role.Add("035", pl);
                    //Spawn_System.Spawn(Exiled.API.Features.Player.List.Where(x => x.IsScp)?.ToList().RandomItem(), 35);
                    pl.GameObject.AddComponent<SCP035>();
                }
            }
        }
    }
}
