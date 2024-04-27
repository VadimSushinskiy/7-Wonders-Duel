using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7_Wonders.Models.DbModels
{
    public class GameResults
    {
        public int Id { get; set; }
        public string FirstPlayerName { get; set; }
        public string SecondPlayerName { get; set; }
        public string WinType { get; set; }
        public string Winner {  get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
