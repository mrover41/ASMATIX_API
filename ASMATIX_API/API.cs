using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestPlugin {
    public static class API {
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
        public static List<Exiled.API.Features.Items.Item> Player_Inventry(Player player) {
            List<Exiled.API.Features.Items.Item> Inv = new List<Exiled.API.Features.Items.Item>();
            foreach (Exiled.API.Features.Items.Item item in player.Items.ToList()) {
                Inv.Add(item);
            }
            return Inv;
        }
        public static void Give_Item_List(Player player, List<Exiled.API.Features.Items.Item> inv) {
            foreach (Exiled.API.Features.Items.Item item in inv) {
                player.AddItem(item.Type);
            }
        }
        public static System.Random random = new System.Random();
    }
}
