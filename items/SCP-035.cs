using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;

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
        base.SubscribeEvents();
    }
    protected override void UnsubscribeEvents() {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
        base.UnsubscribeEvents();
    }

    private void OnRoundStarted() {
        if (Exiled.API.Features.Player.List.Count() >= 8) {
            if (random.Next(0, 10) < 50) {
                CustomRole.Get((uint)1).AddRole(Exiled.API.Features.Player.List.Where(x => x.Role.Type == RoleTypeId.FacilityGuard)?.ToList().RandomItem());
            }
        }
    }
}
