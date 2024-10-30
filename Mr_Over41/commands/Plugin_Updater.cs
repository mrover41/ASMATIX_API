using CommandSystem;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestPlugin.commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Plugin_Updater : ICommand {
        public string Command => "Update";
        public string[] Aliases => new string[] { "up" };
        public string Description => "Обновлелие ASMATIX_API";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player s = Player.Get(sender);
            CheckForUpdates(s);
            response = "Downloading....";
            return true;
        }
        async void CheckForUpdates(Player sender) { 
            try { 
                using (HttpClient client = new HttpClient()) {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                    var response = await client.GetByteArrayAsync("https://github.com/mrover41/ASMATIX_API/releases/download/11.11.11/Asmatix_API.dll");    
                    using (FileStream fileStream = new FileStream("/home/container/.config/EXILED/Plugins/Asmatix_API.dll", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
                        await fileStream.WriteAsync(response, 0, response.Length);
                    }
                }
                sender.Broadcast(2, "<color=#ff0000>Downloading completed!</color>");
                foreach (Player player in Player.List) {
                    player.Broadcast(5, "<color=#ff0000>УВАГА! <color=#00ff00>Сервер буде перезавантажено в наступному раунді з метою оновлення <b>Asmatix_API!</b></color>");
                }
                Exiled.Events.Handlers.Server.RestartingRound += Server_Reload;
            } catch(Exception ex) {
                Log.Error(ex.Message);
            }
        }   
        void Server_Reload() {
            Exiled.Events.Handlers.Server.RestartingRound -= Server_Reload;
            Server.Restart();
        }
    }
}
//https://github.com/mrover41/ASMATIX_API/releases/latest
