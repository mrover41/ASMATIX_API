using Exiled.API.Interfaces;
using System.Collections.Generic;
using TestPlugin.Roles;

namespace TestPlugin.Configs
{
    public sealed class Config : IConfig
    {
        private List<int> players_List;

        // Включить или отключить плагин
        public bool IsEnabled { get; set; } = true;

        // Включить или отключить режим отладки
        public bool Debug { get; set; } = false;

        public Good good { get; set; } = new Good();
        public SCP035 SCP035 { get; set; } = new SCP035();
        public ItemD ChipiChipiChapaChpaa { get; set; } = new ItemD();
        public SCP689 Privid {  get; set; } = new SCP689();
        public List<int> Players_List { get => players_List; set => players_List = value; }
    }
}
