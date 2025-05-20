using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemontcg
{
    public class attackdata
    {
        public string attackname { get; set; }
        public int damage { get; set; }
        public string effect { get; set; }
        public int energycost {get; set; }
        public attackdata(string Attackname, int Damage, string Effect, int Energycost )
        {
            attackname = Attackname;
            damage = Damage;
            effect = Effect;
            energycost = Energycost;
        }
    }
}
