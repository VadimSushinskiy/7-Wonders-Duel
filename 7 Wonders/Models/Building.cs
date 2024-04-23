using System;

namespace _7_Wonders.Models
{
    public abstract class Building
    {
        public Resources Cost { get; protected set; }
        public Resources Reward { get; protected set; }
        public short Fame { get; protected set; }
        public string Name { get; set; }

        protected Building(Resources cost, Resources reward, short fame, string name)
        {
            Cost = cost;
            Reward = reward;
            Fame = (fame >= 0) ? fame : (short)0;
            Name = name;
        }

        protected Building()
        {
            Name = "";
        }
    }
}
