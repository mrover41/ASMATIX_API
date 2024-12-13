using Exiled.API.Enums;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestPlugin.Mr_Over41.Item {
    public class WtfGranate : CustomGrenade {
        public float Duration { get; set; } = 10f;
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;
        public override float FuseTime { get; set; } = 10;
        public override float Weight { get; set; } = 1;
        public override string Description { get; set; } = "Wtf";
        public override uint Id { get; set; } = 141;
        public override string Name { get; set; } = "granatef";
        public override bool ExplodeOnCollision { get; set; } = true;
        public override SpawnProperties SpawnProperties { get; set; } = null;
        /*public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint> {
            new DynamicSpawnPoint() {
                Location = SpawnLocationType.Inside173Gate,
                Chance = 100
            }
        },
            StaticSpawnPoints = new List<StaticSpawnPoint> {
            new StaticSpawnPoint() {
                Chance = 100,
                Position = new UnityEngine.Vector3(0, 0, 0), Name = "f"
            }
        }
        };*/
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
            MapEditorReborn.API.Features.Serializable.PrimitiveSerializable primitiveSerializable = new MapEditorReborn.API.Features.Serializable.PrimitiveSerializable(PrimitiveType.Cube, "green", AdminToys.PrimitiveFlags.Visible);
            MapEditorReborn.API.Features.ObjectSpawner.SpawnPrimitive(primitiveSerializable, ev.Position, Quaternion.identity, new Vector3(1, 1, 1));
            ev.IsAllowed = false;
            base.OnExploding(ev);
        }
    }
}
