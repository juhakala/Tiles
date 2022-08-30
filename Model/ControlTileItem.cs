using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfTiles.Common;

namespace WpfTiles.Model
{
    public enum ENUM_SignTypes
    {
        Forward,
        RotateRight,
        RotateLeft,
    }
    public class ControlTileItem : TileItem, ITileItem, IControlTileItem
    {
        private static int IdCounter = 0;

        private int _Id = -1;
        public int Id
        {
            get
            {
                if (_Id == -1)
                    _Id = IdCounter++;
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }
        private int _Sign;
        public int Sign
        {
            get { return _Sign; }
            set
            {
                if (_Sign != value)
                {
                    _Sign = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                if (value != _Selected)
                {
                    _Selected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand LeftClickCommand => new RelayCommand(o => SetSelected());

        public EventHandler<ControlTileIdEventArgs> TileSelectedHandler;

        private void TileSelectedInvoker()
        {
            EventHandler<ControlTileIdEventArgs> handler = TileSelectedHandler;
            handler?.Invoke(this, new ControlTileIdEventArgs() { Id = Id});
        }

        private void SetSelected()
        {
            TileSelectedInvoker();
            Selected = !Selected;
        }

        public ControlTileItem() 
        {
            Sign = -1;
            Name = string.Empty;
        }
        public ControlTileItem(TileItem baseItem)
        {
            Sign = -1;
            Name = string.Empty;
            X = baseItem.X;
            Y = baseItem.Y;
            Width = baseItem.Width;
            Height = baseItem.Height;
            Color = baseItem.Color;
        }
        public T Clone<T>() where T : IControlTileItem, new()
        {
            var res = new T();
            res.Name = Name;
            res.X = X;
            res.Y = Y;
            res.Width = Width;
            res.Height = Height;
            res.Color = Color;
            res.Sign = Sign;
            res.Selected = Selected;
            res.Id = Id;
            return (T)res;
        }
    }
}
