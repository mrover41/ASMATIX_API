using CommandSystem;
using Exiled.API.Features;
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
    internal class Plugin_Updater {
        public string Command => "Update";
        public string[] Aliases => new string[] { "up" };
        public string Description => "Обновлелие ASMATIX_API";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            response = "Done";
            return true;
        }
        async void CheckForUpdates() { 
            try { 
                using (HttpClient client = new HttpClient()) {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                    var response = await client.GetByteArrayAsync("https://github.com/mrover41/ASMATIX_API/releases/latest");
                    using (FileStream fileStream = new FileStream("/home/container/.config/EXILED/Plugins/Asmatix_API.dll", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
                        await fileStream.WriteAsync(response, 0, response.Length);
                    }
                }
            } catch(Exception ex) {
                Log.Error(ex.Message);
            }

        }
    }
}
//https://github.com/mrover41/ASMATIX_API/releases/latest
