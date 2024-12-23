using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;
using Utils.NonAllocLINQ;
using VoiceChat;
using VoiceChat.Networking;
using static PlayerRoles.PlayableScps.Scp3114.Scp3114Strangle;

namespace API {
    class EventPool {

    }
    public static class Player_Mod {
        public static IEnumerator<float> Damage(Exiled.API.Features.Player player, float s, int damage) {
            for (; ; ) {
                yield return Timing.WaitForSeconds(s);
                if (player.Health > damage) {
                    player.Health -= damage;
                } else {
                    player.Kill(Exiled.API.Enums.DamageType.ParticleDisruptor);
                }
            }
        }
        public static IEnumerator<float> _Heal(Exiled.API.Features.Player player, int Health, float s) {
            player.EnableEffect(EffectType.Burned);
            player.Heal(Health);
            yield return Timing.WaitForSeconds(s);
            player.DisableEffect(EffectType.Burned);
        }
        public static List<Exiled.API.Features.Items.Item> Player_Inventry(Exiled.API.Features.Player player) {
            List<Exiled.API.Features.Items.Item> Inv = new List<Exiled.API.Features.Items.Item>();
            foreach (Exiled.API.Features.Items.Item item in player.Items.ToList()) {
                Inv.Add(item);
            }
            return Inv;
        }
        public static void Give_Item_List(Exiled.API.Features.Player player, List<Exiled.API.Features.Items.Item> inv) {
            foreach (Exiled.API.Features.Items.Item item in inv) {
                player.AddItem(item.Type);
            }
        }
    }
    public class Score_Counter {
        public int Count { get; private set; }
        Exiled.API.Features.Player player;
        public void Connect_Token(Exiled.API.Features.Player player) {
            Exiled.Events.Handlers.Player.Died += Add_Token;
            Exiled.Events.Handlers.Player.Died += Reset_Token;
            this.player = player;
        }
        public void UnConnect_Token() {
            Exiled.Events.Handlers.Player.Died -= Add_Token;
            Exiled.Events.Handlers.Player.Died -= Reset_Token;
        }
        void Add_Token(DiedEventArgs ev) { 
            if (ev.Attacker == player) {
                if (ev.Attacker.IsScp) {
                    Count = 0;
                } else {
                    Count += 20;
                }
            }
        }
        void Reset_Token(DiedEventArgs ev) { 
            if (ev.Player == player) { 
                Count = 0;
            }
        }
        public void Set_Token(int token) { 
            Count = token;
        }
        public Score_Counter() {
            Count = 0;
        }
        public Score_Counter(int count) { 
            Count = count;
        }
    }
    public static class API {
        public static Dictionary<Exiled.API.Features.Player, Score_Counter> player_score { get; private set; } = new Dictionary<Exiled.API.Features.Player, Score_Counter>();
        public static void Load() {
            Exiled.Events.Handlers.Player.Joined += Connect_Player;
            Exiled.Events.Handlers.Player.Left += Disconnect_Player;
        }
        public static void UnLoad() {
            Exiled.Events.Handlers.Player.Joined -= Connect_Player;
            Exiled.Events.Handlers.Player.Left -= Disconnect_Player;
        }
        static void Connect_Player(JoinedEventArgs ev) {
            Score_Counter score_Counter = new Score_Counter();
            score_Counter.Connect_Token(ev.Player);
            player_score.Add(ev.Player, score_Counter);
        }
        static void Disconnect_Player(LeftEventArgs ev) { 
            if (player_score.ContainsKey(ev.Player)) {
                player_score.Remove(ev.Player);
            }
        }
        public static void EnableCustumEffect(Exiled.API.Features.Player target, Data.Enums.CustomEffect effect) { 
            switch (effect) {
                case Data.Enums.CustomEffect.Sleep:
                    target.CurrentItem = null;
                    target.Inventory.enabled = false;
                    target.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                    Ragdoll rg = Ragdoll.CreateAndSpawn(target.Role.Type, target.Nickname, "Немного помялся", target.Position, target.Rotation);
                    target.EnableEffect(EffectType.Deafened, 255);
                    target.EnableEffect(EffectType.Invisible, 255);
                    target.EnableEffect(EffectType.Ensnared, 255);
                    target.EnableEffect(EffectType.Flashed, 255);

                    Data.CustomEffectList.effectUps.Add(new EffectUp(target, Data.Enums.CustomEffect.Sleep));
                    break;
            }
        }
        public static void DisableCustumEffect(Exiled.API.Features.Player target, Data.Enums.CustomEffect effect) { 
            switch (effect) {
                case Data.Enums.CustomEffect.Sleep:
                    target.Inventory.enabled = true;
                    target.Scale = new Vector3(1, 1, 1);
                    target.DisableEffect(EffectType.Deafened);
                    target.DisableEffect(EffectType.Invisible);
                    target.DisableEffect(EffectType.Ensnared);
                    target.DisableEffect(EffectType.Flashed);

                    foreach (EffectUp up in Data.CustomEffectList.effectUps) {
                        if (up.player == target && up.effect == effect) { 
                            Data.CustomEffectList.effectUps.Remove(up);
                        }
                    }
                    break;
            }
        }
        public static bool CheckCustumEffect(Exiled.API.Features.Player player, Data.Enums.CustomEffect effect) { 
            foreach (EffectUp up in Data.CustomEffectList.effectUps) { 
                if (up.player == player) { 
                    if (up.effect == effect) { 
                        return true;
                    } else { 
                        return false;
                    }
                }
            }
            return false;
        }
        public static Exiled.API.Features.Player Best_Player() {
            int Tmp_Score = 0;
            Exiled.API.Features.Player best = Exiled.API.Features.Player.List.First();
            try {
                foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List.Where(x => !x.IsScp)) {
                    if (player_score.ContainsKey(player)) {
                        if (Global.Player_Role.ContainsKey("035")) { 
                            if (Global.Player_Role["035"] == player) {
                                break;
                            }
                        }
                        if (player_score[player].Count >= Tmp_Score) {
                            Tmp_Score = player_score[player].Count;
                            best = player;
                        }
                    }
                }
            } catch (Exception ex) { 
                Log.Info($"[Asmatix_API] Error in metod 'BestPlayer': {ex.Message}");
            }
            return best;
        }
        public static string MakeColorLighter(string hexColor, float lightenFactor = 0.1f)
        {
            // Убедитесь, что lightenFactor в диапазоне 0-1
            lightenFactor = Mathf.Clamp01(lightenFactor);

            // Конвертация HEX в Color
            if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
            {
                // Увеличиваем яркость
                color.r = Mathf.Clamp01(color.r + lightenFactor);
                color.g = Mathf.Clamp01(color.g + lightenFactor);
                color.b = Mathf.Clamp01(color.b + lightenFactor);

                // Конвертация обратно в HEX
                return ColorUtility.ToHtmlStringRGB(color);
            }

            Debug.LogError("Invalid HEX color format.");
            return hexColor; // Возвращаем исходный, если формат неверный
        }
    }
    public static class Spawn_System {
        public static void Spawn(Exiled.API.Features.Player player, uint ID) {
            CustomRole.Get(ID).AddRole(player);
        }
        public static void RoundSt() {
            //SPAWN
            //343
            if (Exiled.API.Features.Player.List.Count() >= 8) {
                if (_System.random.Next(0, 100) < 1) {
                    Spawn_System.Spawn(Exiled.API.Features.Player.List.Where(x => x.Role.Type == RoleTypeId.ClassD)?.ToList().RandomItem(), 343);
                }
            }
            //035
            if (Exiled.API.Features.Player.List.Count() >= 8) {
                if (_System.random.Next(0, 100) < 50) {
                    Exiled.API.Features.Player pl = Exiled.API.Features.Player.List.Where(x => x.IsScp)?.ToList().GetRandomValue();
                    pl.GameObject.AddComponent<SCP035>();
                }
            }
        }
    }
    public static class _System {
        public static System.Random random = new System.Random();
    }

    public class EffectUp { 
        public Exiled.API.Features.Player player;
        public Data.Enums.CustomEffect effect { get; private set; }
        public EffectUp() { 
        
        }
        public EffectUp(Exiled.API.Features.Player player, Data.Enums.CustomEffect effect) { 
            this.player = player;
            this.effect = effect;
        }
    }
    
}



