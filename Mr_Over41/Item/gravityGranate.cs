using Exiled.API.Enums;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestPlugin.Mr_Over41.Item
{
   public class gravityGranate : CustomGrenade {
        public float Duration { get; set; } = 10f;
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;
        public override float FuseTime { get; set; } = 10;
        public override float Weight { get; set; } = 5;
        public override string Description { get; set; } = "r";
        public override uint Id { get; set; } = 200;
        public override string Name { get; set; } = "g";
        public override bool ExplodeOnCollision { get; set; } = false;
        public override Vector3 Scale { get; set; } = new Vector3(2, 5, 2); 
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint> {
            new DynamicSpawnPoint() {
                Location = SpawnLocationType.Inside173Connector,
                Chance = 100
            }
        },
            StaticSpawnPoints = new List<StaticSpawnPoint> {
            new StaticSpawnPoint() {
                Chance = 100,
                Position = new UnityEngine.Vector3(0, 0, 0), Name = "f"
            }
        }
        };
        protected override void SubscribeEvents() {
            Exiled.Events.Handlers.Player.ChangedItem += Select_Info;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.ChangedItem -= Select_Info;
            base.UnsubscribeEvents();
        }
        void Select_Info(ChangedItemEventArgs ev) {
            if (Check(ev.Item)) {
                ev.Player.Broadcast(4, "<b><color=#FCF7D9>Ви підібрали</color> <color=#A9BCD4>😣</color></b>");
            }
        }
        protected override void OnExploding(ExplodingGrenadeEventArgs ev) {
            foreach (Pickup pickup in Pickup.List) { 
                pickup.PhysicsModule.Rb.useGravity = false;
                pickup.PhysicsModule.Rb.AddForce(0, 5, 0);
            }
            Timing.CallDelayed(60, () => {
                foreach (Pickup pickup in Pickup.List) {
                    pickup.PhysicsModule.Rb.useGravity = true;
                }
            });
            ev.IsAllowed = false;
            base.OnExploding(ev);
        }
    }
}
