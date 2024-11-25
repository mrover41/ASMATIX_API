
using Exiled.API.Features;
using Exiled.CustomItems.API;
using Exiled.CustomRoles.API;
using HarmonyLib;
using System;
using TestPlugin;
using TestPlugin.GoodMode;
using TestPlugin.HUD;

public sealed class test : Plugin<Config>
{
    public override string Author => "Mr_Over41";
    public override string Name => "API";
    public override string Prefix => "Asmatix_API";
    public override Version Version => new Version(1, 0, 2);
    //Harmony patch;

    public override void OnEnabled() {
        OnLoad();
        //patch = new Harmony("com.patch.asmatix");
        //patch.PatchAll();
        Harmony.DEBUG = true;
        Exiled.Events.Handlers.Server.RoundStarted += RoundSt;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnRoundRest;
        Exiled.Events.Handlers.Player.ChangingRole += GMode._ChaingRole;
        base.OnEnabled();
    }

    public override void OnDisabled() {
        OnUnload();
        //patch.UnpatchAll();
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
        API.Spawn_System.RoundSt();
        Global.Player_Role.Clear();
    }
     void OnLoad () {
        //HUD
        //WaitPlayer_HUD.Run();
        HUD_LOADER.OnEnabled();
        API.API.Load();
        //REGISTER
        Config.Travka.Register();
        Config.good.Register();
        Config.ChipiChipiChapaChpaa.Register();
        Config.Gr.Register();
        Config.water.Register();
        Config.Trangulizer.Register();
     }
    void OnUnload() {
        //HUD
        HUD_LOADER.OnDisabled();
        API.API.UnLoad();
        //UNREGISTER
        Config.Travka.Unregister();
        Config.good.Unregister();
        Config.ChipiChipiChapaChpaa.Unregister();
        Config.Gr.Unregister();
        Config.water.Unregister();
        Config.Trangulizer.Unregister();
    }
}


