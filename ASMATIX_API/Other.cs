using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace TestPlugin {
    public static class Global {
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
