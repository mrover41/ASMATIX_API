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
public class Trangulizer : CustomItem {
    public override string Description { get; set; } = "Знешкоджує об'єкти";
    public override float Weight { get; set; } = 2f;
    public override string Name { get; set; } = "Транквілізатор";
    public override uint Id { get; set; } = 120;
    public override ItemType Type { get; set; } = ItemType.GunCOM15;

    protected override void SubscribeEvents() {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.Shot += Sh;
        Exiled.Events.Handlers.Player.ReloadingWeapon += Reload;
        Exiled.Events.Handlers.Player.Shot += Pk;
    }
    
    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Player.Shot -= Sh;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= Reload;
        Exiled.Events.Handlers.Player.Shot -= Pk;
        base.UnsubscribeEvents();
    }

    void Sh(ShotEventArgs ev) {
        if (!Check(ev.Item)) {
            return;
        }
        ev.Damage = 0;
        if (ev.Target.IsScp) {
            Timing.RunCoroutine(SCPDelay(ev.Target));
        } else {
            if (ev.Player.Role.Team != ev.Target.Role.Team) {
                Timing.RunCoroutine(Delay(ev.Target));
            }
        }
    }

    void Pk(ShotEventArgs ev) { 
    }

    private IEnumerator<float> Delay(Player player) {
        player.CurrentItem = null;
        player.Scale = new Vector3(0.5f, 0.5f, 0.5f);
        Ragdoll rg = Ragdoll.CreateAndSpawn(player.Role.Type, player.Nickname, "Немного помялся", player.Position, player.Rotation);
        player.EnableEffect(EffectType.Deafened);
        player.EnableEffect(EffectType.Invisible);
        player.EnableEffect(EffectType.Ensnared);
        player.EnableEffect(EffectType.Flashed);
        yield return Timing.WaitForSeconds(12);
        player.DisableAllEffects();
        player.Scale = new Vector3(1, 1, 1);
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

    void Reload(ReloadingWeaponEventArgs ev) {
        if (Check(ev.Item)) { 
            ev.IsAllowed = false;
        }
    }

    //public override SpawnProperties SpawnProperties { get; set; } = null;

    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties() {
        Limit = 1,
        DynamicSpawnPoints = new List<DynamicSpawnPoint> {
            new DynamicSpawnPoint()
            {
                Location = SpawnLocationType.InsideGr18,
                Chance = 100
            }
        },
        StaticSpawnPoints = new List<StaticSpawnPoint> {
            new StaticSpawnPoint() {
                    Chance = 100,
                    Position = new UnityEngine.Vector3(0, 0, 0f), Name = "Транквілізатор"
            }
        }
    };
}