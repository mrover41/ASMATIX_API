using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using MEC;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TestPlugin;
using UnityEngine;


[Exiled.API.Features.Attributes.CustomItem(ItemType.GunCOM15)]
public class Trangulizer : CustomWeapon {
    public override string Description { get; set; } = "Знешкоджує об'єкти";
    public override float Weight { get; set; } = 2f;
    public override string Name { get; set; } = "Транквілізатор";
    public override uint Id { get; set; } = 120;
    public override ItemType Type { get; set; } = ItemType.GunCOM15;
    public override float Damage { get; set; } = 0;
    public override byte ClipSize { get; set; } = 7;

    protected override void SubscribeEvents() {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.Shot += Sh;
        Exiled.Events.Handlers.Player.ChangedItem += Select_Info;
    }
    
    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Player.Shot -= Sh;
        Exiled.Events.Handlers.Player.ChangedItem -= Select_Info;
        base.UnsubscribeEvents();
    }
    void Select_Info(ChangedItemEventArgs ev) { 
        if (Check(ev.Item)) {
            ev.Player.Broadcast(4, "<b><color=#FCF7D9>Ви підібрали</color> <color=#DB633C>Транквілізатор</color></b>");
        }
    }
    void Sh(ShotEventArgs ev) {
        if (!Check(ev.Item)) {
            return;
        } if (ev.Target.IsGodModeEnabled) {
            return;
        } if (ev.Target.IsScp) {
            Timing.KillCoroutines("SCP_tr");
            switch (ev.Target.Role.Type) {
                case RoleTypeId.Scp173:
                    break;
                case RoleTypeId.Scp106:
                    Hitmarker.SendHitmarkerDirectly(ev.Player.ReferenceHub, 1.5f);
                    Timing.RunCoroutine(BDelay(ev.Target, 10, true), "SCP_tr");
                    ev.Target.EnableEffect(EffectType.Slowness, 25, 10);
                    ev.Target.EnableEffect(EffectType.Blinded, 255, 10);
                    break;
                case RoleTypeId.Scp049:
                    Hitmarker.SendHitmarkerDirectly(ev.Player.ReferenceHub, 1.5f);
                    Timing.RunCoroutine(BDelay(ev.Target, 10, true), "SCP_tr");
                    ev.Target.EnableEffect(EffectType.SinkHole, 255, 10);
                    ev.Target.EnableEffect(EffectType.AmnesiaVision, 255, 10);
                    ev.Target.EnableEffect(EffectType.Blinded, 255, 10);
                    break;
                case RoleTypeId.Scp096:
                    Hitmarker.SendHitmarkerDirectly(ev.Player.ReferenceHub, 1.5f);
                    Timing.RunCoroutine(BDelay(ev.Target, 10, true), "SCP_tr");
                    ev.Target.EnableEffect(EffectType.SinkHole, 255, 10);
                    ev.Target.EnableEffect(EffectType.Blinded, 255, 10);
                    break;
                case RoleTypeId.Scp3114:
                    Hitmarker.SendHitmarkerDirectly(ev.Player.ReferenceHub, 1.5f);
                    Timing.RunCoroutine(BDelay(ev.Target, 10, true), "SCP_tr");
                    ev.Target.EnableEffect(EffectType.Flashed, 255, 10);
                    ev.Target.EnableEffect(EffectType.Deafened, 255, 10);
                    ev.Target.EnableEffect(EffectType.SinkHole, 255, 10);
                    break;
                case RoleTypeId.Scp939:
                    Hitmarker.SendHitmarkerDirectly(ev.Player.ReferenceHub, 1.5f);
                    Timing.RunCoroutine(BDelay(ev.Target, 10, true), "SCP_tr");
                    ev.Target.EnableEffect(EffectType.Slowness, 40, 10);
                    ev.Target.EnableEffect(EffectType.AmnesiaVision, 200, 10);
                    break;
            }
        } else if (ev.Player.LeadingTeam != ev.Target.LeadingTeam) {
            Hitmarker.SendHitmarkerDirectly(ev.Player.ReferenceHub, 1.5f);
            if (Global.Player_Role.ContainsKey("035")) { 
                if (ev.Target == Global.Player_Role["035"]) {
                    Timing.RunCoroutine(BDelay(ev.Target, 15));
                    Timing.RunCoroutine(SCPDelay(ev.Player));
                } else {
                    Timing.RunCoroutine(Delay(ev.Target));
                }
            } else {
                Timing.RunCoroutine(Delay(ev.Target));
            }
        }
        ev.CanHurt = false;
    }


    private IEnumerator<float> BDelay(Exiled.API.Features.Player player, int s, bool isSCP = false) { 
        for (int i = s; i > 0; i--) {
            if (isSCP) {
                //player.Broadcast(10, $"<b><color=#FF3C36>Ви під впливом Транквілізатора ({i} сек.)");
            } else {
                //player.Broadcast(10, $"<b><color=#1A1A1A>Ви під впливом Транквілізатора ({i} сек.)");
            }
            yield return Timing.WaitForSeconds(1);
        }
    }

    private IEnumerator<float> Delay(Exiled.API.Features.Player player) {
        player.CurrentItem = null;
        player.Inventory.enabled = false;
        player.Scale = new Vector3(0.5f, 0.5f, 0.5f);
        player.IsGodModeEnabled = true;
        Ragdoll rg = Ragdoll.CreateAndSpawn(player.Role.Type, player.Nickname, "Немного помялся", player.Position, player.Rotation);
        player.EnableEffect(EffectType.Deafened);
        player.EnableEffect(EffectType.Invisible);
        player.EnableEffect(EffectType.Ensnared);
        player.EnableEffect(EffectType.Flashed);
        yield return Timing.WaitForSeconds(5);
        player.DisableEffect(EffectType.Deafened);
        player.DisableEffect(EffectType.Invisible);
        player.DisableEffect(EffectType.Ensnared);
        player.DisableEffect(EffectType.Flashed);
        player.Scale = new Vector3(1, 1, 1);
        player.Inventory.enabled = true;
        player.IsGodModeEnabled = false;
        rg.Destroy();
    }

    private IEnumerator<float> SCPDelay(Exiled.API.Features.Player player) {
        player.EnableEffect(EffectType.Flashed);
        player.EnableEffect(EffectType.SinkHole);
        yield return Timing.WaitForSeconds(4);
        player.DisableEffect(EffectType.Flashed);
        yield return Timing.WaitForSeconds(7);
        player.DisableAllEffects();
    }

    protected override void OnReloading(ReloadingWeaponEventArgs ev) {
        ev.IsAllowed = false;
        base.OnReloading(ev);
    }

    public override SpawnProperties SpawnProperties { get; set; } = null;

}