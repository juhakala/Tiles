using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model.Parser;

namespace WpfTiles.Model
{
    public class PlayerTileItem : TileItem
    {
        //private bool _MyPlayerNextMoveReady;
        //public bool MyPlayerNextMoveReady
        //{
        //    get { return _MyPlayerNextMoveReady; }
        //    set
        //    {
        //        if (_MyPlayerNextMoveReady != value)
        //        {
        //            _MyPlayerNextMoveReady = value;
        //            NotifyPropertyChanged();
        //            //_MyPlayerNextMoveReady = !value;
        //        }
        //    }
        //}

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
