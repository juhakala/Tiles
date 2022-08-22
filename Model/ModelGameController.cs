﻿using Resources.Schemas.XMLSchema1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WpfTiles.Model.Parser;

namespace WpfTiles.Model
{
    class ModelGameController
    {
        public ModelMap Map { get; set; }
        public ModelControl Control { get; set; }
        public ModelControl AvailableControlT { get; set; }
        public ModelPlayerController PlayerController { get; set; }

        public List<TileItem> MapTiles { get; set; }
        public PlayerTileItem PlayerTile { get; set; }
        public List<ControlTileItem> ControlTiles { get; set; }
        public List<NameTileItem> NameTiles { get; set; }


        public List<ControlTileItem> AvailableControlTiles { get; set; }

        public void HandleCustomEvent(object sender, EventArgs a)
        {
            PlayerController.StartMoveSet();
        }

        public ModelGameController(string FilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            Root? root;
            using (Stream reader = new FileStream(FilePath, FileMode.Open))
            {
                root = serializer.Deserialize(reader) as Root;
            }
            if (root == null)
            {
                //relevant error message and logging once implemented
                Map = new ModelMap();
                MapTiles = new List<TileItem>();
                ControlTiles = new List<ControlTileItem>();
                PlayerTile = new PlayerTileItem();
            }
            else
            {
                Map = ParserMap.ParseMap(root.Map);
                MapTiles = ParserTiles.Parse(root.MapTiles);
                PlayerTile = ParserTiles.Parse(root.PlayerTiles);
                ParserMap.ParseMapMaxMinXY(Map, MapTiles);
                ParserMap.ParseOffsetXY(Map);
                ParserTiles.ParseTilesNewXY(Map, MapTiles);
                ParserTiles.ParseTilesNewXY(Map, PlayerTile);

                Control = ParserMap.ParseControl(root.Control);
                (ControlTiles, NameTiles) = ParserTiles.Parse(root.ControlTiles);
                
                ParserMap.ParseMapMaxMinXY(Control, ControlTiles, NameTiles);
                ParserMap.ParseOffsetXY(Control);
                ParserTiles.ParseTilesNewXY(Control, ControlTiles, NameTiles);

                PlayerTile.MapTiles = MapTiles;

                AvailableControlTiles = ParserTiles.ParseAvailableTiles(root.AvailableControlTiles, 0);
                AvailableControlTiles.AddRange(ParserTiles.ParseAvailableTiles(root.AvailableControlTiles, 1));
                AvailableControlTiles.AddRange(ParserTiles.ParseAvailableTiles(NameTiles));

                AvailableControlT = new ModelControl() { MapAreaWidth = 200, MapAreaHeight = 150 };
                ParserMap.ParseMapMaxMinXY(AvailableControlT, AvailableControlTiles);
                ParserMap.ParseOffsetXY(AvailableControlT);
                ParserTiles.ParseTilesNewXY(AvailableControlT, AvailableControlTiles);

            }
            PlayerController = new ModelPlayerController(PlayerTile, ControlTiles);
        }
    }
}