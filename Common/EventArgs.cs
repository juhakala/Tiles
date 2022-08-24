﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTiles.Model;

namespace WpfTiles.Common
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
    class LevelLoaderEvent : EventArgs
    {
        public ModelGameController mCont { get; set; }
        public LevelLoaderEvent(ModelGameController cont)
        {
            mCont = cont;
        }
    }
}
