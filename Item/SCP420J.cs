using CustomPlayerEffects;
using CustomRendering;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace TestPlugin {
    public class SCP420J : CustomItem {
        public override string Description { get; set; } = ";)";
        public override float Weight { get; set; } = 2f;
        public override string Name { get; set; } = "SCP-420-J";
        public override uint Id { get; set; } = 124;
        public override ItemType Type { get; set; } = ItemType.Adrenaline;
        public override SpawnProperties SpawnProperties { get; set; } = null;

        /*public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties() {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint> {
            new DynamicSpawnPoint() {
                Location = SpawnLocationType.InsideIntercom,
                Chance = 100
            }
        },
            StaticSpawnPoints = new List<StaticSpawnPoint> {
            new StaticSpawnPoint() {
                Chance = 100,
                Position = new UnityEngine.Vector3(0, 0, 0), Name = "SCP-420-J"
            }
        }
        };*/
        List<Exiled.API.Features.Player> playersList = new List<Exiled.API.Features.Player>();
        protected override void SubscribeEvents() {
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsed;
            Exiled.Events.Handlers.Player.Hurting += Damage;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsed;
            Exiled.Events.Handlers.Player.Hurting -= Damage;
            base.UnsubscribeEvents();
        }
        void Damage(HurtingEventArgs ev) { 
            if (ev.DamageHandler.Type == DamageType.CardiacArrest) { 
                if (playersList.Contains(ev.Player)) {
                    ev.IsAllowed = false;
                }
            }
        }
        void OnUsed(UsingItemCompletedEventArgs ev) {
            if (!Check(ev.Item)) {
                return;
            }
            playersList.Add(ev.Player);
            ev.Player.EnableEffect(EffectType.Slowness, 255, 10);
            ev.Player.EnableEffect(EffectType.CardiacArrest, 255, 10);
            ev.Player.EnableEffect(EffectType.FogControl, 255, 10);
            FogControl fogControl = ev.Player.GetEffect(EffectType.FogControl) as FogControl;
            fogControl.Duration = 10;
            EffectTypeExtension.SetFogType(fogControl, FogType.Decontamination);
            Timing.CallDelayed(10, () => { playersList.Remove(ev.Player); });
        }
    }
}
