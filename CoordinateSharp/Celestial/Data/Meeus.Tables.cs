﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoordinateSharp
{
    internal partial class MeeusTables
    {
        //Ch 47
        private static double[] Table47A_Arguments = new double[]
        {
            0,0,1,0,
            2,0,-1,0,
            2,0,0,0,
            0,0,2,0,
            0,1,0,0,
            0,0,0,2,
            2,0,-2,0,
            2,-1,-1,0,
            2,0,1,0,
            2,-1,0,0,
            0,1,-1,0,
            1,0,0,0,
            0,1,1,0,
            2,0,0,-2,
            0,0,1,2,
            0,0,1,-2,
            4,0,-1,0,
            0,0,3,0,
            4,0,-2,0,
            2,1,-1,0,
            2,1,0,0,
            1,0,-1,0,
            1,1,0,0,
            2,-1,1,0,
            2,0,2,0,
            4,0,0,0,
            2,0,-3,0,
            0,1,-2,0,
            2,0,-1,2,
            2,-1,-2,0,
            1,0,1,0,
            2,-2,0,0,

            0,1,2,0,
            0,2,0,0,
            2,-2,-1,0,
            2,0,1,-2,
            2,0,0,2,
            4,-1,-1,0,
            0,0,2,2,
            3,0,-1,0,
            2,1,1,0,
            4,-1,-2,0,
            0,2,-1,0,
            2,2,-1,0,
            2,1,-2,0,
            2,-1,0,-2,
            4,0,1,0,
            0,0,4,0,
            4,-1,0,0,
            1,0,-2,0,
            2,1,0,-2,
            0,0,2,-2,
            1,1,1,0,
            3,0,-2,0,
            4,0,-3,0,
            2,-1,2,0,
            0,2,1,0,
            1,1,-1,0,
            2,0,3,0,
            2,0,-1,-2
        };
        private static double[] Table47B_Arguments = new double[]
        {
            0,0,0,1,
            0,0,1,1,
            0,0,1,-1,
            2,0,0,-1,
            2,0,-1,1,
            2,0,-1,-1,
            2,0,0,1,
            0,0,2,1,
            2,0,1,-1,
            0,0,2,-1,
            2,-1,0,-1,
            2,0,-2,-1,
            2,0,1,1,
            2,1,0,-1,
            2,-1,-1,1,
            2,-1,0,1,
            2,-1,-1,-1,
            0,1,-1,-1,
            4,0,-1,-1,
            0,1,0,1,
            0,0,0,3,
            0,1,-1,1,
            1,0,0,1,
            0,1,1,1,
            0,1,1,-1,
            0,1,0,-1,
            1,0,0,-1,
            0,0,3,1,
            4,0,0,-1,
            4,0,-1,1,

            0,0,1,-3,
            4,0,-2,1,
            2,0,0,-3,
            2,0,2,-1,
            2,-1,1,-1,
            2,0,-2,1,
            0,0,3,-1,
            2,0,2,1,
            2,0,-3,-1,
            2,1,-1,1,
            2,1,0,1,
            4,0,0,1,
            2,-1,1,1,
            2,-2,0,-1,
            0,0,1,3,
            2,1,1,-1,
            1,1,0,-1,
            1,1,0,1,
            0,1,-2,-1,
            2,1,-1,-1,
            1,0,1,1,
            2,-1,-2,-1,
            0,1,2,1,
            4,0,-2,-1,
            4,-1,-1,-1,
            1,0,1,-1,
            4,0,1,-1,
            1,0,-1,-1,
            4,-1,0,-1,
            2,-2,0,1,
        };
        private static double[] Table47A_El_Er = new double[]
        {
            //El
            6288774, 1274027,658314,213618,-185116,-114332,58793,57066,53322,45758,
            -40923,-34720,-30383,15327,-12528,10980,10675,10034,8548,-7888,-6766,-5163,
            4987,4036,3994,3861,3665,-2689,-2602,2390,-2348,2236,-2120,-2069,2048,-1773,
            -1595,1215,-1110,-892,-810,759,-713,-700,691,596,549,537,520,-487,-399,-381,
            351,-340,330,327,-323,299,294,0,
            //Er
            -20905355,-3699111,-2955968,-569925,48888,-3149,246158,-152138,-170733,-204586,
            -129620,108743,104755,10321,0,79661,-34782,-23210,-21636,24208,30824,-8379,-16675,
            -12831,-10445,-11650,14403,-7003,0,10056,6322,-9884,5751,0,-4950,4130,0,-3958,0,3258,
            2616,-1897,-2117,2354,0,0,-1423,-1117,-1571,-1739,0,-4421,0,0,0,0,1165,0,0,8752

        };
        private static double[] Table47B_Eb = new double[]
        {
            5128122,280602,277693,173237,55413,46271,32573,17198,9266,8822,
            8216,4324,4200,-3359,2463,2211,2065,-1870,1828,-1794,-1749,-1565,-1491,
            -1475,-1410,-1344,-1335,1107,1021,833,

            777,671,607,596,491,-451,439,422,421,-366,-351,331,315,302,-283,-229,
            223,223,-220,-220,-185,181,-177,176,166,-164,132,-119,115,107
        };
        private static double Get_Table47A_Values(double[] values, int l, double t, bool sine)
        {
            //sine true returns El
            //sine false return Er
            //Er values start at 60 in the Table47A_El_Er array.

            int nl = l * 4;

            if (sine)
            {
                double e = 1;

                if (Table47A_Arguments[nl + 1] != 0)
                {
                    e = 1 - .002516 * t - .0000074 * Math.Pow(t, 2);

                    if (Math.Abs(Table47A_Arguments[nl + 1]) == 2)
                    {
                        e *= e;
                    }
                }
                return (Table47A_El_Er[l] * e) * Math.Sin(Table47A_Arguments[nl] * values[0] + Table47A_Arguments[nl + 1] * values[1] +
                   Table47A_Arguments[nl + 2] * values[2] + Table47A_Arguments[nl + 3] * values[3]);
            }
            else
            {
                double e = 1;
                if (Table47A_Arguments[nl + 1] != 0)
                {
                    e = 1 - .002516 * t - .0000074 * Math.Pow(t, 2);

                    if (Math.Abs(Table47A_Arguments[nl + 1]) == 2)
                    {
                        e *= e;
                    }
                }
                return (Table47A_El_Er[l + 60] * e) * Math.Cos(Table47A_Arguments[nl] * values[0] + Table47A_Arguments[nl + 1] * values[1] +
                   Table47A_Arguments[nl + 2] * values[2] + Table47A_Arguments[nl + 3] * values[3]);
            }
        }
        private static double Get_Table47B_Values(double[] values, int l, double t)
        {
            int nl = l * 4;
            double e = 1;

            if (Table47B_Arguments[nl + 1] != 0)
            {
                e = 1 - .002516 * t - .0000074 * Math.Pow(t, 2);

                if (Math.Abs(Table47B_Arguments[nl + 1]) == 2)
                {
                    e *= e;
                }
            }
            return (Table47B_Eb[l] * e) * Math.Sin(Table47B_Arguments[nl] * values[0] + Table47B_Arguments[nl + 1] * values[1] +
               Table47B_Arguments[nl + 2] * values[2] + Table47B_Arguments[nl + 3] * values[3]);
        }
    }
}