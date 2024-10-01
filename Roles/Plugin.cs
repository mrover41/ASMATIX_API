
﻿using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using TestPlugin.Configs;

public sealed class test : Plugin<Config>
{
    public override string Author => "Mr_Over41";
    public override string Name => "Roles";
    public override string Prefix => "CustumRole";
    public override Version Version => new Version(1, 0, 2);

    public override void OnEnabled() {
        Config.good.Register();
        Config.SCP035.Register();
        base.OnEnabled();
    }

    public override void OnDisabled() {
        Config.good.Unregister();
        Config.SCP035.Unregister();
        base.OnDisabled();
    }

}


