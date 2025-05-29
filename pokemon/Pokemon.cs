using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemontcg
{
    public class Pokemon
    {
        public string name { get; set; }
        public string type { get; set; }
        public int hp { get; set; }
        public int retreatcost { get; set; }
        public string weakness { get; set; }
        public int stage { get; set; }
        public int energy { get; set; }

        public Pokemon(string Name, string Type, int Hp, int Retreatcost, string Weakness, int Stage, int Energy, attackdata atkdata)
        {
            name = Name;
            type = Type;
            hp = Hp;
            retreatcost = Retreatcost;
            weakness = Weakness;
            stage = Stage;
            energy = Energy;
        }
    }
}
