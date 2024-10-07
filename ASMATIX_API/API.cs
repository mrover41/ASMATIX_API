using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestPlugin {
    public static class API {
        public static IEnumerator<float> Updater() {
        for (; ; ) {
            yield return Timing.WaitForSeconds(1);
            Global.Cd[0]--;
        }
    }
        public static IEnumerator<float> Damage(Player player, float s, int damage) {
            for (; ; ) {
                yield return Timing.WaitForSeconds(s);
                if (player.Health > damage) {
                    player.Health -= damage;
                } else {
                    player.Kill(DamageType.ParticleDisruptor);
                    Timing.KillCoroutines(035);
                }
            }
        }
        public static IEnumerator<float> _Heal(Player player, int Health, float s) {
            player.EnableEffect(EffectType.Burned);
            player.Heal(Health);
            yield return Timing.WaitForSeconds(s);
            player.DisableEffect(EffectType.Burned);
        }
        public static int RoundTime;
        public static IEnumerator<float> Pass(Player player) {
            yield return Timing.WaitForSeconds(4f);
            player.EnableEffect(EffectType.Invisible);
            player.IsGodModeEnabled = false;
            for (; ; ) {
                yield return Timing.WaitForSeconds(1f);
                foreach (Pickup item in Pickup.List.Where(r => Vector3.Distance(player.Position, r.Position) < 100).ToList())
                {
                    Vector3 pos = player.Position - item.Position;
                    pos.Normalize();
                    pos *= 500;
                    item.PhysicsModule.Rb.AddForce(pos);
                }
            }
        }
    }
    public static class Global {
        public static Action Run_ob;
        public static Action Stop_ob;
        public static Dictionary<Exiled.API.Features.Items.Item, int> it = new Dictionary<Exiled.API.Features.Items.Item, int>();
        public static Dictionary<int, int> Cd = new Dictionary<int, int>() {
            { 0, 0 },
        };
        public static Dictionary<Player, int> Player_Oboron = new Dictionary<Player, int>();
        public static bool SCP035 = true;
    }
}
