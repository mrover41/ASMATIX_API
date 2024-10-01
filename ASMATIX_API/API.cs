using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace TestPlugin.ASMATIX_API {
    public static class API {
        public static IEnumerator<float> Damage(Player player, float s, int damage) {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(s);
                if (player.Health > damage)
                {
                    player.Health -= damage;
                }
                else
                {
                    player.Kill(DamageType.ParticleDisruptor);
                    Timing.KillCoroutines(Global.coroutine);
                }
            }
        }
        public static IEnumerator<float> _Heal(Player player, int Health, float s) {
            player.EnableEffect(EffectType.Burned);
            player.Heal(Health);
            yield return Timing.WaitForSeconds(s);
            player.DisableEffect(EffectType.Burned);
        }
    }
    public static class Global { 
        public static CoroutineHandle coroutine;
        public static Dictionary<Exiled.API.Features.Items.Item, int> it = new Dictionary<Exiled.API.Features.Items.Item, int>();
    }
}
