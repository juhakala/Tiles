using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Common;

namespace WpfTiles.Model
{
    class ModelScoreBoard : NotifyPropertyChangedBase
    {
        private int _Score;
        private int _MaxScore;

        public int Score
        {
            get { return _Score; }
            set
            {
                if (_Score != value)
                {
                    _Score = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int MaxScore
        { 
            get { return _MaxScore; }
            set
            {
                if (_MaxScore != value)
                {
                    _MaxScore = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void IncreaseScore(object sender, PlayerPicketStarEventArgs e)
        {
            Score++;
            if (Score == MaxScore)
            {
                //check if max and if it is finish game/round
            }
        }
        public ModelScoreBoard(int max)
        {
            MaxScore = max;
        }
    }
}
