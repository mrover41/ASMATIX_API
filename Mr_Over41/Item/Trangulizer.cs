using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerRoles.Ragdolls;
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
            Timing.RunCoroutine(SCPDelay(ev.Target));
        } else if (ev.Player.LeadingTeam != ev.Target.LeadingTeam) {
            Timing.RunCoroutine(Delay(ev.Target));
        }
        ev.Damage = 0;
    }


    private IEnumerator<float> Delay(Player player) {
        player.CurrentItem = null;
        player.Inventory.enabled = false;
        player.Scale = new Vector3(0.5f, 0.5f, 0.5f);
        player.IsGodModeEnabled = true;
        Ragdoll rg = Ragdoll.CreateAndSpawn(player.Role.Type, player.Nickname, "Немного помялся", player.Position, player.Rotation);
        player.EnableEffect(EffectType.Deafened);
        player.EnableEffect(EffectType.Invisible);
        player.EnableEffect(EffectType.Ensnared);
        player.EnableEffect(EffectType.Flashed);
        yield return Timing.WaitForSeconds(12);
        player.DisableEffect(EffectType.Deafened);
        player.DisableEffect(EffectType.Invisible);
        player.DisableEffect(EffectType.Ensnared);
        player.DisableEffect(EffectType.Flashed);
        player.Scale = new Vector3(1, 1, 1);
        player.Inventory.enabled = true;
        player.IsGodModeEnabled = false;
        rg.Destroy();
    }

    private IEnumerator<float> SCPDelay(Player player) {
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