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
    CoroutineHandle coroutine;
    protected override void SubscribeEvents() {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
        Exiled.Events.Handlers.Player.Spawned += OnSpawn;
        Exiled.Events.Handlers.Player.Died += OnDie;
        base.SubscribeEvents();
    }
    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawn;
        Exiled.Events.Handlers.Player.Died -= OnDie;
        base.UnsubscribeEvents();
    }

    private void OnRoundStarted() {
        if (Exiled.API.Features.Player.List.Count() >= 8) {
            if (random.Next(0, 10) < 50) {
                CustomRole.Get((uint)1).AddRole(Exiled.API.Features.Player.List.Where(x => x.Role.Type == RoleTypeId.FacilityGuard)?.ToList().RandomItem());
            }
        }
    }
    void OnSpawn(SpawnedEventArgs ev) {
        if (Check(ev.Player)) {
            coroutine = Timing.RunCoroutine(Damage(ev.Player, 1, 2));
        }
    }
    void OnDie(DiedEventArgs ev) { 
        if (Check(ev.Player)) {
            Timing.KillCoroutines(coroutine);
            Log.Info("hh");
        }
    }
    private IEnumerator<float> Damage(Player player ,float s, int damage) {
        for (; ; ) {
            yield return Timing.WaitForSeconds(s);
            if (player.Health > damage)
            {
                player.Health -= damage;
            }
            else
            {
                player.Kill(DamageType.ParticleDisruptor);
                Timing.KillCoroutines(coroutine);
            }
        }
    }
}
