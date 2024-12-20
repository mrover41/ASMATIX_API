﻿using Exiled.API.Enums;
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
using VoiceChat;
using VoiceChat.Networking;
using static PlayerRoles.PlayableScps.Scp3114.Scp3114Strangle;

namespace API {
    class EventPool {

    }
    public static class Player_Mod {
        public static IEnumerator<float> Damage(Player player, float s, int damage) {
            for (; ; ) {
                yield return Timing.WaitForSeconds(s);
                if (player.Health > damage) {
                    player.Health -= damage;
                } else {
                    player.Kill(Exiled.API.Enums.DamageType.ParticleDisruptor);
                }
            }
        }
        public static IEnumerator<float> _Heal(Player player, int Health, float s) {
            player.EnableEffect(EffectType.Burned);
            player.Heal(Health);
            yield return Timing.WaitForSeconds(s);
            player.DisableEffect(EffectType.Burned);
        }
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
    }
    public class Score_Counter {
        public int Count { get; private set; }
        Player player;
        public void Connect_Token(Player player) {
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
        public static Dictionary<Player, Score_Counter> player_score { get; private set; } = new Dictionary<Player, Score_Counter>();
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
        public static Player Best_Player() {
            int Tmp_Score = 0;
            Player best = Player.List.First();
            try {
                foreach (Player player in Player.List.Where(x => !x.IsScp)) {
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
        public static void Spawn(Player player, uint ID) {
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
    
}

namespace Patches {
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
    internal static class Patch689 {
        public static bool Prefix(Scp3114Strangle __instance, NetworkReader reader, ref StrangleTarget? __result) {
            __result = default;
            return true;
        }
    }*/
}
