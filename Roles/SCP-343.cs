using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp3114;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;
using VoiceChat;
public class Good : CustomRole {
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
    public override uint Id { get; set; } = 343;
    public override float SpawnChance { get; set; } = 0;
    public override int MaxHealth { get; set; } = 2000000000;
    public override string Name { get; set; } = "Бог";
    public override string Description { get; set; } =
        "Ви можете допомагати людям ;)";
    public override string CustomInfo { get; set; } = "SCP-343";
    public override List<string> Inventory { get; set; } = new List<string>() {
        $"{ItemType.Medkit}", $"{ItemType.SCP1853}", $"{ItemType.Coin}", $"{ItemType.Adrenaline}", $"{ItemType.SCP2176}"
    };

    System.Random random = new System.Random();

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

    protected override void SubscribeEvents() {
        Timing.RunCoroutine(Delay());
        Exiled.Events.Handlers.Player.UsingItemCompleted += Us;
        Exiled.Events.Handlers.Player.Spawned += Sp;
        Exiled.Events.Handlers.Player.PickingUpItem += Pk;
        Exiled.Events.Handlers.Player.FlippingCoin += Uk;
        Exiled.Events.Handlers.Player.DroppingItem += Dr;
        Exiled.Events.Handlers.Player.Hurting += Gd;
        Exiled.Events.Handlers.Player.Handcuffing += Hc;
        Exiled.Events.Handlers.Player.Escaping += Rol;
        Exiled.Events.Handlers.Map.Decontaminating += Decon;
        Exiled.Events.Handlers.Scp096.AddingTarget += SCP096;
        Exiled.Events.Handlers.Player.ActivatingGenerator += _Generator;
        Exiled.Events.Handlers.Map.ExplodingGrenade += Explore_Granate;
        Exiled.Events.Handlers.Scp3114.Strangling += _SCP3114;
        Exiled.Events.Handlers.Player.ChangingItem += Select_Item;
        Exiled.Events.Handlers.Warhead.Detonated += Aw;
        Exiled.Events.Handlers.Player.ActivatingWarheadPanel += Wp;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Player.UsingItemCompleted -= Us;
        Exiled.Events.Handlers.Player.DroppingItem += Dr;
        Exiled.Events.Handlers.Player.Spawned -= Sp;
        Exiled.Events.Handlers.Player.PickingUpItem -= Pk;
        Exiled.Events.Handlers.Player.FlippingCoin -= Uk;
        Exiled.Events.Handlers.Player.Hurting -= Gd;
        Exiled.Events.Handlers.Player.Handcuffing -= Hc;
        Exiled.Events.Handlers.Player.Escaping -= Rol;
        Exiled.Events.Handlers.Map.Decontaminating -= Decon;
        Exiled.Events.Handlers.Scp096.AddingTarget -= SCP096;
        Exiled.Events.Handlers.Player.ActivatingGenerator -= _Generator;
        Exiled.Events.Handlers.Map.ExplodingGrenade -= Explore_Granate;
        Exiled.Events.Handlers.Scp3114.Strangling -= _SCP3114;
        Exiled.Events.Handlers.Player.ChangingItem -= Select_Item;
        Exiled.Events.Handlers.Warhead.Detonated -= Aw;
        Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= Wp;
        base.UnsubscribeEvents();
    }

    bool lzs = true;
    string HUD;
    string HUD_0;
    string HUD_1;
    string HUD_D;
    CoroutineHandle _Pass;
    CoroutineHandle _HUD;

    void Wp(ActivatingWarheadPanelEventArgs ev) { 
        if (Check(ev.Player)) {
            ev.IsAllowed = false;
        }
    }
    void Aw() {
        foreach (Player player in Player.List) { 
            if (Check(player)) {
                player.Kill(DamageType.Bleeding);
                Timing.KillCoroutines(_Pass);
                Timing.KillCoroutines(343);
                player.ShowHint(String.Empty);
            }
        }
    }
    void Select_Item(ChangingItemEventArgs ev) { 
        if (Check(ev.Player)) {
            switch (ev.Item.Type) {
                case ItemType.Coin:
                    //HUD_1 = "<color=#c7956b> При підкиданні монети \n гравцям поблизу видається безсмертя на невеликий час </color> \n";
                    ev.Player.ShowHint("<color=#c7956b> При підкиданні монети \n гравцям поблизу видається безсмертя на невеликий час </color> \n", 3);
                    break;
                case ItemType.Medkit:
                    //HUD_1 = "<color=#c7956b> Лікує гравців поряд </color> \n";
                    ev.Player.ShowHint("<color=#c7956b> Лікує гравців поряд </color> \n", 3);
                    break;
                case ItemType.Adrenaline:
                    ev.Player.ShowHint("<color=#c7956b> Дає можливість літати на короткий час </color> \n", 3);
                    break;
                case ItemType.SCP2176:
                    ev.Player.ShowHint("<color=#c7956b> Осліплює та уповільнює SCP-об'єкти в зоні кімнати </color> \n", 3);
                    break;
                case ItemType.SCP1853:
                    ev.Player.ShowHint("<color=#c7956b> Переміщує вас у випадкове місце </color> \n", 3);
                    break;
            }
        }
    }
    void _SCP3114(StranglingEventArgs ev) {
        if (Check(ev.Target)) {
            ev.IsAllowed = false;
        }
    }
    void Explore_Granate(ExplodingGrenadeEventArgs ev) {
        if (Check(ev.Player)) {
            Manager.it[4] = 120;
            ev.IsAllowed = false;
            foreach (Door dor in ev.Projectile.Room.Doors) {
                if (!dor.IsElevator) {
                    dor.IsOpen = true;
                }
            }
            Timing.RunCoroutine(Boom(Manager.it[4], ev.Player));
            ev.Player.Broadcast(5, $"<color=#FF5E3F> Ви зможете використовувати знову через {Manager.it[4]} секунд </color>");
            foreach (Player player in ev.Projectile.Room.Players) { 
                if (player.IsScp) {
                    Timing.RunCoroutine(Ef(10, player));
                }
            }
        }
    }
    void _Generator(ActivatingGeneratorEventArgs ev) { 
        if (Check(ev.Player)) {
            ev.IsAllowed = false;
        }
    }
    void SCP096(AddingTargetEventArgs ev) {
        if (Check(ev.Player)) {
            ev.IsAllowed = false;
        }
    }
    void Decon(DecontaminatingEventArgs ev) {
        lzs = false;
    }
    
