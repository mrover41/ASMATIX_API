using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin {
    internal class SCP420J : CustomItem {
        public override string Description { get; set; } = ";)";
        public override float Weight { get; set; } = 2f;
        public override string Name { get; set; } = "SCP-420-J";
        public override uint Id { get; set; } = 122;
        public override ItemType Type { get; set; } = ItemType.Adrenaline;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties() {
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
        };

        protected override void SubscribeEvents() {
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsed;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsed;
            base.UnsubscribeEvents();
        }
        void OnUsed(UsingItemCompletedEventArgs ev) {
            if (!Check(ev.Item)) {
                return;
            }
            ev.IsAllowed = false;
            ev.Player.EnableEffect(EffectType.Slowness, 255, 10);
        }
    }
}
