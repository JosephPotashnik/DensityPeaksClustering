using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DensityPeaksClustering
{
    public class KNNAlgorithmParams
    {
        public float[][] matrix { get; set; }
        public int k { get; set; }

        public override string ToString()
        {
            string s = $"k : {k}";
            s += "matrix: ";
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    s += $"{matrix[i][j]} ";

                }


            }
            return s;
        }
    }
}
