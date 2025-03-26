using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace domino
{
    [XmlRoot("Tile")]
    public class DominoTile
    {
        [XmlElement("Left")]
        public int Left {  get; set; }
        [XmlElement("Right")]
        public int Right { get; set; }

        public DominoTile() { }
        public DominoTile(int left, int right)
        {
            Left = left;
            Right = right;
        }
        
    }
}
