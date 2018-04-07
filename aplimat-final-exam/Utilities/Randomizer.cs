using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplimat_core.utilities
{
    public class Randomizer
    {
        private double d_min, d_max;
        private int min, max;
        private Random random;

        public Randomizer(int min, int max)
        {
            this.min = min;
            this.max = max + 1;
            random = new Random();
        }

        public Randomizer(double d_min, double d_max)
        {
            this.d_min = d_min;
            this.d_max = d_max + 1;
            this.random = new Random();
        }

        public int Generate()
        {
            return random.Next(min, max);
        }

        public double GenerateDouble()
        {
            return random.NextDouble() * (d_max - d_min) + d_min;
        }
    }
}
