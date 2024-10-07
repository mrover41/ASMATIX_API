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
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Items;
using PluginAPI.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VoiceChat;

namespace TestPlugin.Roles
{
    public class SCP689 : CustomRole {
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
        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            Exiled.Events.Handlers.Player.Spawned += Sp;
            Exiled.Events.Handlers.Scp3114.Slapped += _Coin;
            Exiled.Events.Handlers.Scp3114.Strangling += Tp;
            Exiled.Events.Handlers.Player.Interacted += Inter;
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.Spawned -= Sp;
            Exiled.Events.Handlers.Scp3114.Slapped -= _Coin;
            Exiled.Events.Handlers.Scp3114.Strangling -= Tp;
            Exiled.Events.Handlers.Player.Interacted -= Inter;
            base.UnsubscribeEvents();
        }
        void Inter(InteractedEventArgs ev) { 
            if (Check(ev.Player)) { 
                ev.Player.EnableEffect(EffectType.Invisible);
            }
        }
        void Tp(StranglingEventArgs ev) { 
            if (Check(ev.Player)) { 
                ev.IsAllowed = false;
                ev.Target.Teleport(RoomType.Surface, new Vector3(0, 0, 0));
                ev.Player.Teleport(RoomType.Surface, new Vector3(0f, 0f, 0f));
            }
        }
        void _Coin(SlappedEventArgs ev) {
            if (Check(ev.Player)) {
                ev.Player.EnableEffect(EffectType.Invisible);
            }
        }
        void Sp(SpawnedEventArgs ev) { 
            if (Check(ev.Player)) {
                Timing.RunCoroutine(API.Pass(ev.Player), 689);
            }
        }
    }
}
