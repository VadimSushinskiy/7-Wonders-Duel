using System;

namespace _7_Wonders.Models
{
    public class Resources
    {
        public short Wood { get; set; }
        public short Stone { get; set; }
        public short Glass { get; set; }
        public short Paper { get; set; }
        public short Brick { get; set; }
        public short Gold { get; set; }

        public Resources() {}

        public Resources(short wood, short stone, short glass, short paper, short brick, short gold)
        {
            Wood = (wood >= 0) ? wood : (short)0;
            Stone = (stone >= 0) ? stone : (short)0;
            Glass = (glass >= 0) ? glass : (short)0;
            Paper = (paper >= 0) ? paper : (short)0;
            Brick = (brick >= 0) ? brick : (short)0;
            Gold = (gold >= 0) ? gold : (short)0;
        }

        public Resources(short gold)
        {
            Gold = (gold >= 0) ? gold : (short)0;
        }

        public short this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Brick;
                    case 1: return Stone;
                    case 2: return Wood;
                    case 3: return Paper;
                    case 4: return Glass;
                    case 5: return Gold;
                    default: return -1;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Brick = value; 
                        break;
                    case 1: Stone = value;
                        break;
                    case 2: Wood = value;
                        break;
                    case 3: Paper = value;
                        break;
                    case 4: Glass = value;
                        break;
                    case 5: Gold = value;
                        break;
                    default: break;
                }
            }
        }
    }
}
