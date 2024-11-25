using CommandSystem;
using PluginAPI.Core;
using System;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using InventorySystem.Items.Pickups;
using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;

namespace TestPlugin.commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class Take : ICommand {
        public string Command => "Take";
        public string[] Aliases => new string[] { "T" };
        public string Description => "Подобрать карточчку";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            if (sender == null) {
                response = "Вы не игрок";
                return false;
            }
            Player player = Player.Get(sender);
            if (player.Role != RoleTypeId.Scp049) {
                response = "У вас нету ручек ;)";
                return false;
            }
            foreach (Pickup pickup in Pickup.List) { 
                if (Vector3.Distance(player.Position, pickup.Position) < 20) {
                    player.AddItem(pickup.Info.ItemId);
                    player.CurrentItem = pickup.Info.ItemId.GetItemBase();
                    pickup.Destroy();
                    response = "Вы подняли ключкарту";
                    return true;
                }
            }
            /*if (Physics.Linecast(player.Camera.gameObject.transform.position, player.Camera.forward * 100, out RaycastHit info)) {
                if (info.transform.TryGetComponent(out ItemPickupBase pickupBase)) {
                    if (pickupBase.NetworkInfo.ItemId.IsKeycard()) {
                        player.AddItem(pickupBase.NetworkInfo.ItemId);
                        player.CurrentItem = pickupBase.Info.ItemId.GetItemBase();
                        pickupBase.DestroySelf();
                        response = "Вы подняли ключкарту";
                        return true;
                    } else {
                        response = "Это не ключкарта";
                        return false;
                    }
                } else {
                    response = "Это не предмет";
                    return false;
                }
            }*/
            response = "Чтото вызывает скриптовые ошибки";
            return false;
        }
    }
}
