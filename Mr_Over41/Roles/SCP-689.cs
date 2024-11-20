using CommandSystem.Commands.Console;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.Patches;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp3114;
using Exiled.Events.Handlers;
using InventorySystem;
using MEC;
using Microsoft.Win32;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using PluginAPI.Core.Zones;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using TestPlugin;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Windows;
using VoiceChat;

namespace TestPlugin.Roles
{
    /*public class SCP689 : CustomRole {
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp3114;
        public override uint Id { get; set; } = 689;
        public override float SpawnChance { get; set; } = 0;
        public override int MaxHealth { get; set; } = 1750;
        public override string Name { get; set; } = "Дух";
        public override string Description { get; set; } =
            "SCP-689";
        public override string CustomInfo { get; set; } = "SCP-689";
        public override List<string> Inventory { get; set; } = new List<string>() {
        };
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties() {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint> {
                new RoleSpawnPoint() {
                    Role = RoleTypeId.Scientist,
                    Chance = 0,
                }
            }
        };
        static int Cd = 5;
        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            Exiled.Events.Handlers.Player.Spawned += Sp;
            Exiled.Events.Handlers.Scp3114.Slapped += _Coin;
            Exiled.Events.Handlers.Scp3114.Strangling += Tp;
            Exiled.Events.Handlers.Player.Interacted += Inter;
            Exiled.Events.Handlers.Player.InteractingDoor += Inter_D;
            Exiled.Events.Handlers.Player.Died += Die;
            Exiled.Events.Handlers.Player.Hurting += TD;
            Exiled.Events.Handlers.Player.TogglingNoClip += Alt;
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.Spawned -= Sp;
            Exiled.Events.Handlers.Scp3114.Slapped -= _Coin;
            Exiled.Events.Handlers.Scp3114.Strangling -= Tp;
            Exiled.Events.Handlers.Player.Interacted -= Inter;
            Exiled.Events.Handlers.Player.InteractingDoor -= Inter_D;
            Exiled.Events.Handlers.Player.Died -= Die;
            Exiled.Events.Handlers.Player.Hurting -= TD;
            Exiled.Events.Handlers.Player.TogglingNoClip -= Alt;
            base.UnsubscribeEvents();
        }
        static bool _Alt = true;
        static bool Nv(Exiled.API.Features.Player player, bool isEnable = true) {
            if (isEnable) {
                Global.d = true;
                player.DisableEffect(EffectType.Slowness);
                player.EnableEffect(EffectType.MovementBoost, 40, 0);
                player.EnableEffect(EffectType.Invisible);
                player.EnableEffect(EffectType.Ghostly);
                player.IsGodModeEnabled = true;
                Timing.RunCoroutine(Pass(player), 689);
                _Alt = true;
                return true;
            } else {
                Global.d = false;
                player.IsGodModeEnabled = false;
                player.DisableEffect(EffectType.Invisible);
                player.DisableEffect(EffectType.MovementBoost);
                player.EnableEffect(EffectType.Slowness, 15, 1000000);
                Timing.KillCoroutines(689);
                _Alt = false;
                return true;
            }
        }
        void Alt(TogglingNoClipEventArgs ev) { 
            if (Check(ev.Player) && _Alt) {
                Cd = 20;
                Nv(ev.Player, false);
            } else if (Check(ev.Player) && Cd <= 0) {
                Nv(ev.Player);
            }
        }
        static int HCd = 5;
        void TD(HurtingEventArgs ev) { 
            if (Check(ev.Attacker) && !Global.d) {
                //ev.IsAllowed = false;
            } if (Check(ev.Player) && ev.DamageHandler.Type != DamageType.Unknown) {
                Cd = 60;
            }
        }
        void Die(DiedEventArgs ev) {
            Timing.KillCoroutines(689);
        }
        void Inter_D(InteractingDoorEventArgs ev) {
            if (Check(ev.Player)) {
                ev.IsAllowed = false;
                if (Cd <= 0) {
                    Nv(ev.Player);
                }
            }
        }
        void Inter(InteractedEventArgs ev) { 
            if (Check(ev.Player)) { 
                //ev.Player.EnableEffect(EffectType.Invisible);
            }
        }
        void Tp(StranglingEventArgs ev) { 
            if (Check(ev.Player)) {
                if (!ev.Player.TryGetEffect(EffectType.Invisible, out StatusEffectBase ef)) {
                    ev.IsAllowed = false;
                    if (HCd <= 0) {
                        HCd = 100;
                        List<Exiled.API.Features.Player> pl = new List<Exiled.API.Features.Player>();
                        foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List.Where(x => x.IsScp && Vector3.Distance(x.Position, ev.Player.Position) < 6 && x != ev.Player).ToList()) {
                            pl.Add(player);
                        }
                        Timing.RunCoroutine(NViz(pl));
                    }
                }
                Timing.RunCoroutine(PocketC(ev.Player, ev.Target));
                Nv(ev.Player, false);
                Cd = 20;
            }
        }
        void _Coin(SlappedEventArgs ev) {
        
        }
        void Sp(SpawnedEventArgs ev) { 
            if (Check(ev.Player) && Global.f) {
                Timing.RunCoroutine(Pass(ev.Player), 689);
                Timing.RunCoroutine(Updater());
                Nv(ev.Player);
                Global.d = true;
            }
        }
        IEnumerator<float> NViz(List<Exiled.API.Features.Player> list) {
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) { 
                player.EnableEffect(EffectType.Invisible);
            }
            yield return Timing.WaitForSeconds(5);
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) { 
                player.DisableEffect(EffectType.Invisible);
            }
            yield break;
        }
       
        public static IEnumerator<float> Updater() {
            for (; ; ) {
                yield return Timing.WaitForSeconds(1);
                Cd--;
                HCd--;
            }
        }
        public static IEnumerator<float> PocketC(Exiled.API.Features.Player player, Exiled.API.Features.Player target) { 
            if (target.Health < 15) { 
                yield break;
            }
            for (int i = 0; i <= 5; i++) { 
                yield return Timing.WaitForSeconds(1);
                if (target.Health < 15) {
                    target.Role.Set(target.Role.Type);
                    target.Position = new Vector3(68.834f, 892.089f, -105.098f);
                    player.Position = new Vector3(68.834f, 892.089f, -110.098f);
                    target.Health = 1;
                    i = 6;
                }
            }
            for (; ; ) {
                yield return Timing.WaitForSeconds(1);
                if (target.Position.z < -54 && API._System.random.Next(0, 2) == 1) {
                    target.Teleport(Room.List.Where(x => x.Type != RoomType.EzShelter && x.Type != RoomType.Pocket).ToList().GetRandomValue());
                    player.Teleport(Room.List.Where(x => x.Type != RoomType.EzShelter && x.Type != RoomType.Pocket).ToList().GetRandomValue());
                    Nv(player);
                }
            }
        }
        public static IEnumerator<float> Pass(Exiled.API.Features.Player player)
        {
            int y = 5;
            yield return Timing.WaitForSeconds(2f);
            player.EnableEffect(EffectType.Ghostly);
            player.IsGodModeEnabled = false;
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1f);
                foreach (Pickup item in Pickup.List.ToList())
                {
                    if (Vector3.Distance(player.Position, item.Position) < 5)
                    {
                        Vector3 pos = player.Position - item.Position;
                        pos.Normalize();
                        pos *= 50;
                        item.PhysicsModule.Rb.useGravity = false;
                        item.PhysicsModule.Rb.AddForce(pos);
                    }
                    else
                    {
                        item.PhysicsModule.Rb.useGravity = true;
                    }
                }
                foreach (Exiled.API.Features.Player player1 in Exiled.API.Features.Player.List)
                {
                    if (Vector3.Distance(player1.Position, player.Position) < 10)
                    {
                        if (player1.CurrentItem != null && player1.CurrentItem.Type == ItemType.Lantern)
                        {
                            player.Hurt(30);
                            if (y >= 0)
                            {
                                player.Broadcast(1, $"<color=#FF5E3F> Ви з`явитесь через {y} </color>");
                            }
                            y--;
                            if (y + 1 == 0)
                            {
                                Nv(player, false);
                                Cd = 60;
                                Global.d = false;
                            }
                        }
                    }
                }
            }
        }

    }*/
    class SCP689 : MonoBehaviour {
        Exiled.API.Features.Player player;
        void Start() {
            player = Exiled.API.Features.Player.Get(this.gameObject);
            player.Role.Set(RoleTypeId.Scp3114);
            player.MaxHealth = 1750;
            player.Health = 1750;
            player.CustomInfo = "SCP689";
            player.EnableEffect(EffectType.Invisible);
            Global.Player_Role.Add("689", player);
        }
        void Update() {
            foreach (Pickup item in Pickup.List.ToList()) {
                if (Vector3.Distance(player.Position, item.Position) < 5) {
                    Vector3 pos = player.Position - item.Position;
                    pos.Normalize();
                    pos *= 50 * Time.deltaTime;
                    item.PhysicsModule.Rb.useGravity = false;
                    item.PhysicsModule.Rb.AddForce(pos);
                } else {
                    item.PhysicsModule.Rb.useGravity = true;
                }
            }
        }
        void OnEnable() { 
        
        }
        void OnDisable() {
            player.CustomInfo = string.Empty;
            Global.Player_Role.Remove("689");
        }
    }
}
