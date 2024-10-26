using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp3114;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;


class SCP035 : MonoBehaviour {
    Player player;
    public static int Coin_CD = 0;
    void Start() {
        player = Player.Get(this.gameObject);
        if (player == null) {
            return;
        }
        player.CustomInfo = "SCP-035";
        Timing.RunCoroutine(Updater(player), 35);
        Timing.RunCoroutine(Cd_Updater(), 35);
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
        player.AddItem(ItemType.KeycardZoneManager);
        Global.Player_Role.Add("035", player);
    }
    void OnDamage(HurtingEventArgs ev) { 
        if (ev.Player == player && ev.DamageHandler.Type.IsScp(true)) { 
            ev.IsAllowed = false;
        } if (ev.Attacker == player && ev.Player.IsScp) {
            ev.IsAllowed = false;
        }
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
            ev.Player.CustomInfo = string.Empty;
            Destroy(this);
        }
    }
    void _Coin(FlippingCoinEventArgs ev) {
        if (ev.Player == player && Coin_CD <= 0) { 
            foreach(Player pla in Player.List.Where(x => Vector3.Distance(x.Position, player.Position) <= 7 && x != player)) {
                pla.EnableEffect(EffectType.Slowness, 15, 4);
                pla.EnableEffect(EffectType.Deafened, 255, 4);
                ev.Player.Broadcast(2, "Поглинення Активовано");
                Coin_CD = 100;
                pla.Hurt(37);
                player.Health += 37;
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
                case ItemType.SCP330:
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
                ev.Player.ShowHint("<color=#c7956b> Ця здібность дає змогу вкрасти у ближнього гравця 37 хп\n (Примітка: Здатність підсумовується, якщо гравці, що стоять поруч із маскою (SCP 035), більше, ніж 1 людина). </color>");
            }
        }
    }
    void Door_Interact(InteractingDoorEventArgs ev) { 
        /*if (ev.Player == player && ev.Door.IsCheckpoint) {
            ev.Door.IsOpen = true;
        }*/
    }
    void OnAddTarget(AddingTargetEventArgs ev) { 
        if (ev.Player == player) {
            ev.IsAllowed = false;
        }
    }
    void HCSCP3114(StranglingEventArgs ev) { 
        if (ev.Target == player) {
            ev.IsAllowed = false;
        }
    }
    void Cf(HandcuffingEventArgs ev) { 
        if (ev.Player == player) {
            ev.IsAllowed = false;   
        }
    }
    void OnEnable() {
        Exiled.Events.Handlers.Player.Handcuffing += Cf;
        Exiled.Events.Handlers.Scp3114.Strangling += HCSCP3114;
        Exiled.Events.Handlers.Player.DroppingItem += Drop;
        Exiled.Events.Handlers.Player.FlippingCoin += _Coin;
        Exiled.Events.Handlers.Player.ChangingRole += Ch_Role;
        Exiled.Events.Handlers.Player.PickingUpItem += Pk;
        Exiled.Events.Handlers.Player.ChangedItem += Select_Item;
        Exiled.Events.Handlers.Scp096.AddingTarget += OnAddTarget;
        Exiled.Events.Handlers.Player.InteractingDoor += Door_Interact;
        Exiled.Events.Handlers.Player.Hurting += OnDamage;
    }
    void OnDisable() {
        Exiled.Events.Handlers.Player.Handcuffing -= Cf;
        Exiled.Events.Handlers.Scp3114.Strangling -= HCSCP3114;
        Exiled.Events.Handlers.Player.DroppingItem -= Drop;
        Exiled.Events.Handlers.Player.FlippingCoin -= _Coin;
        Exiled.Events.Handlers.Player.ChangingRole -= Ch_Role;
        Exiled.Events.Handlers.Player.PickingUpItem -= Pk;
        Exiled.Events.Handlers.Player.ChangedItem -= Select_Item;
        Exiled.Events.Handlers.Scp096.AddingTarget -= OnAddTarget;
        Exiled.Events.Handlers.Player.InteractingDoor -= Door_Interact;
        Exiled.Events.Handlers.Player.Hurting -= OnDamage;
    }
    IEnumerator<float> Updater(Player pl) {
        yield return Timing.WaitForSeconds(2f);
        for (; ; ) {
            yield return Timing.WaitForSeconds(1f);
            pl.Hurt(2, "Ловля ебалай");
        }
    }
    IEnumerator<float> Cd_Updater() {
        yield return Timing.WaitForSeconds(1f);
        for (; ; ) {
            yield return Timing.WaitForSeconds(1);
            if (Coin_CD > 0) {
                Coin_CD--;
            }
        }
    }
}
