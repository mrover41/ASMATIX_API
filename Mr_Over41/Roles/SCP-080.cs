using Exiled.API.Features;
using UnityEngine;
using PlayerRoles;
using Exiled.Events.EventArgs.Player;
using System.Collections.Generic;
using System.Linq;
using MEC;
using Exiled.API.Enums;
using Exiled.API.Extensions;

namespace TestPlugin.Roles {
    class SCP080 : MonoBehaviour {
        Player player;
        Dictionary<Player, float> player0 = new Dictionary<Player, float>();
        float Cd;
        float maxEffect = 60;
        bool isSpidozni = false;
        void Start() {
            player = Player.Get(this.gameObject);
            if (player == null) {
                Log.Error($"[ASMATIX_API] Error in 'SCP080', player = null, player: {player}");
                return; 
            }
            Global.Player_Role.Add("080", player);
            player.Role.Set(RoleTypeId.Tutorial);
            player.CustomInfo = "SCP-080";
            player.AddItem(ItemType.Coin);
            Timing.RunCoroutine(Updater());
            player.IsGodModeEnabled = false;
            player.RankColor = new Color(255, 0, 0).ToHex();
        }
        void Update() { 
            foreach (Player player1 in Player.List.Where(x => !x.IsScp && x != player)) { 
                if (Vector3.Distance(player1.Position, player.Position) < 20) { 
                    if (!player0.ContainsKey(player1)) {
                        player0.Add(player1, 0);
                    }
                } else { 
                    if (player0.ContainsKey(player1)) { 
                        player0.Remove(player1);
                    }
                }
            }
        }
        void FlippCoin(FlippingCoinEventArgs ev) {
            if (ev.Player != player) return;
            foreach (Player pl in player0.Keys.ToList()) {
                if (API.API.CheckCustumEffect(pl, Data.Enums.CustomEffect.Sleep)) {
                    pl.Scale = new Vector3(1, 1, 1);
                    pl.Inventory.enabled = true;
                    pl.Kill("Уснул на завжди");
                }
            }
        }
        void _Alt(TogglingNoClipEventArgs ev) {
            if (ev.Player != player) return;
            if (Cd <= 0) {
                player.CurrentRoom.Blackout(20);
                Timing.CallDelayed(1, () => player.CurrentRoom.SetRoomLightsForTargetOnly(player, true));
                player.EnableEffect(EffectType.Invisible, 255, 20);
                player.EnableEffect(EffectType.MovementBoost, 100, 20);
                player.Broadcast(2, "Свiтло вимкненно");
                isSpidozni = true;
                Timing.CallDelayed(60, () => isSpidozni = false);
                Cd = 10;
            }
        }
        void DropItem(DroppingItemEventArgs ev) {
            if (ev.Player != player) return;
            ev.IsAllowed = false;
        }
        void PkItem(PickingUpItemEventArgs ev) {
            if (ev.Player != player) return;
            ev.IsAllowed = false;
        }
        void Die(DiedEventArgs ev) {
            if (ev.Player != player) return;
            Destroy(this);
        }
        void OnEnable() {
            Exiled.Events.Handlers.Player.Died += Die;
            Exiled.Events.Handlers.Player.FlippingCoin += FlippCoin;
            Exiled.Events.Handlers.Player.PickingUpItem += PkItem;
            Exiled.Events.Handlers.Player.DroppingItem += DropItem;
            Exiled.Events.Handlers.Player.TogglingNoClip += _Alt;
        }
        void OnDisable() {
            Exiled.Events.Handlers.Player.Died -= Die;
            Exiled.Events.Handlers.Player.FlippingCoin -= FlippCoin;
            Exiled.Events.Handlers.Player.PickingUpItem -= PkItem;
            Exiled.Events.Handlers.Player.DroppingItem -= DropItem;
            Exiled.Events.Handlers.Player.TogglingNoClip -= _Alt;
            Global.Player_Role.Remove("080");
            player.CustomInfo = null;
        }
        IEnumerator<float> Updater() {
            for (;;) { 
                foreach(Player pl in player0.Keys.ToList()) {
                    if (player0[pl] < maxEffect) { 
                        if (!isSpidozni) {
                            player0[pl]++;
                        } else {
                            player0[pl] += 5;
                        }
                    }
                    pl.EnableEffect(EffectType.Slowness, 20, 1);
                    Human_HUD pizdek = pl.GameObject.GetComponent<Human_HUD>();
                    pizdek.sl = player0[pl];
                    pizdek.maxSl = maxEffect;
                    if (player0[pl] >= maxEffect) {
                        if (!API.API.CheckCustumEffect(pl, Data.Enums.CustomEffect.Sleep)) { 
                            API.API.EnableCustumEffect(pl, Data.Enums.CustomEffect.Sleep);
                        }
                    } else {
                        if (API.API.CheckCustumEffect(pl, Data.Enums.CustomEffect.Sleep)) { 
                            API.API.DisableCustumEffect(pl, Data.Enums.CustomEffect.Sleep);
                        }
                    }
                }
                if (Cd > 0) { 
                    Cd--;
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
