using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace TestPlugin.Configs
{
    public sealed class Config : IConfig
    {
        // Включить или отключить плагин
        public bool IsEnabled { get; set; } = true;

        // Включить или отключить режим отладки
        public bool Debug { get; set; } = false;

        public Good good { get; set; } = new Good();
        public SCP035 SCP035 { get; set; } = new SCP035();
        public Item ChipiChipiChapaChpaa { get; set; } = new Item();
    }
}