namespace Patches {
    /*[HarmonyPatch(typeof(Scp3114Strangle), nameof(Scp3114Strangle.ProcessAttackRequest))]
    public static class Patch3114 {
        [HarmonyPostfix]
        public static void Postfix(ref Scp3114Strangle __instance, NetworkReader reader, ref StrangleTarget? __result) {
            try {
                // Логируем вызов метода
                Log.Info("Patch3114: Вызван ProcessAttackRequest.");

                // Проверка NetworkReader на остаток данных
                if (reader.Remaining == 0) {
                    Log.Error("Patch3114: NetworkReader пуст. Пропуск изменений.");
                    return;
                }

                // Устанавливаем откат
                Debug.Log($"Patch3114: Установка _onKillCooldown на 1000 (предыдущее значение: {__instance._onKillCooldown}).");
                __instance._onKillCooldown = 1000;
                __instance._onReleaseCooldown = 1000;

                if (__result != null) {
                    Log.Info($"Patch3114: результат ({__result}) на.");
                }
            } catch (Exception ex) {
                // Логируем ошибки для отладки
                Log.Error($"Patch3114: Ошибка при выполнении патча: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }*/
    
        /* [HarmonyPatch(typeof(VoiceChat.Networking.VoiceTransceiver), nameof(VoiceChat.Networking.VoiceTransceiver.ServerReceiveMessage))]
         public class SCP035_Voice_Patch {
             public static bool Prefix(NetworkConnection conn, VoiceMessage msg) {
                 try { 
                     if (Global.Player_Role.ContainsKey("035")) {
                         Player pl = Player.Get(msg.Speaker.PlayerId);
                         if (pl == Global.Player_Role["035"]) {
                             Log.Info($"{pl.Nickname} sp to {msg.Channel}");
                             foreach (ReferenceHub allHub in ReferenceHub.AllHubs) {
                                 //pl.VoiceChannel = VoiceChatChannel.ScpChat;
                                 VoiceChatChannel voiceChatChannel2 = pl.VoiceModule.ValidateReceive(msg.Speaker, VoiceChatChannel.ScpChat);
                                 msg.Channel = voiceChatChannel2;
                                 pl.VoiceChannel = msg.Channel;
                                 allHub.connectionToClient.Send(msg);
                             }
                         }
                     }
                     return false;
                 } catch (Exception ex) {
                     Log.Error($"[ASMATIX_API] Error in SCP035_Voice_Patch: {ex.Message}");
                     return true;
                 }
             }
         }*/
        /*[HarmonyPatch(typeof(Scp3114Dance), nameof(Scp3114Dance.ClientProcessRpc))]
        public class SCP_3114_P {
            static void Postfix(Scp3114Dance __instance, NetworkReader reader) {
                __instance.DanceVariant = 1;
            }
        }

        [HarmonyPatch(typeof(Scp3114Strangle), nameof(Scp3114Strangle.ProcessAttackRequest))]
        internal static class Patch3114 {
            public static bool Prefix(Scp3114Strangle __instance, NetworkReader reader, ref StrangleTarget? __result) {
                __result = default;
                return true;
            }
        }*/
    }
