using System;

namespace _7_Wonders.Models
{
    public struct Resources
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
    }
}
