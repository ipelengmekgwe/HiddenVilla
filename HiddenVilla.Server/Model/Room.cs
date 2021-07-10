using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Server.Model
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public List<RoomProp> RoomProps { get; set; }
    }
}