    void Rol(EscapingEventArgs ev) { 
        if (Check(ev.Player)) {
            ev.IsAllowed = false;
        }
    }
    void Hc(HandcuffingEventArgs ev) {
        if (Check(ev.Target)) { 
            ev.IsAllowed = false;
        }
    }
    void Gd(HurtingEventArgs ev) { 
        if(Check(ev.Player)) {
            ev.IsAllowed = false;
        }
    }

    void Pk(PickingUpItemEventArgs ev) {
        if (Check(ev.Player)) {
            ev.IsAllowed = false;
        }
    }
    void Uk(FlippingCoinEventArgs ev) {
        if (Check(ev.Player)) {
            if (ev.Item.Type == ItemType.Coin) {
                if (Manager.it[0] <= 0) {
                    Manager.it[0] = 110;
                    ev.Player.Broadcast(5, "<color=#FFC745> Ви видали безсмертя гравцям </color>");
                    foreach (Player player in Player.List) {
                        if (Vector3.Distance(ev.Player.Position, player.Position) < 7) {
                            if (!player.IsScp && !Check(player) && player.Role != RoleTypeId.Tutorial) {
                                Timing.RunCoroutine(Hl(player));
                            }
                        }
                    }
                } else {
                    ev.Player.ShowHint($"<color=#FF5E3F> > {Manager.it[0]} < </color>", 3);
                }
            }
        }
    }

    void Us(UsingItemCompletedEventArgs ev) { 
        if (!Check(ev.Player)) {
            return;
        }
        if (ev.Item.Type == ItemType.Medkit) {
            if (Manager.it[2] <= 0) {
                Manager.it[2] = 60;
                ev.Player.Broadcast(5, "<color=#FFC745> Ви лікуєте гравців поряд </color>");
                foreach (Player player in Player.List) {
                    Timing.RunCoroutine(_Heal(player, 13, 0.4f, ev));
                }
            } else {
                ev.Player.ShowHint($"<color=#FF5E3F> > {Manager.it[2]} < </color>", 3);
            }
        } else if (ev.Item.Type == ItemType.SCP1853) {
            if (Manager.it.ContainsKey(1)) {
               if (Manager.it[1] <= 0) {
                    Manager.it[1] = 1;
                    List<Room> rooms = Room.List.ToList();
                    Room tp_room = rooms[random.Next(0, Room.List.Count)];
                    if (lzs) {
                        ev.Player.Teleport(tp_room);
                    } else { 
                        while (tp_room.Zone == ZoneType.LightContainment) {
                            tp_room = rooms[random.Next(0, Room.List.Count)];
                        }
                        ev.Player.Teleport(tp_room);
                    }
                    ev.Player.DisableEffect(EffectType.Scp1853);
               } else {
                    ev.Player.ShowHint($"<color=#FF5E3F> > {Manager.it[1]} < </color>", 3);
               }
            }
        } else if (ev.Item.Type == ItemType.Adrenaline) { 
            if (Manager.it[3] <= 0) {
                Manager.it[3] = 90;
                Timing.RunCoroutine(NoClip(ev.Player));
                ev.Player.DisableAllEffects();
                ev.Player.EnableEffect(EffectType.Ghostly);
            } else {
                ev.Player.ShowHint($"<color=#FF5E3F> > {Manager.it[3]} < </color>", 3);
            }
        }
        if (ev.Item.IsUsable) { 
            ev.Player.AddItem(ev.Item.Type);
        }
    }
    void Dr(DroppingItemEventArgs ev) {
        if (!Check(ev.Player)) {
            return;
        }
        ev.IsAllowed = false;
    }
    void Sp(SpawnedEventArgs ev) {
        if (Check(ev.Player)) {
            Global.Player_Role.Add("343", ev.Player);
            ev.Player.Teleport(DoorType.Scp173Armory);
            Round.IgnoredPlayers.Add(ev.Player.ReferenceHub);
            _Pass = Timing.RunCoroutine(Pass(ev.Player));
            Timing.RunCoroutine(HUD_Render(0.5f, ev.Player, 2), 343);
            ev.Player.IsGodModeEnabled = true;
            ev.Player.EnableEffect(EffectType.Ghostly);
        }
    }

