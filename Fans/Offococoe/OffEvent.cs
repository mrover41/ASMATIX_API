using Exiled.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerRoles;
using UnityEngine;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;

namespace TestPlugin.Fans.Offococoe {
    internal class OffEvent : MonoBehaviour {
        void OnEnable() {
            Exiled.Events.Handlers.Player.Hurt += Snoww;
        }
        void OnDisable() {
            Exiled.Events.Handlers.Player.Hurt -= Snoww;
        }
        void Start() { 
            foreach (Exiled.API.Features.Player player in Exiled.API.Features.Player.List.Where(x => x.Role.Type == RoleTypeId.ZombieFlamingo || x.Role.Type == RoleTypeId.ClassD)) { 
                switch (player.Role.Type) { 
                    case RoleTypeId.ZombieFlamingo:
                        if (API._System.random.Next(0, 100) < 50) player.Teleport(RoomType.EzGateA);
                        else player.Teleport(RoomType.EzGateB);
                        break;
                    case RoleTypeId.ClassD:
                        player.Teleport(Room.List.Where(x => x.Zone == ZoneType.Entrance && x.Type == RoomType.EzStraightColumn).GetRandomValue());
                        player.AddItem(ItemType.Snowball, 2);
                        player.AddItem(ItemType.SCP500);
                        player.AddItem(ItemType.Medkit);
                        break;
                    default:
                        break;

                }
            }
            foreach (Room room in Room.List.Where(x => x.Zone == ZoneType.Entrance)) { 
                if (API._System.random.Next(0, 100) < 50) {
                    Pickup.CreateAndSpawn(ItemType.SpecialCoal, room.Position + Vector3.up);
                }
            }
        }
        void Update() { 
            if (Exiled.API.Features.Player.List.Where(x => x.Role.Type == RoleTypeId.ClassD).Count() <= 1) {
                Exiled.API.Features.Map.Broadcast(10, "Ивент завершенно");
                Destroy(this);
            }
        }
        void Snoww(HurtEventArgs ev) {
            if (ev.Attacker.Role.Type == RoleTypeId.ClassD) {
                ev.Player.EnableEffect(EffectType.Slowness, 5);
            }
        }
    }
}
