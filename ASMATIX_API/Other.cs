using CommandSystem.Commands.RemoteAdmin;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestPlugin
{
    public static class Global
    {
        //Action
        public static Action Run_ob;
        public static Action Stop_ob;
        //bpr
        public static bool d = false;
        public static bool f = true;
        public static bool SCP035 = true;
        //Information
        public static Dictionary<Exiled.API.Features.Items.Item, int> it = new Dictionary<Exiled.API.Features.Items.Item, int>();
        public static Dictionary<Player, int> Player_Oboron = new Dictionary<Player, int>();
        public static Dictionary<string, Player> Player_Role = new Dictionary<string, Player>();
    }
}
