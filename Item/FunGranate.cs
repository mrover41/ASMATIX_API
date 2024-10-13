using CustomPlayerEffects;
using CustomRendering;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.EventArgs;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp244;
using InventorySystem.Items.Usables.Scp244;
using InventorySystem.Items.Usables.Scp244.Hypothermia;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PlayerRoles;

namespace TestPlugin.Item
{
    public class FunGranate : CustomGrenade {
        public float Duration { get; set; } = 10f;
        public override ItemType Type { get; set; } = ItemType.GrenadeFlash;
        public override float FuseTime { get; set; } = 10;
        public override float Weight { get; set; } = 1;
        public override string Description { get; set; } = "Димова граната";
        public override uint Id { get; set; } = 140;
        public override string Name { get; set; } = "Граната";
        public override bool ExplodeOnCollision { get; set; } = true;
        public override SpawnProperties SpawnProperties { get; set; } = null;
        List<Vector3> Granates = new List<Vector3>();
        protected override void SubscribeEvents() {
            Exiled.Events.Handlers.Player.Hurting += Hut;
            Exiled.Events.Handlers.Player.Spawned += Spawner;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.Hurting -= Hut;
            Exiled.Events.Handlers.Player.Spawned -= Spawner;
            base.UnsubscribeEvents();
        }
        void Spawner(SpawnedEventArgs ev) { 
            if (ev.Player.Role.Type == RoleTypeId.NtfSergeant) {
                CustomItem.TryGive(ev.Player, 140, false);
            }
        }
        void Hut(HurtingEventArgs ev) { 
            if (ev.DamageHandler.Type == Exiled.API.Enums.DamageType.Hypothermia) {
                foreach (Vector3 vector in Granates) { 
                    if (Vector3.Distance(ev.Player.Position, vector) <= 5) {
                        ev.IsAllowed = false;
                    }
                }
            }
        }
        protected override void OnExploding(ExplodingGrenadeEventArgs ev) {
            Scp244Pickup fog = Pickup.Create(ItemType.SCP244a).As<Scp244Pickup>();
            fog.Scale = Vector3.one * 0.01f;
            fog.Rotation = Quaternion.Euler(0, 0, 90);
            fog.ActivationDot = 0;
            fog.Spawn(ev.Position, fog.Rotation);
            Granates.Add(fog.Position);
            Timing.CallDelayed(10, () => {fog.State = Scp244State.Destroyed; });
            Timing.CallDelayed(10, () => { Granates.Remove(fog.Position); });
            ev.IsAllowed = false;
            base.OnExploding(ev);
        }
    }
}
