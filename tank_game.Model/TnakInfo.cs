using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace tank_game.Model
{
    [Serializable]
    public class TankInfo
    {
        public string Name { get; set; }
        public int Year { get; set; }

 
    }

    
}
