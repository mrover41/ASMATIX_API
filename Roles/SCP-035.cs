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
using UnityEngine;
using UnityEngine.Assertions.Must;
using VoiceChat;

public class SCP035 : CustomRole {
    public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    public override uint Id { get; set; } = 35;
    public override float SpawnChance { get; set; } = 0;
    public override int MaxHealth { get; set; } = 500;
    public override string Name { get; set; } = "Маска";
    public override string Description { get; set; } =
        "SCP-035";
    public override string CustomInfo { get; set; } = "SCP-035";
    public override List<string> Inventory { get; set; } = new List<string>() {
        $"{ItemType.Medkit}", $"{ItemType.Coin}", $"{ItemType.SCP500}", $"{ItemType.KeycardZoneManager}",
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
        Exiled.Events.Handlers.Player.ChangedItem += Select_Item;
        Exiled.Events.Handlers.Player.Hurting += Damage;
        Exiled.Events.Handlers.Scp3114.Strangling += _SCP3114;
        Exiled.Events.Handlers.Player.ActivatingGenerator += _Generator;
        Exiled.Events.Handlers.Player.ReceivingEffect += Ef;
        Exiled.Events.Handlers.Player.Handcuffing += Hc;
        Exiled.Events.Handlers.Player.PickingUpItem += Pk;
        base.SubscribeEvents();
    }
    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawn;
        Exiled.Events.Handlers.Player.Died -= OnDie;
        Exiled.Events.Handlers.Player.DroppingItem -= Dr;
        Exiled.Events.Handlers.Player.FlippingCoin -= Att;
        Exiled.Events.Handlers.Player.ChangedItem -= Select_Item;
        Exiled.Events.Handlers.Player.Hurting -= Damage;
        Exiled.Events.Handlers.Scp3114.Strangling -= _SCP3114;
        Exiled.Events.Handlers.Player.ActivatingGenerator -= _Generator;
        Exiled.Events.Handlers.Player.ReceivingEffect -= Ef;
        Exiled.Events.Handlers.Player.Handcuffing -= Hc;
        Exiled.Events.Handlers.Player.PickingUpItem -= Pk;
        base.UnsubscribeEvents();
    }
    void Pk(PickingUpItemEventArgs ev) { 
        if (Check(ev.Player)) { 
            switch(ev.Pickup.Type) { 
                case ItemType.MicroHID:
                    ev.IsAllowed = false;
                    break;
                case ItemType.SCP1853:
                    ev.IsAllowed = false;
                    break;
                case ItemType.AntiSCP207:
                    ev.IsAllowed = false;
                    break;
                case ItemType.SCP018:
                    ev.IsAllowed = false;
                    break;
            }
        }
    }
     void Hc(HandcuffingEventArgs ev) { 
        if (Check(ev.Target)) { 
            ev.IsAllowed = false;
        }
     }
    void Ef(ReceivingEffectEventArgs ev) {
        if (Check(ev.Player) && ev.Effect.GetEffectType() == EffectType.AntiScp207) { 
            ev.IsAllowed = false;
        }
    }
    void _SCP3114(StranglingEventArgs ev) { 
        if (Check(ev.Target)) { 
            ev.IsAllowed = false;
        }
    }
    void _Generator(ActivatingGeneratorEventArgs ev) { 
        if (Check(ev.Player)) { 
            ev.IsAllowed = false;
        }
    }
    void Damage(HurtingEventArgs ev) {
        if (Check(ev.Player)) { 
            switch (ev.DamageHandler.Type) {
                case DamageType.Scp106:
                    ev.IsAllowed = false;
                    break;
                case DamageType.Scp173:
                    ev.IsAllowed = false;
                    break;
                case DamageType.Scp939:
                    ev.IsAllowed = false;
                    break;
                case DamageType.Scp049:
                    ev.IsAllowed = false;
                    break;
                case DamageType.Scp096:
                    ev.IsAllowed = false;
                    break;
                case DamageType.Scp3114:
                    ev.IsAllowed = false;
                    break;
                case DamageType.Scp0492:
                    ev.IsAllowed = false;
                    break;
            }
        } else if (ev.Player.IsScp && Check(ev.Attacker)) { 
            ev.IsAllowed = false;
        }
    }
    void Select_Item(ChangedItemEventArgs ev) {
        if (Check(ev.Player)) { 
            if (ev.Item.Type == ItemType.Coin) {
                ev.Player.ShowHint("<color=#c7956b> Під час активації цієї здатності\n ви змушуєте гравця поруч із дверима\n відчинити їх за вашим бажанням, якщо у нього є карта доступу </color>");
            }
        }
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
                                    ev.Player.Broadcast(3, "<color=#AD4DFE> Гравець під Психічною Атакою </color>");
                                    player.Broadcast(3, "<color=#FF5E3F> Ви під впливом SCP-035 </color>");
                                    Global.Cd[0] = 120;
                                } else {
                                    ev.Player.Broadcast(3, "<color=#FF5E3F> У гравця немає відповідної карти для цих дверей </color>");
                                }
                            } else {
                                door.IsOpen = true;
                            }
                        }
                    }
                }
            }
        } else if (Check(ev.Player)) {
            ev.Player.ShowHint($"<color=#FF5E3F> > {Global.Cd[0]} < </color>");
        }
    }
    void Dr(DroppingItemEventArgs ev) { 
        if (Check(ev.Player) && ev.Item.Type == ItemType.Coin) {
            ev.IsAllowed = false;
        }
    }
    private void OnRoundStarted() {
        if (Exiled.API.Features.Player.List.Count() >= 8) {
            if (random.Next(0, 100) < 50) {
                Global.SCP035 = false;
                CustomRole.Get((uint)1).AddRole(Exiled.API.Features.Player.List.Where(x => x.IsScp)?.ToList().RandomItem());
            }
        }
    }
    void OnSpawn(SpawnedEventArgs ev) {
        if (Check(ev.Player)) {
            Timing.RunCoroutine(Sp());
        }
    }
    void OnDie(DiedEventArgs ev) { 
        if (Check(ev.Player)) {
            Cassie.Message("<size=0> scp - 0 35 has been containment PITCH_0.1 .G6 PITCH_0.5 <color=green> <size=25> SCP-035 СДЕРЖАН </size> </color>");
            Timing.KillCoroutines(035);
        }
    }
    private IEnumerator<float> Ef(int s, Player pl) {
        pl.EnableEffect(EffectType.Flashed);
        yield return Timing.WaitForSeconds(s);
        pl.DisableEffect(EffectType.Flashed);
    }
    private IEnumerator<float> Sp() {
        yield return Timing.WaitForSeconds(1);
        Cassie.Message("<size=0> SCP - 0 35 has PITCH_0.2 .G2 .G5 PITCH_1 containment room PITCH_1 conditions <color=green> <size=25> SСP-035 НАРУШИЛ УСЛОВИЯ СОДЕРЖАНИЯ </size> </color>");
        foreach (Player player in Player.List)
        {
            if (Check(player))
            {
                player.IsGodModeEnabled = false;
                player.MaxHealth = 500;
                player.Broadcast(5, "<color=#AD4DFE> Ви з'явилися як SCP-035 (Маска).\nВаше завдання – знищити всіх гравців та допомогти SCP, за винятком Бога </color>");
                Timing.RunCoroutine(API.Damage(player, 1f, 2), 035);
                player.Teleport(RoomType.HczNuke);
            }
        }
    }
}
