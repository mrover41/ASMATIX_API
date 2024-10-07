
﻿using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using PlayerRoles;
using PluginAPI.Core.Items;
using System.Collections.Generic;
using System.Linq;
using TestPlugin;
using UnityEngine;


[Exiled.API.Features.Attributes.CustomItem(ItemType.GunCOM18)]
public class ItemD : CustomItem
{
    public override string Description { get; set; } = "Дублює предмети";
    public override float Weight { get; set; } = 2f;
    public override string Name { get; set; } = "Дублiкатор";
    public override uint Id { get; set; } = 122;
    public override ItemType Type { get; set; } = ItemType.GunCOM18;

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Player.Shot += Wapon;
        Exiled.Events.Handlers.Player.ReloadingWeapon += Reload;
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.Shot -= Wapon;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= Reload;
        base.UnsubscribeEvents();
    }

    void Wapon(ShotEventArgs ev) {
        if (!Check(ev.Item)) {
            return;
        }
        if (!Global.it.ContainsKey(ev.Item)) {
            Global.it.Add(ev.Item, 3);
        } if (Physics.Linecast(ev.Player.CameraTransform.position, ev.RaycastHit.point, out RaycastHit raycastHit)) {
            if (raycastHit.transform.TryGetComponent(out ItemPickupBase itemPickupBase) && Global.it[ev.Item] > 0) {
                if (itemPickupBase.NetworkInfo.ItemId != ItemType.MicroHID && itemPickupBase.NetworkInfo.ItemId != ItemType.ParticleDisruptor && itemPickupBase.NetworkInfo.ItemId != ItemType.Jailbird) {
                    Pickup.CreateAndSpawn(itemPickupBase.NetworkInfo.ItemId, ev.RaycastHit.point + Vector3.up * 0.5f, default);
                    Global.it[ev.Item]--;
                    ev.Firearm.Ammo = (byte)Global.it[ev.Item];
                } else {
                    if (ev.Player.Health > 30) {
                        ev.Player.Health -= 30;
                        ev.Player.Broadcast(5, "<color=#FF5E3F> Ви не можете дублювати цей предмет </color>");
                    } else {
                        ev.Player.Kill(DamageType.ParticleDisruptor);
                    }
                }
            }
        }
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
                Location = SpawnLocationType.InsideHid,
                Chance = 100
            }
        },
        StaticSpawnPoints = new List<StaticSpawnPoint> {
            new StaticSpawnPoint() {
                    Chance = 100,
                    Position = new UnityEngine.Vector3(0.007f, 1.413f, -7f), Name = "Дублiкатор"
            }
        }
    };
}
