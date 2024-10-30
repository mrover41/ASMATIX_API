using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace API {
    public static class Player_Mod {
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
                    Count += 1;
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
            player_score.Remove(ev.Player);
        }
        public static Player Best_Player() {
            int Tmp_Score = 0;
            Player best = Player.List.Last();
            try {
                foreach (Player player in Player.List)
                {
                    if (player_score[player].Count >= Tmp_Score && player_score.ContainsKey(player))
                    {
                        Tmp_Score = player_score[player].Count;
                        best = player;
                    }
                }
            } catch (Exception ex) { 
                Log.Info(ex.Message);
            }
            return best;
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
                    Exiled.API.Features.Player pl = Exiled.API.Features.Player.List.GetRandomValue();
                    TestPlugin.Global.Player_Role.Add("035", pl);
                    //Spawn_System.Spawn(Exiled.API.Features.Player.List.Where(x => x.IsScp)?.ToList().RandomItem(), 35);
                    pl.GameObject.AddComponent<SCP035>();
                }
            }
        }
    }
    public static class _System {
        public static System.Random random = new System.Random();
    }
}
