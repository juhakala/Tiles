using Resources.Schemas.XMLSchema1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using WpfTiles.Common;
using WpfTiles.Model.Parser;

namespace WpfTiles.Model
{
    class LevelInfo : NotifyPropertyChangedBase
    {
        private bool _Passed;

        public uint Major { get; set; }
        public uint Minor { get; set; }
        public string FilePath { get; set; }
        public int Width { get { return (int)ENUM_TileSizes.MapBackground; } }
        public int Height { get { return (int)ENUM_TileSizes.MapBackground; } }
        public bool Passed
        { 
            get { return _Passed; }
            set
            {
                if (_Passed != value)
                {
                    _Passed = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        public ICommand LevelInfoSelectedCommand => new RelayCommand(o => Selected());
        public event EventHandler<ChangeMapToEventArgs> ChangeMapToEventHandler;
        private void Selected()
        {
            EventHandler<ChangeMapToEventArgs> handler = ChangeMapToEventHandler;
            handler?.Invoke(this, new ChangeMapToEventArgs(FilePath));
        }
    }
    class LevelInfoes : NotifyPropertyChangedBase
    {
        public List<LevelInfo> Levels { get; set; }
        public uint Major { get; set; }
        public int Width { get { return (int)ENUM_TileSizes.MapBackground; } }
        public int Height { get { return (int)ENUM_TileSizes.MapBackground; } }

        private bool _IsExpanded;
        public bool IsExpanded
        { 
            get { return _IsExpanded; } 
            set
            {
                if (_IsExpanded != value)
                {
                    _IsExpanded = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public bool Passed
        {
            get { return !Levels.Any(o => !o.Passed); }
        }
        public ICommand LevelInfoesExpandedCommand => new RelayCommand(o => Expanded());
        private void Expanded()
        {
            IsExpanded = !IsExpanded;
        }
        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Passed));
        }
    }

    class ModelLevelSelectorController : NotifyPropertyChangedBase
    {
        private List<LevelInfoes> _Levels;
        public List<LevelInfoes> Levels
        {
            get { return _Levels; }
            private set
            {
                if (_Levels != value)
                {
                    _Levels = value;
                }
            }
        }
        public void LevelPassedMethod(object sender, LevelPassedEventArgs e)
        {
            foreach (var item in _Levels)
            {
                var curLvl = item.Levels.FirstOrDefault(o => o.FilePath == e.FilePath);
                if (curLvl != null)
                {
                    curLvl.Passed = true;
                    item.Refresh();
                    break;
                }
            }
        }

        public ModelLevelSelectorController()
        {
            var tmpLevelsLst= new List<LevelInfo>();
            var levelfiles = FileSystem.GetFiles(ENUM_SpecialFolders.Maps);
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            for (int i = 0; i < levelfiles.Length; i++)
            {
                using (Stream reader = new FileStream(levelfiles[i], FileMode.Open))
                {
                    var root = serializer.Deserialize(reader) as Root;
                    var tmpLevelInfo = new LevelInfo()
                    {
                        Major = root.LevelInformation.MajorLevel,
                        Minor = root.LevelInformation.MinorLevel,
                        FilePath = levelfiles[i],
                    };
                    tmpLevelsLst.Add(tmpLevelInfo);
                }
            }
            tmpLevelsLst.Sort((x, y) => x.Major.CompareTo(y.Major));

            _Levels = new List<LevelInfoes>();
            foreach (var item in tmpLevelsLst)
            {
                if (_Levels.Count - 1 < item.Major) //assume tmpLevelLst is in order and start from 0, and continues 1,2,3 -> and so on, maybe add check for this later
                    _Levels.Add(new LevelInfoes() { Levels = new List<LevelInfo>() { item }, Major = item.Major });
                else
                    _Levels[(int)item.Major].Levels.Add(item);
            }
            foreach (var lst in _Levels)
            {
                lst.Levels.Sort((x, y) => x.Minor.CompareTo(y.Minor));
            }
        }
    }
}
