﻿using Exiled.API.Extensions;
using MEC;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI.Core;
using PluginAPI.Events;
using Respawning;
using RueI.Displays;
using RueI.Displays.Scheduling;
using RueI.Elements;
using RueI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Waits;
using YamlDotNet.Core.Tokens;

namespace TestPlugin {
    public static class WaitPlayer_HUD {
        static CoroutineHandle coroutine;
        static string Base_Texst = $"<color=#FED93B> Очікування гравців </color>\n" +
            $"<color=#407DFE> Раунд розпочнеться через {0} секунд </color>";
        static string Locked_Round_Texst = $"<color=#FED93B> Очікування гравців </color>\n" +
            $"<color=#407DFE> Раунд заблоковано </color>";
        public static void Run() {
            coroutine = Timing.RunCoroutine(Hud_Updater());
        }
        public static void Stop() {
            Timing.KillCoroutines(coroutine);
        }
        static IEnumerator<float> Hud_Updater() {
            yield return Timing.WaitForSeconds(2);
            for (; ; ) { 
                if (!PluginAPI.Core.Round.IsRoundStarted) { 
                    if (Exiled.API.Features.Round.IsLobbyLocked || Exiled.API.Features.Player.List.Count <= 1) { 
                        foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) {
                            player.ShowHint(Locked_Round_Texst);
                        }
                    } else if (GameCore.RoundStart.singleton.NetworkTimer > 0) { 
                        foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) {
                            player.ShowHint(Base_Texst);
                            Base_Texst = $"<color=#FED93B> Очікування гравців </color>\n" +
                                $"<color=#407DFE> Раунд розпочнеться через {GameCore.RoundStart.singleton.NetworkTimer} секунд </color>";
                        }
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }
    }
    class Human_HUD : MonoBehaviour {
        Exiled.API.Features.Player player;
        string HUD_Role;
        string HUD_Name;
        string HUD_SCPs;
        string HUD_MyTeam_Player;
        string SCPl;
        string Generator_HUD;
        string _Ghost_HUD;
        //RESULT
        string Mixed_HUD;
        string HUD_Result;
        //d
        int size = 20;
        int offset = 0;
        string[] Name = new string[] { "Охоронець", "Вчений", "Д-Клас", "Повстанець Хаосу", "МОГ" };
        void Start() {
            player = Exiled.API.Features.Player.Get(this.gameObject);
            Timing.RunCoroutine(Update_HUD(), $"{player.Id}");
        }
        IEnumerator<float> Update_HUD() {
            for (; ; ) {
                if (player == null) {
                    yield break;
                }
                offset = -20;
                HUD_Name = $"<align=left><color={player.Role.Color.ToHex()}><size={size}><b>       〚⭐〛Ім'я: {player.Nickname} </b></size></color></align>";
                HUD_Role = $"<align=left><color={player.Role.Color.ToHex()}><size={size}><b>       〚🕑〛Час раунду: {PluginAPI.Core.Round.Duration.Minutes.ToString("D2")} : {PluginAPI.Core.Round.Duration.Seconds.ToString("D2")} </b></size></color></align>";
                HUD_MyTeam_Player = $"<align=left><color={player.Role.Color.ToHex()}><size={size}><b>       〚🍪〛Cоюзникiв: {Exiled.API.Features.Player.List.Where(x => x.LeadingTeam == player.LeadingTeam).ToList().Count()} </b></size></color></align>";
                HUD_SCPs = $"<align=left><color={player.Role.Color.ToHex()}><size={size}><b>{SCPl} </b></size></color></align>";
                Generator_HUD = $"<align=left><color={player.Role.Color.ToHex()}><size={size}><b>       〚🚂〛Акт.Генераторів: {Scp079Recontainer.AllGenerators.Count(x => x.Engaged).ToString()}</b></size></color></align>";
                _Ghost_HUD = $"<size={size}></size>";
                DisplayCore displayCore = DisplayCore.Get(player.ReferenceHub);

                var elementReference_1 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(HUD_Name, 100, TimeSpan.FromSeconds(2), elementReference_1);

                var elementReference_3 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(HUD_Role, 70, TimeSpan.FromSeconds(2), elementReference_3);

                var elementReference_4 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(HUD_MyTeam_Player, 40, TimeSpan.FromSeconds(2), elementReference_4);

                var elementReference_5 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(Generator_HUD, 10, TimeSpan.FromSeconds(2), elementReference_5);

                var elementReference_2 = new TimedElemRef<SetElement>();
                var elementReference_6 = new TimedElemRef<SetElement>();
                if (API.API.Best_Player() == player) {
                    offset += 45;
                    displayCore.SetElemTemp($"<align=right><size={size}><color={player.Role.Color.ToHex()}><b>〚🏆〛Ви найкращий гравець </b></color></size></align>", offset, TimeSpan.FromSeconds(2), elementReference_2);
                } if (PluginAPI.Core.Warhead.IsDetonationInProgress) {
                    offset += 45;
                    displayCore.SetElemTemp($"<align=right><size={size}><color={player.Role.Color.ToHex()}><b>〚💥〛Час до вибуху: {Math.Round(PluginAPI.Core.Warhead.DetonationTime)}</b></color></size></align>", offset, TimeSpan.FromSeconds(2), elementReference_6);
                }

                HUD_Result = "" + Mixed_HUD;
                SCPl = $" <b>        〚🎃〛Аномалії:";
                //player.ShowHint(HUD_Result, 5);
                yield return Timing.WaitForSeconds(1);
                displayCore.RemoveReference(elementReference_1);
                displayCore.RemoveReference(elementReference_2);
                displayCore.RemoveReference(elementReference_3);
                displayCore.RemoveReference(elementReference_4);
                displayCore.RemoveReference(elementReference_5);
                displayCore.RemoveReference(elementReference_6);
            }
        }
        void OnDisable() { 
            Timing.KillCoroutines($"{player.Id}");
        }
        /*string Role_Translste(Exiled.API.Features.Player player) { 
            switch(player.Role.Type) {
                case RoleTypeId.ClassD:
                    return Name[2];
                case RoleTypeId.FacilityGuard:
                    return Name[0];
                case RoleTypeId.Scientist:
                    return Name[1];
                default:
                    if (player.IsNTF) { 
                        return Name[4];
                    } else { 
                        return Name[3];
                    }
            }
        }*/
    }
    class Ghost_HUD : MonoBehaviour {
        Exiled.API.Features.Player player;
        string AlphaWarhead_HUD;
        string RoundTime_HUD;
        string Spawn_T;
        string Spawn_W;
        string Progress_Bar;
        List<string> Info = new List<string>() {
            "<align=left><size=23>Хотіли б відпочити?\nНапишіть в консоль: .h</size></align>", 
            "<size=23><align=left>Хочеш дюпати ?\nДублікатор - це ваше рішення.</size></align>",
            "<size=23><align=left>А ви знали?\nТранквілізатор знаходиться в GR18!</size></align>",
            "<size=23><align=left>А ви знали?\nЩо у нас є димова граната (тільки у Сержанта МОГ)</size></align>",
            "<size=23><align=left> А ви знали?\nЩо заряджений MicroHID ламає двері!</size></align>",
            "<size=23><align=left>Прикиньте!\nА у нас на сервері є SCP-343, SCP-035!</size></align>",

        };
        void Start() {
            player = Exiled.API.Features.Player.Get(this.gameObject);
            Timing.RunCoroutine(Update_HINT(), $"{player.Id}");
            Timing.RunCoroutine(Update_Info(), $"{player.Id}");
        }
        IEnumerator<float> Update_Info() { 
            for (; ; ) {
                DisplayCore displayCore = DisplayCore.Get(player.ReferenceHub);
                var elementReference = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(Info.GetRandomValue().ToString(), 800, TimeSpan.FromSeconds(6f), elementReference);
                yield return Timing.WaitForSeconds(5);
                displayCore.RemoveReference(elementReference);
            }
        }
        /*string Counter() {
            try {
                string Progress_Bar = string.Empty;
                for (int i = 0; i <= RespawnTokensManager.Counters[1].Amount; i++) {
                    Progress_Bar = Progress_Bar + "█";
                }
                return Progress_Bar;
            } catch (Exception ex) { 
                Exiled.API.Features.Log.Error(ex.Message);
                return string.Empty;
            }
        }*/
        IEnumerator<float> Update_HINT() {
            for (; ; ) {
                if (player == null) {
                    yield break;
                }
                DisplayCore displayCore = DisplayCore.Get(player.ReferenceHub);
                Progress_Bar = $"<size=23></size>";
                RoundTime_HUD = $"<color=#00ff08><size=23><align=right> Час раунду:<color=#00634e> {PluginAPI.Core.Round.Duration.Minutes.ToString("D2")} : {PluginAPI.Core.Round.Duration.Seconds.ToString("D2")} </align></size></color>\n";
                if (PluginAPI.Core.Warhead.IsDetonationInProgress) {
                    AlphaWarhead_HUD = $"<color=#808080><size=23><align=right> Стан боєголовки: {Math.Round(PluginAPI.Core.Warhead.DetonationTime)} </size></align>\n";
                } else if (Exiled.API.Features.Warhead.IsLocked) {
                    AlphaWarhead_HUD = "<color=#808080><size=23><align=right> Стан боєголовки: <color=red> заблокированно </align></size></color>\n";
                } else if (!Exiled.API.Features.Warhead.IsLocked) {
                    AlphaWarhead_HUD = "<color=#808080><size=23><align=right> Стан боєголовки:<color=#02f723> Готова </align></size></color>\n";
                } else {
                    AlphaWarhead_HUD = "<color=#808080><size=23><align=right> Стан боєголовки:<color=#f7db02> Сдетанированна </align></size></color>\n";
                }
                Spawn_T = $"<color=#00ff08><size=23> Ви з'явитесь за: {RespawnTokensManager.DominatingTeam} </size></color>\n";
                Spawn_W = $"<color=#00ff08><size=23> Ви з'явитеся через: {RespawnManager.Singleton.TimeTillRespawn.ToString("D2")} </size></color>\n";
                var elementReference = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(RoundTime_HUD, 760, TimeSpan.FromSeconds(2), elementReference);

                var elementReference_0 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(AlphaWarhead_HUD, 800, TimeSpan.FromSeconds(2), elementReference_0);
                
                var elementReference_1 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(Spawn_T, 40, TimeSpan.FromSeconds(2), elementReference_1);

                var elementReference_2 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp(Spawn_W, 0, TimeSpan.FromSeconds(2), elementReference_2);

                var elementReference_3 = new TimedElemRef<SetElement>();
                //displayCore.SetElemTemp("Текст", 120, TimeSpan.FromSeconds(2), elementReference_3);

                Exiled.API.Features.Player best = API.API.Best_Player();
                var elementReference_4 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp($"<color={best.Role.Color.ToHex()}><size=23>Найкрашчий гравець: {best.Nickname}</size></color>\n", 80, TimeSpan.FromSeconds(2), elementReference_4);


                yield return Timing.WaitForSeconds(1);
                displayCore.RemoveReference(elementReference_0);
                displayCore.RemoveReference(elementReference);
                displayCore.RemoveReference(elementReference_1);
                displayCore.RemoveReference(elementReference_2);
                displayCore.RemoveReference(elementReference_3);
                displayCore.RemoveReference(elementReference_4);
            }
        }
        void OnDisable() {
            Timing.KillCoroutines($"{player.Id}");
        }
    }
    class Tutorial_HUD : MonoBehaviour {
        Exiled.API.Features.Player player;
        string GmodEnabled_HUD;
        string NoClip_HUD;
        string Chaos_HUD;
        string MTF_HUD;
        string Time_HUD;
        //RESULT
        string Mixed_HUD;
        string HUD_Result;
        void Start() {
            player = Exiled.API.Features.Player.Get(this.gameObject);
            if (player != null) {
                Timing.RunCoroutine(Update_HUD(), $"{player.Id}");
            }
        }
        IEnumerator<float> Update_HUD() {
            for (; ; ) {
                GmodEnabled_HUD = $"<voffset=-400><align=left><color={player.Role.Color.ToHex()}> Режим богу: {player.IsGodModeEnabled} </color></voffset></align>\n";
                Mixed_HUD = GmodEnabled_HUD + NoClip_HUD;
                HUD_Result = "" + Mixed_HUD;
                yield return Timing.WaitForSeconds(1);
            }
        }
        void OnDisable() {
            Timing.KillCoroutines($"{player.Id}");
        }
    }
}