using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model.Parser;

namespace WpfTiles.Model
{
    public class PlayerTileItem : TileItem
    {
        private List<TileItem> _MapTiles = new List<TileItem>();
        public List<TileItem> MapTiles
        {
            get { return _MapTiles; }
            set { _MapTiles = value; }
        }

        private int _Direction;
        public int Direction
        {
            get { return _Direction; }
            set
            {
                if (_Direction != value)
                {
                    _Direction = value;
                    if (_Direction < 0)
                        _Direction = 3;
                    else if (_Direction > 3) 
                        _Direction = 0;
                    NotifyPropertyChanged();
                }
            }
        }
        public void MoveForward()
        {
            if (_Direction == 0)
                X++;
            else if (_Direction == 1)
                Y++;
            else if (_Direction == 2)
                X--;
            else if (_Direction == 3)
                Y--;
            else
                throw new NotImplementedException($"MoveForward->_Direction:{_Direction}, invalid");
        }

        public void RotateRight()
        {
            Direction++;               
        }
        public void RotateLeft()
        {
            Direction--;
        }
        public void MakeSignMove(ControlTileItem item)
        {
            switch (item.Sign)
            {
                case (int)ENUM_SignTypes.Forward:
                    if (MoveValidator.ValidateForwardMove(this, item, MapTiles))
                        MoveForward();
                    break;
                case (int)ENUM_SignTypes.RotateRight:
                    if (MoveValidator.ValidateRotation(this, item, MapTiles))
                        RotateRight();
                    break;
                case (int)ENUM_SignTypes.RotateLeft:
                    if (MoveValidator.ValidateRotation(this, item, MapTiles))
                        RotateLeft();
                    break;
                default:
                    throw new NotImplementedException($"PlayerTileItem.MakeSignMove sign: '{item.Sign}'");
            }
        }
        public bool ValidateMoveSet(TileItem item)
        {
            return MoveValidator.ValidateMoveSet(this, item, MapTiles);
        }

        public PlayerTileItem() { }
        public PlayerTileItem(TileItem baseItem)
        {
            X = baseItem.X;
            Y = baseItem.Y;
            Width = baseItem.Width;
            Height = baseItem.Height;
            Color = baseItem.Color;
        }
    }
}
