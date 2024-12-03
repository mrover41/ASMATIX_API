using Exiled.API.Interfaces;
using System.Collections.Generic;
using TestPlugin.Item;
using TestPlugin.Mr_Over41.Item;
using TestPlugin.Roles;

namespace TestPlugin
{
    public sealed class Config : IConfig
    {
        //private List<int> players_List;

        // Включить или отключить плагин
        public bool IsEnabled { get; set; } = true;

        // Включить или отключить режим отладки
        public bool Debug { get; set; } = false;

        public Good good { get; set; } = new Good();
        public ItemD ChipiChipiChapaChpaa { get; set; } = new ItemD();
        public SCP420J Travka {  get; set; } = new SCP420J();
        public FunGranate Gr {  get; set; } = new FunGranate();
        public Water water { get; set; } = new Water();
        public Trangulizer Trangulizer { get; set; } = new Trangulizer();
        public gravityGranate gravity { get; set; } = new gravityGranate();
        public WtfGranate Wtf { get; set; } = new WtfGranate();
        //public List<int> Players_List { get => players_List; set => players_List = value; }
        //public static List<uint> HUD_Donat_Players { get; set; } = new List<uint>();
    }
}