    private IEnumerator<float> Hl(Player player) {
        player.IsGodModeEnabled = true;
        player.EnableEffect(EffectType.Invigorated);
        yield return Timing.WaitForSeconds(5);
        player.DisableEffect(EffectType.Invigorated);
        player.IsGodModeEnabled = false;
    }

    private IEnumerator<float> NoClip(Player player) {
        player.IsNoclipPermitted = true;
        player.Broadcast(5, "<color=#FFC745> Ви можете літати </color>");
        yield return Timing.WaitForSeconds(10);
        player.IsNoclipPermitted = false;
    }
    private IEnumerator<float> Delay()
    {
        for (; ; ) {
            for (int i = 0; i <= 8; i++) {
                if (Manager.it[i] > 0) {
                    Manager.it[i]--;
                }
            }
            yield return Timing.WaitForSeconds(1);
        }
    }

    private IEnumerator<float> Pass(Player pl) {
        for (; ; ) {
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List) { 
                if (Vector3.Distance(pl.Position, player.Position) < 6) {
                    if (player.IsScp) {
                        if (Vector3.Distance(pl.Position, player.Position) < 4) {
                            player.EnableEffect(EffectType.Blinded, 255, 5);
                        }
                        player.EnableEffect(EffectType.AmnesiaVision, 255, 5);
                        player.Broadcast(2, "<color=#FF5E3F> Ви дуже близько підійшли до SCP-343 </color>");
                    } else if (Global.Player_Role.ContainsKey("035")) {
                        if (player != pl && Global.Player_Role["035"] == player) {
                            player.Heal(1);
                            player.EnableEffect(EffectType.MovementBoost, 15, 5);
                        }
                    } else if (!Check(player)) { 
                        if (Vector3.Distance(pl.Position, player.Position) < 4) {
                            player.EnableEffect(EffectType.Blinded, 255, 5);
                        }
                        player.EnableEffect(EffectType.AmnesiaVision, 255, 5);
                        player.Broadcast(2, "<color=#FF5E3F> Ви дуже близько підійшли до SCP-343 </color>");
                    }
                }
            }
            yield return Timing.WaitForSeconds(2);
        }
    }

    private IEnumerator<float> _Heal(Player player, int Health, float s, UsingItemCompletedEventArgs ev) {
        for (int i = 0; i < Health; i++) {
            if (Vector3.Distance(ev.Player.Position, player.Position) < 10 && !player.IsScp) {
                player.EnableEffect(EffectType.Burned);
                player.Heal(6);
                if (player.Health >= player.MaxHealth) {
                    player.AddAhp(2, 70, 0);
                }
            }
            yield return Timing.WaitForSeconds(s);
            player.DisableEffect(EffectType.Burned);
        }
    }

    private IEnumerator<float> Boom(int s, Player pl) {
        yield return Timing.WaitForSeconds(s);
        pl.AddItem(ItemType.SCP2176);
    }

    private IEnumerator<float> Ef(int s, Player pl) {
        pl.EnableEffect(EffectType.Flashed);
        pl.EnableEffect(EffectType.Slowness, 20, 2);
        yield return Timing.WaitForSeconds(s);
        pl.DisableEffect(EffectType.Flashed);
        pl.DisableEffect(EffectType.Slowness);
    }
    private IEnumerator<float> HUD_Render(float s, Player pl, int del) {
        int i = 0;
        for (; ; ) {
            yield return Timing.WaitForSeconds(s);
            HUD =
                $"<size=20><voffset=-400><align=right><color=#bebbb6>|Безсмертя: <color=#FF5E3F>{Manager.it[0]}</color></align></voffset></size>\n" +
                $"<size=20><align=right><color=#bebbb6>|Телепорт: <color=#FF5E3F>{Manager.it[1]}</color></align></size>\n" +
                $"<size=20><align=right><color=#bebbb6>|Лікування: <color=#FF5E3F>{Manager.it[2]}</color></align></size>\n" +
                $"<size=20><align=right><color=#bebbb6>|Політ: <color=#FF5E3F>{Manager.it[3]}</color></align></size>\n";
            HUD_D = HUD_1 + HUD_0;
            pl.ShowHint(string.Empty, 3);
            if (i < del) {
                i++;
            } else {
                HUD_D = " ";
                HUD_1 = " ";
                HUD_0 = " ";
                i = 0;
            }
        }
    }
}