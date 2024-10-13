using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestPlugin.Item
{
    public class Water : CustomItem {
        public override string Description { get; set; } = "Дає прискорення";
        public override float Weight { get; set; } = 0f;
        public override string Name { get; set; } = "Водичка";
        public override uint Id { get; set; } = 0;
        public override ItemType Type { get; set; } = ItemType.SCP207;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties() {
            Limit = 10,
            DynamicSpawnPoints = new List<DynamicSpawnPoint> {
                new DynamicSpawnPoint() {
                    Location = SpawnLocationType.Inside914,
                    Chance = 100
                },
                new DynamicSpawnPoint() {
                    Location = SpawnLocationType.InsideGateA,
                    Chance = 100
                },
                new DynamicSpawnPoint() {
                    Location = SpawnLocationType.InsideGateB,
                    Chance = 100
                },
                 new DynamicSpawnPoint() {
                    Location = SpawnLocationType.InsideNukeArmory,
                    Chance = 100
                }
            }, 
            StaticSpawnPoints = new List<StaticSpawnPoint> {
                new StaticSpawnPoint() {
                        Chance = 100,
                        Position = new Vector3(0, 0, 0), Name = "Водичка"
                }
            }
        };
        protected override void SubscribeEvents() {
            Exiled.Events.Handlers.Player.UsingItemCompleted += Us;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents() {
            Exiled.Events.Handlers.Player.UsingItemCompleted -= Us;
            base.UnsubscribeEvents();
        }
        void Us(UsingItemCompletedEventArgs ev) { 
            if (Check(ev.Item)) {
                ev.Player.RemoveItem(ev.Item);
                Timing.RunCoroutine(_Effect(ev.Player));
            }
        }
        IEnumerator<float> _Effect(Player player) {
            player.DisableEffect(EffectType.AntiScp207);
            player.DisableEffect(EffectType.Scp207);
            player.EnableEffect(EffectType.MovementBoost, 40, 10);
            yield return Timing.WaitForSeconds(5);
            player.DisableEffect(EffectType.MovementBoost);
        }
    }
}
