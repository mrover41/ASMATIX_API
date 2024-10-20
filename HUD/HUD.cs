using Discord;
using Exiled.API.Features;
using Exiled.CreditTags;
using Exiled.Loader;
using MEC;
using Mono.Cecil;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        //RESULT
        string Mixed_HUD;
        string HUD_Result;
        //d
        int size = 23;
        string[] Name = new string[] { "Охоронець", "Вчений", "Д-Клас", "Повстанець Хаосу", "МОГ" };
        void Start() {
            player = Exiled.API.Features.Player.Get(this.gameObject);
        }
        void Update() {
            if (player == null) {
                return;
            }
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) { 
                if (player.IsScp) {
                    SCPl += $" {player.Role.Type} |";
                } if (Global.Player_Role.ContainsKey("035")) { 
                    if (player == Global.Player_Role["035"]) {
                        SCPl += $" Scp035 |";
                    }
                } if (Global.Player_Role.ContainsKey("343")) { 
                    if (player == Global.Player_Role["343"]) {
                        SCPl += $" Scp343 |";
                    }
                }
            }
            HUD_Name = $"<voffset=-600><align=left><color={player.Role.Color.ToHex()}><size={size}>         〚⭐〛Ім'я: {player.Nickname} </size></color></voffset></align>\n";
            HUD_Role = $"<align=left><color={player.Role.Color.ToHex()}><size={size}>         〚👨‍〛Клас: {Role_Translste(player)} </size></color></align>\n";
            HUD_MyTeam_Player = $"<align=left><color={player.Role.Color.ToHex()}><size={size}>         〚🍪〛Cоюзникiв: {Exiled.API.Features.Player.List.Where(x => x.LeadingTeam == player.LeadingTeam).ToList().Count()} </size></color></align>\n";
            HUD_SCPs = $"<align=left><color={player.Role.Color.ToHex()}><size={size}>{SCPl} </size></color></align>";
            Generator_HUD = $"<align=left><color={player.Role.Color.ToHex()}><size={size}>         〚🧲〛Акт.Генераторів: {Generator.List.Where(x => x.IsActivating).Count()}</size></color></align>\n";
            Mixed_HUD = HUD_Name + HUD_Role + Generator_HUD + HUD_MyTeam_Player;
            /*if (Config.HUD_Donat_Players.Any(x => player.NetId == x)) {
                Mixed_HUD += SCPl;
            } else {
                Mixed_HUD += "<align=left><color={player.Role.Color.ToHex()}><size=26> Привелегия </size></color></align>";
            }*/
            HUD_Result = "" + Mixed_HUD;
            SCPl = $"         〚🎃〛Аномалії:";
            player.ShowHint(HUD_Result, 5);
        }
        string Role_Translste(Exiled.API.Features.Player player) { 
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
        }
    }
    class Ghost_HUD : MonoBehaviour {
        Exiled.API.Features.Player player;
        string AlphaWarhed_HUD;
        string RoundTime_HUD;
        string Spawn_Time;
        //RESULT
        string Mixed_HUD;
        string HUD_Result;
        void Start() {
            player = Exiled.API.Features.Player.Get(this.gameObject);
        }
        void Update() { 
            if (player == null) {
                return;
            }
            RoundTime_HUD = $"<voffset=-400><align=left><color=#00ff08> Час раунду:<color=#00634e> {PluginAPI.Core.Round.Duration.Minutes} минут </color></voffset></align>\n";
            if (PluginAPI.Core.Warhead.IsDetonationInProgress) {
                AlphaWarhed_HUD = $"<align=left><color=#808080> Состояние боеголовки: {Math.Round(PluginAPI.Core.Warhead.DetonationTime)} </align>\n";
            } else if (Exiled.API.Features.Warhead.IsLocked) {
                AlphaWarhed_HUD = "<align=left><color=#808080> Состояние боеголовки: <color=red> заблокированно </color></align>\n";
            } else if (!Exiled.API.Features.Warhead.IsLocked){
                AlphaWarhed_HUD = "<align=left><color=#808080> Состояние боеголовки:<color=#02f723> Готова </color></align>\n";
            } else {
                AlphaWarhed_HUD = "<align=left><color=#808080> Состояние боеголовки:<color=#f7db02> Сдетанированна </color></align>\n";
            }
            Mixed_HUD =  RoundTime_HUD + AlphaWarhed_HUD;
            HUD_Result = "" + Mixed_HUD;
            player.ShowHint(HUD_Result, 5);
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
        }
        void Update() { 
            GmodEnabled_HUD = $"<voffset=-400><align=left><color={player.Role.Color.ToHex()}> Режим богу: {player.IsGodModeEnabled} минут </color></voffset></align>\n";
            Mixed_HUD = GmodEnabled_HUD + NoClip_HUD;
            HUD_Result = "" + Mixed_HUD;
        }
    }
}
