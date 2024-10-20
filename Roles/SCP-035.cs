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
using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles;
using PluginAPI.Core.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UserSettings.VideoSettings;
using VoiceChat;
using VoiceChat.Networking;

/*public class SCP035 : CustomRole {
    public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    public override uint Id { get; set; } = 35;
    public override float SpawnChance { get; set; } = 0;
    public override int MaxHealth { get; set; } = 500;
    public override string Name { get; set; } = "Маска";
    public override string Description { get; set; } =
        "SCP-035";
    public override string CustomInfo { get; set; } = "SCP-035";
    public override List<string> Inventory { get; set; } = new List<string>() {
        $"{ItemType.Medkit}", $"{ItemType.Coin}", $"{ItemType.SCP500}",
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
    public static Dictionary<int, int> Cd = new Dictionary<int, int>() {
            { 0, 0 },
        };
    protected override void SubscribeEvents() {
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
        Exiled.Events.Handlers.Player.InteractingDoor += Door_Inter;
        Exiled.Events.Handlers.Scp096.AddingTarget += OnAddTarget;
        base.SubscribeEvents();
    }
    protected override void UnsubscribeEvents() {
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
        Exiled.Events.Handlers.Player.InteractingDoor -= Door_Inter;
        Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddTarget;
        base.UnsubscribeEvents();
    }
    void OnAddTarget(AddingTargetEventArgs ev) { 
        if (Check(ev.Target)) {
            ev.IsAllowed = false;
        }
    }
    void Door_Inter(InteractingDoorEventArgs ev) { 
        if (!Check(ev.Player)) {
            return;
        }
        if (ev.Door.KeycardPermissions == KeycardPermissions.Checkpoints) { 
            ev.Door.IsOpen = true;
        }
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
        if (ev.Player == null) {
            return;
        }
        if (Check(ev.Player)) { 
            if (ev.Item.Type == ItemType.Coin) {
                ev.Player.ShowHint("<color=#c7956b> Під час активації цієї здатності\n ви змушуєте гравця поруч із дверима\n відчинити їх за вашим бажанням, якщо у нього є карта доступу </color>");
            }
        }
    }
    void Att(FlippingCoinEventArgs ev) { 
        if (ev.Player == null) {
            return;
        }
        if (Check(ev.Player) && Cd[0] <= 0) { 
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
                                    Cd[0] = 120;
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
            ev.Player.ShowHint($"<color=#FF5E3F> > {Cd[0]} < </color>");
        }
    }
    void Dr(DroppingItemEventArgs ev) { 
        if (ev.Player == null) {
            return;
        }
        if (Check(ev.Player) && ev.Item.Type == ItemType.Coin) {
            ev.IsAllowed = false;
        }
    }
    void OnSpawn(SpawnedEventArgs ev) {
        if (ev.Player == null) {
            return;
        }
        Timing.CallDelayed(1, () => {
            Global.Player_Role.Add("035", ev.Player);
            Timing.RunCoroutine(Sp());
            Timing.RunCoroutine(Updater());
        });
    }
    void OnDie(DiedEventArgs ev) {
        if (ev.Player == null) {
            return;
        }
        if (ev.Player == Global.Player_Role["035"]) {
            Global.Player_Role["035"] = null;
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
        yield return Timing.WaitForSeconds(2);
        Cassie.Message("<size=0> SCP - 0 35 has PITCH_0.2 .G2 .G5 PITCH_1 containment room PITCH_1 conditions <color=green> <size=25> SСP-035 НАРУШИЛ УСЛОВИЯ СОДЕРЖАНИЯ </size> </color>");
        foreach (Player player in Player.List) {
            if (Check(player)) {
                player.IsGodModeEnabled = false;
                player.MaxHealth = 500;
                player.Broadcast(5, "<color=#AD4DFE> Ви з'явилися як SCP-035 (Маска).\nВаше завдання – знищити всіх гравців та допомогти SCP, за винятком Бога </color>");
                Timing.RunCoroutine(API.Damage(player, 1f, 2), 035);
                player.Teleport(RoomType.HczNuke);
            }
        }
    }
    public static IEnumerator<float> Updater()
    {
        for (; ; )
        {
            yield return Timing.WaitForSeconds(1);
            Cd[0]--;
        }
    }
}*/
class SCP035 : MonoBehaviour {
    Player player;
    public static int Coin_CD = 0;
    void Start() {
        player = Player.Get(this.gameObject);
        if (player == null) {
            return;
        }
        player.Role.Set(RoleTypeId.Tutorial);
        player.MaxHealth = 500;
        player.Health = 500;
        player.Teleport(RoomType.HczNuke);
        player.IsGodModeEnabled = false;
        Cassie.Message("<size=0> SCP - 0 35 has PITCH_0.2 .G2 .G5 PITCH_1 containment room PITCH_1 conditions <color=green> <size=25> ^^**^^ </size></color>");
        player.Broadcast(5, "<color=#AD4DFE> Ви з'явилися як SCP-035 (Маска).\nВаше завдання – знищити всіх гравців та допомогти SCP, за винятком Бога </color>");
        player.AddItem(ItemType.Medkit);
        player.AddItem(ItemType.Coin);
        player.AddItem(ItemType.SCP500);
        Global.Player_Role.Add("035", player);
        Timing.RunCoroutine(Updater(player), 35);
        Timing.RunCoroutine(Cd_Updater(player), 35);
    }
    void Update() {
        if (player == null) { 
            return;
        }
    }
    void Ch_Role(ChangingRoleEventArgs ev) {
        if (ev.Player == player && ev.NewRole == RoleTypeId.Spectator) {
            Global.Player_Role["035"] = null;
            Cassie.Message("<size=0> scp - 0 35 has been containment PITCH_0.1 .G6 PITCH_0.5 <color=green> <size=25> ^^ </size> </color>");
            Timing.KillCoroutines(35);
            Destroy(this);
        }
    }
    void _Coin(FlippingCoinEventArgs ev) {
        if (ev.Player == player && Coin_CD <= 0) { 
            foreach(Player pla in Player.List.Where(x => Vector3.Distance(x.Position, player.Position) <= 7 && x != player)) {
                player.EnableEffect(EffectType.Slowness, 15, 4);
                player.EnableEffect(EffectType.Deafened, 255, 4);
                ev.Player.Broadcast(2, "Поглинення Активовано");
                Coin_CD = 120;
                pla.Hurt(20);
                player.Health += 20 * Player.List.Where(x => Vector3.Distance(x.Position, player.Position) <= 7 && x != player).Count();
            }
        } else if (ev.Player == player && Coin_CD > 0) {
            player.ShowHint($">> {Coin_CD} <<");
        }
    }
    void Drop(DroppingItemEventArgs ev) { 
        if (ev.Player == player && ev.Item.Type == ItemType.Coin) {
            ev.IsAllowed = false;
        }
    }
    void Pk(PickingUpItemEventArgs ev) { 
        if (ev.Player == player) { 
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
                case ItemType.Coin:
                    ev.IsAllowed = false;
                    break;
            }
        }
    }
    void Select_Item(ChangedItemEventArgs ev) {
        if (ev.Player == null) {
            return;
        }
        if (ev.Player == player) { 
            if (ev.Item.Type == ItemType.Coin) {
                ev.Player.ShowHint("<color=#c7956b> Під час активації цієї здатності\n ви змушуєте гравця поруч із дверима\n відчинити їх за вашим бажанням, якщо у нього є карта доступу </color>");
            }
        }
    }
    void Door_Interact(InteractingDoorEventArgs ev) { 
        if (ev.Player == player && ev.Door.IsCheckpoint) { 
            ev.Door.IsOpen = true;
        }
    }
    void OnAddTarget(AddingTargetEventArgs ev) { 
        if (ev.Player == player) {
            ev.IsAllowed = false;
        }
    }
    void OnEnable() {
        Exiled.Events.Handlers.Player.DroppingItem += Drop;
        Exiled.Events.Handlers.Player.FlippingCoin += _Coin;
        Exiled.Events.Handlers.Player.ChangingRole += Ch_Role;
        Exiled.Events.Handlers.Player.PickingUpItem += Pk;
        Exiled.Events.Handlers.Player.ChangedItem += Select_Item;
        Exiled.Events.Handlers.Scp096.AddingTarget += OnAddTarget;
        Exiled.Events.Handlers.Player.InteractingDoor += Door_Interact;
    }
    void OnDisable() {
        Exiled.Events.Handlers.Player.DroppingItem -= Drop;
        Exiled.Events.Handlers.Player.FlippingCoin -= _Coin;
        Exiled.Events.Handlers.Player.ChangingRole -= Ch_Role;
        Exiled.Events.Handlers.Player.PickingUpItem -= Pk;
        Exiled.Events.Handlers.Player.ChangedItem -= Select_Item;
        Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddTarget;
        Exiled.Events.Handlers.Player.InteractingDoor -= Door_Interact;
    }
    public static IEnumerator<float> Updater(Player pl) {
        yield return Timing.WaitForSeconds(2f);
        for (; ; ) {
            yield return Timing.WaitForSeconds(0.7f);
            pl.Hurt(2, "Ловля ебалай");
        }
    }
    public static IEnumerator<float> Cd_Updater(Player pl) {
        yield return Timing.WaitForSeconds(1f);
        for (; ; ) {
            yield return Timing.WaitForSeconds(1);
            if (Coin_CD > 0) {
                Coin_CD--;
            }
        }
    }
}
