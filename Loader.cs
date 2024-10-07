
﻿using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using TestPlugin;
using TestPlugin.Configs;

public sealed class test : Plugin<Config>
{
    public override string Author => "Mr_Over41";
    public override string Name => "API";
    public override string Prefix => "Asmatix_API";
    public override Version Version => new Version(1, 0, 2);

    public override void OnEnabled() {
        OnLoad();
        Timing.RunCoroutine(API.Updater());
        Config.Privid.Register();
        Config.good.Register();
        Config.SCP035.Register();
        Config.ChipiChipiChapaChpaa.Register();
        Exiled.Events.Handlers.Server.RoundStarted += RoundSt;
        base.OnEnabled();
    }

    public override void OnDisabled() {
        OnUnload();
        Config.Privid.Unregister();
        Config.good.Unregister();
        Config.SCP035.Unregister();
        Config.ChipiChipiChapaChpaa.Unregister();
        Exiled.Events.Handlers.Server.RoundStarted -= RoundSt;
        base.OnDisabled();
    }

    void RoundSt() {
        API.RoundTime = DateTime.Now.Second;
        Global.SCP035 = true;
    }
     void OnLoad () { 
    
     }
    void OnUnload() { 
    
    }
}


