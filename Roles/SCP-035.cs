using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.Patches;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp3114;
using InventorySystem;
using MEC;
using PlayerRoles;
using PluginAPI.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using TestPlugin.ASMATIX_API;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VoiceChat;

public class SCP035 : CustomRole {
    public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    public override uint Id { get; set; } = 1;
    public override float SpawnChance { get; set; } = 0;
    public override int MaxHealth { get; set; } = 500;
    public override string Name { get; set; } = "Маска";
    public override string Description { get; set; } =
        "SCP-035";
    public override string CustomInfo { get; set; } = "SCP-035";
    public override List<string> Inventory { get; set; } = new List<string>() {
        $"{ItemType.Medkit}", $"{ItemType.Coin}"
    };
    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
    {
        Limit = 1,
        RoleSpawnPoints = new List<RoleSpawnPoint> {
            new RoleSpawnPoint() {
                Role = RoleTypeId.Scientist,
                Chance = 0,
            }
        }
    };
    System.Random random = new System.Random();
    protected override void SubscribeEvents() {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.Spawned += OnSpawn;
        Exiled.Events.Handlers.Player.Died += OnDie;
        Exiled.Events.Handlers.Player.DroppingItem += Dr;
        Exiled.Events.Handlers.Player.FlippingCoin += Att;
        base.SubscribeEvents();
    }
    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawn;
        Exiled.Events.Handlers.Player.Died -= OnDie;
        Exiled.Events.Handlers.Player.DroppingItem -= Dr;
        Exiled.Events.Handlers.Player.FlippingCoin -= Att;
        base.UnsubscribeEvents();
    }
    void Att(FlippingCoinEventArgs ev) { 
        if (Check(ev.Player) && Global.Cd[0] <= 0) { 
            foreach (Player player in Player.List) { 
                if (Vector3.Distance(ev.Player.Position, player.Position) <= 4 && ev.Player.NetId != player.NetId) {
                    foreach (Door door in Door.List) { 
                        if (Vector3.Distance(player.Position, door.Position) <= 4) {
                            if (door.KeycardPermissions != KeycardPermissions.None) {
                                if (player.Items.Any(item => item is Keycard keycard && keycard.Permissions > door.KeycardPermissions)) {
                                    door.IsOpen = true;
                                    Timing.RunCoroutine(Ef(2, player));
                                    ev.Player.Broadcast(5, "<color=#AD4DFE> Ви атакували гравця </color>");
                                    player.Broadcast(5, "<color=#FF5E3F> Ви під впливом SCP-035 </color>");
                                    Global.Cd[0] = 120;
                                }
                            } else {
                                door.IsOpen = true;
                            }
                        }
                    }
                }
            }
        } else if (Check(ev.Player)) {
            ev.Player.Broadcast(2, $"{Global.Cd[0]}");
        }
    }
    void Dr(DroppingItemEventArgs ev) { 
        if (Check(ev.Player) && ev.Item.Type == ItemType.Coin) {
            ev.IsAllowed = false;
        }
    }
    private void OnRoundStarted() {
        Global.Cd[0] = 0;
        if (Exiled.API.Features.Player.List.Count() >= 8) {
            if (random.Next(0, 10) < 75) {
                CustomRole.Get((uint)1).AddRole(Exiled.API.Features.Player.List.Where(x => x.IsScp)?.ToList().RandomItem());
            }
        }
        Timing.RunCoroutine(Sp());
    }
    void OnSpawn(SpawnedEventArgs ev) {
    }
    void OnDie(DiedEventArgs ev) { 
        if (Check(ev.Player)) {
            Timing.KillCoroutines(Global.coroutine);
            Cassie.Announcer.SendMessage("<size=0> scp - 035 has been died <color=green> <size=25> SCP 035 УНИЧТОЖЕН </size> </color>");
        }
    }
    private IEnumerator<float> Ef(int s, Player pl)
    {
        pl.EnableEffect(EffectType.Flashed);
        yield return Timing.WaitForSeconds(s);
        pl.DisableEffect(EffectType.Flashed);
    }
    private IEnumerator<float> Updater() {
        for (; ; ) {
            yield return Timing.WaitForSeconds(1);
            Global.Cd[0]--;
        }
    }
    private IEnumerator<float> Sp() {
        yield return Timing.WaitForSeconds(4);
        foreach (Player player in Player.List)
        {
            if (Check(player))
            {
                player.IsGodModeEnabled = false;
                player.MaxHealth = 500;
                player.Broadcast(5, "<color=#AD4DFE> Ви з'явилися як SCP-035 (Маска).\nВаше завдання – знищити всіх гравців та допомогти SCP, за винятком Бога </color>");
                Global.coroutine = Timing.RunCoroutine(API.Damage(player, 1, 2));
                Timing.RunCoroutine(Updater());
                player.Teleport(RoomType.HczNuke);
            }
        }
    }
}
