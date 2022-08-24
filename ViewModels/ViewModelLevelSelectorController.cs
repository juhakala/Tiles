using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model;

namespace WpfTiles.ViewModels
{
    class ViewModelLevelSelectorController : ViewModelBase
    {
        public List<LevelInfoes> LevelsDict { get; set; }

        public ViewModelLevelSelectorController(ModelGameController cont)
        {
            LevelsDict = cont.LevelSelectorController.Levels;
        }
    }
}
