using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

namespace TestPlugin.HUD {
    internal class HUD_LOADER {
        public static void OnEnabled() {
            Exiled.Events.Handlers.Player.Spawned += Spawn;
            Exiled.Events.Handlers.Player.Died += _Died;
        }
        public static void OnDisabled() {
            Exiled.Events.Handlers.Player.Spawned -= Spawn;
            Exiled.Events.Handlers.Player.Died -= _Died;
        }
        static void Spawn(SpawnedEventArgs ev) {
            if (ev.Player == null) {
                return;
            } if (Round.IsLobby) {
                return;
            }
            Human_HUD human_HUD = ev.Player.GameObject.GetComponent<Human_HUD>();
            Ghost_HUD ghost_HUD = ev.Player.GameObject.GetComponent<Ghost_HUD>();
            Tutorial_HUD tutorial_HUD = ev.Player.GameObject.GetComponent<Tutorial_HUD>();
            if (human_HUD != null) {
                MonoBehaviour.Destroy(human_HUD);
            } if (ghost_HUD != null) { 
                MonoBehaviour.Destroy(ghost_HUD);
            } if (tutorial_HUD != null) { 
                MonoBehaviour.Destroy(tutorial_HUD);
            }

            if (ev.Player.IsHuman && ev.Player.Role.Type != RoleTypeId.Tutorial) { 
                ev.Player.GameObject.AddComponent<Human_HUD>();
            } if (ev.Player.Role.Type == RoleTypeId.Tutorial) {
                ev.Player.GameObject.AddComponent<Tutorial_HUD>();
            }
        }
        public static void _Died(DiedEventArgs ev) { 
            Human_HUD human_HUD = ev.Player.GameObject.GetComponent<Human_HUD>();
            Ghost_HUD ghost_HUD = ev.Player.GameObject.GetComponent<Ghost_HUD>();
            if (human_HUD != null) { 
                MonoBehaviour.Destroy(human_HUD);
            } if (ghost_HUD == null) {
                ev.Player.GameObject.AddComponent<Ghost_HUD>();
            }
        }
    }
}
