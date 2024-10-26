
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
using PluginAPI.Core;
using System;
using System.Linq;
using TestPlugin;
using TestPlugin.Configs;
using TestPlugin.GoodMode;
using TestPlugin.HUD;
using UnityEngine;

public sealed class test : Plugin<Config>
{
    public override string Author => "Mr_Over41";
    public override string Name => "API";
    public override string Prefix => "Asmatix_API";
    public override Version Version => new Version(1, 0, 2);

    public override void OnEnabled() {
        OnLoad();
        Exiled.Events.Handlers.Server.RoundStarted += RoundSt;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnRoundRest;
        Exiled.Events.Handlers.Player.ChangingRole += GMode._ChaingRole;
        base.OnEnabled();
    }

    public override void OnDisabled() {
        OnUnload();
        Exiled.Events.Handlers.Server.RoundStarted -= RoundSt;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnRoundRest;
        Exiled.Events.Handlers.Player.ChangingRole -= GMode._ChaingRole;
        base.OnDisabled();
    }
    System.Random random = new System.Random();
    void OnRoundRest() {
        //WaitPlayer_HUD.Run();
    }
    void RoundSt() {
        API.RoundTime = DateTime.Now.Second;
        Spawn_System.RoundSt();
        Global.Player_Role.Clear();
    }
     void OnLoad () {
        //HUD
        //WaitPlayer_HUD.Run();
        HUD_LOADER.OnEnabled();
        //REGISTER
        Config.Travka.Register();
        Config.Privid.Register();
        Config.good.Register();
        Config.ChipiChipiChapaChpaa.Register();
        Config.Gr.Register();
        Config.water.Register();
        Config.Trangulizer.Register();
     }
    void OnUnload() {
        //HUD
        HUD_LOADER.OnDisabled();
        //UNREGISTER
        Config.Travka.Unregister();
        Config.Privid.Unregister();
        Config.good.Unregister();
        Config.ChipiChipiChapaChpaa.Unregister();
        Config.Gr.Unregister();
        Config.water.Unregister();
        Config.Trangulizer.Unregister();
    }
}


