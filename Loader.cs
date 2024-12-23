
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.CustomItems.API;
using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Server;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp3114;
using System;
using TestPlugin;
using TestPlugin.GoodMode;
using TestPlugin.HUD;

public sealed class test : Plugin<Config> {
    public override string Author => "Mr_Over41";
    public override string Name => "Asmatix_API";
    public override string Prefix => "Made for Imperial Asmatix";
    public override Version Version => new Version(6, 6, 6);
    public static Harmony patch;

    public override void OnEnabled() {
        OnLoad();
        Global.Player_Role.Clear();
        patch = new Harmony("com.patch.asmatix");
        patch.PatchAll();
        Harmony.DEBUG = false;
        var patchedMethods = Harmony.GetAllPatchedMethods();
        foreach (var method in patchedMethods) {
            Log.Info($"Harmony: Патч применён к методу: {method.Name}");
        }
        Exiled.Events.Handlers.Server.RoundStarted += RoundSt;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnRoundRest;
        Exiled.Events.Handlers.Player.ChangingRole += GMode._ChaingRole;
        Exiled.Events.Handlers.Server.RoundEnded += RoundStop;
        base.OnEnabled();
    }

    public override void OnDisabled() {
        OnUnload();
        patch.UnpatchAll();
        Exiled.Events.Handlers.Server.RoundStarted -= RoundSt;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnRoundRest;
        Exiled.Events.Handlers.Player.ChangingRole -= GMode._ChaingRole;
        Exiled.Events.Handlers.Server.RoundEnded -= RoundStop;
        base.OnDisabled();
    }
    System.Random random = new System.Random();
    void RoundStop(RoundEndedEventArgs ev) {
        Global.Player_Role.Clear();
    }
    void OnRoundRest() {
        //WaitPlayer_HUD.Run();
    }
    void RoundSt() {
        API.Spawn_System.RoundSt();
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
        Config.Wtf.Register();
        Config.gravity.Register();
        TestPlugin.Roles.Fixed_Roles.Scp3114Fix.Enable();
     }
    void OnUnload() {
        //HUD
        HUD_LOADER.OnDisabled();
        API.API.UnLoad();
        //UNREGISTER
        TestPlugin.Roles.Fixed_Roles.Scp3114Fix.Disable();
        Config.Travka.Unregister();
        Config.good.Unregister();
        Config.ChipiChipiChapaChpaa.Unregister();
        Config.Gr.Unregister();
        Config.water.Unregister();
        Config.Trangulizer.Unregister();
        Config.Wtf.Unregister();
        Config.gravity.Unregister();
    }
}


