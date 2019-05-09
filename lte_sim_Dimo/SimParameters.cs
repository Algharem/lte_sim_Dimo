using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lte_sim_Dimo
{
    class SimParameters
    {

        static double eNodeB_Radius = 1000; //Meters
       public static int eNodeB_ZonesNumber = 10;// main each zone=100 meter
        static bool _SaveZonesDetails;
        public static bool SaveUsersDetails=false;
        public static bool SaveZonesDetails
        {
            get { return _SaveZonesDetails; }
            set { _SaveZonesDetails = value; }
        }
        static bool _SavefinallResults;
        public static bool SavefinallResults
        {
            get { return _SavefinallResults; }
            set { _SavefinallResults = value; }
        }
        public static double Get_FACH_POWER(double distance)
        {
            return 6*Math.Log10(Math.Exp(distance / 1000));
        }

        public static double Get_DCH_POWER(double distance)
        {
            return Math.Log10(Math.Exp(distance / 1000));
        }
    }
}
