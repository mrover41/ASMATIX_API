using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp3114;
using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.EventArgs.Warhead;
using HarmonyLib;
using MapEditorReborn.API.Features;
using MEC;
using Mirror;
using PlayerRoles;
using RueI.Displays.Scheduling;
using RueI.Displays;
using RueI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;
using RueI.Extensions;


class SCP035 : MonoBehaviour {
    Player player;
    MapEditorReborn.API.Features.Objects.SchematicObject spawnedSchematic = ObjectSpawner.SpawnSchematic("scp035", new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), null, null, false);
    public static int Coin_CD = 0;
    void Start() {
        player = Player.Get(this.gameObject);
        if (player == null) {
            return;
        }
        Global.Player_Role.Add("035", player);
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
        spawnedSchematic = ObjectSpawner.SpawnSchematic("scp035mask", player.Transform.position, Quaternion.identity, null, null, false);
        spawnedSchematic.transform.SetParent(player.CameraTransform.transform);
        spawnedSchematic.transform.localPosition = new Vector3(-0.1f, 0f, 0.2f);
    }
    void OnDamage(HurtingEventArgs ev) { 
        if (ev.Player == player && ev.DamageHandler.Type.IsScp(true)) { 
            ev.IsAllowed = false;
        } if (ev.Attacker == player && ev.Player.IsScp) {
            ev.IsAllowed = false;
        }
    }
    void Update() {
        //spawnedSchematic.transform.position = player.Transform.position + new Vector3(0, 0.6f, 0);
        //spawnedSchematic.transform.rotation = player.CameraTransform.rotation * Quaternion.Euler(0, 90, 0);
        if (player == null) { 
            return;
        }
    }
    void Ch_Role(ChangingRoleEventArgs ev) {
        if (ev.Player == player && ev.NewRole == RoleTypeId.Spectator) {
            Global.Player_Role.Remove("035");
            Cassie.Message($"<size=0> scp - 0 35 has been containment <color=green> <size=25> ^^ </size> </color>");
            Timing.KillCoroutines(35);
            ev.Player.CustomInfo = string.Empty;
            Destroy(this);
        }
    }
    void _Coin(FlippingCoinEventArgs ev) {
        if (ev.Player == player && Coin_CD <= 0) { 
            foreach(Player pla in Player.List.Where(x => Vector3.Distance(x.Position, player.Position) <= 7 && x != player && !x.IsScp)) {
                pla.EnableEffect(EffectType.Slowness, 15, 4);
                pla.EnableEffect(EffectType.Deafened, 255, 4);
                ev.Player.Broadcast(2, "Поглинення Активовано");
                Coin_CD = 100;
                pla.Hurt(37);
                if (player.Health + 37 <= 500) {
                    player.Health += 37;
                } else {
                    player.Health = 500;
                }
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
                DisplayCore displayCore = DisplayCore.Get(player.ReferenceHub);
                var elementReference_1 = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp("<color=#c7956b> Краде у ближнього гравця 37 ХП\nСумується, якщо поруч більше 1 гравця </color>", 200, TimeSpan.FromSeconds(2), elementReference_1);
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
        if (ev.Target == player) {
            ev.IsAllowed = false;   
        }
    }
    void Cand(EatingScp330EventArgs ev) { 
        if (ev.Player == player) {
            ev.IsAllowed = false;
        }
    }

    void Generator(ActivatingGeneratorEventArgs ev) { 
        if (ev.Player == player) {
            ev.IsAllowed = false;
        }
    }
    void Voice(VoiceChattingEventArgs ev) { 
        if (ev.Player == player) {
            ev.VoiceModule.CurrentChannel = VoiceChat.VoiceChatChannel.ScpChat;
        }
    }

    void OnEnable() {
        Exiled.Events.Handlers.Player.ActivatingGenerator += Generator;
        Exiled.Events.Handlers.Scp330.EatingScp330 += Cand;
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
        //AudioPlayer.API.AudioController.SpawnDummy(0, "SCP-035 Controller");
        Exiled.Events.Handlers.Player.VoiceChatting += Voice;
    }
    void OnDisable() {
        Exiled.Events.Handlers.Player.ActivatingGenerator -= Generator;
        Exiled.Events.Handlers.Scp330.EatingScp330 -= Cand;
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
        //AudioPlayer.API.AudioController.DisconnectDummy(0);
        Exiled.Events.Handlers.Player.VoiceChatting -= Voice;
        spawnedSchematic.Destroy();
        Global.Player_Role.Remove("035");
        Timing.KillCoroutines("035");
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
        var elementReference = new TimedElemRef<SetElement>();
        DisplayCore displayCore = DisplayCore.Get(player.ReferenceHub);
        for (; ; ) {
            if (Coin_CD > 0) {
                Coin_CD--;
                displayCore = DisplayCore.Get(player.ReferenceHub);
                elementReference = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp($"<align=left><color=#ff008c><size={20}><b>       〚🎭〛Здiбнoсть: <color=#F9F1FF>{Coin_CD}</color></align></size>", 10, TimeSpan.FromSeconds(2), elementReference);
            } else {
                displayCore = DisplayCore.Get(player.ReferenceHub);
                elementReference = new TimedElemRef<SetElement>();
                displayCore.SetElemTemp($"<align=left><color=#ff008c><size={20}><b>       〚🎭〛Здiбнoсть: <color=#66F261>Готово</color></align></size>", 10, TimeSpan.FromSeconds(2), elementReference);
            }
            yield return Timing.WaitForSeconds(1);
            displayCore.RemoveReference(elementReference);
        }
    }
}
