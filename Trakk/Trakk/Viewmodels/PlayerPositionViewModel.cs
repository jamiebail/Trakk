using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trakk.Viewmodels
{
    public class PlayerPositionViewModel
    {
        public int PlayerId { get; set; }
        public int Index { get; set; }
        public string PlayerName{ get; set; }
        public string PositionName { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public byte[] Profile { get; set; }
    }
}