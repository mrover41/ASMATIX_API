﻿using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using UnityEngine;

namespace TestPlugin.HUD {
    internal class HUD_LOADER {
        public static void OnEnabled() {
            Exiled.Events.Handlers.Player.Spawned += Sp;
            Exiled.Events.Handlers.Player.Died += ChRole;
        }
        public static void OnDisabled() {
            Exiled.Events.Handlers.Player.Spawned -= Sp;
            Exiled.Events.Handlers.Player.Died -= ChRole;
        }
        static void Sp(SpawnedEventArgs ev) { 
            HudCh(ev.Player);
        }
        static void ChRole(DiedEventArgs ev) {
            DiedGhost(ev.Player);
        }
        static void HudCh(Player player) {
            if (player.GameObject.TryGetComponent<Human_HUD>(out var component)) {
                UnityEngine.Object.Destroy(component);
            } if (player.GameObject.TryGetComponent<Tutorial_HUD>(out var component0)) {
                UnityEngine.Object.Destroy(component0);
            } if (player.GameObject.TryGetComponent<SCP_HUD>(out var component1)) {
                UnityEngine.Object.Destroy(component1);
            } if (player.GameObject.TryGetComponent<Ghost_HUD>(out var component2)) {
                UnityEngine.Object.Destroy(component2);
            } if (player.GameObject.TryGetComponent<SCP035_HUD>(out var component3)) {
                UnityEngine.Object.Destroy(component3);
            }

            if (player.IsHuman) {
                player.GameObject.AddComponent<Human_HUD>();
            } if (player.IsScp && player.Role != RoleTypeId.Scp079) {
                player.GameObject.AddComponent<SCP_HUD>();
            } if (player.IsTutorial) {
                //player.GameObject.AddComponent<Tutorial_HUD>();
            }

            Timing.CallDelayed(2, () => Roles());
        }

        static void DiedGhost(Player player) {
            if (player.GameObject.TryGetComponent<Human_HUD>(out var component)) {
                UnityEngine.Object.Destroy(component);
            } if (player.GameObject.TryGetComponent<Tutorial_HUD>(out var component0)) {
                UnityEngine.Object.Destroy(component0);
            } if (player.GameObject.TryGetComponent<SCP_HUD>(out var component1)) {
                UnityEngine.Object.Destroy(component1);
            } if (player.GameObject.TryGetComponent<SCP035_HUD>(out var component2)) {
                UnityEngine.Object.Destroy(component2);
            }

            //player.GameObject.AddComponent<Ghost_HUD>();
        }
        static void Roles() { 
            if (Global.Player_Role.ContainsKey("035")) {
                if (Global.Player_Role["035"].GameObject.TryGetComponent<Human_HUD>(out var component_035_FIX)) {
                    UnityEngine.Object.Destroy(component_035_FIX);
                    Global.Player_Role["035"].GameObject.AddComponent<SCP035_HUD>();
                }
            }
        }
    }
}
