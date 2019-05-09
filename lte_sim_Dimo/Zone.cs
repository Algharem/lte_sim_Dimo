using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace lte_sim_Dimo
{
   public class Zone:CollectionBase
    {
        private double _Zone_FACH_Power;
        private double _Zone_Users_Total_DCH_Power;
       internal static int id = 0;
        public double Zone_Users_Total_DCH_Power
        {
            get { return
                 Total_DCH_Power();
            }
            
        }
        private double _Radius;
        private int _ZoneId;

        public double Zone_FACH_Power
        {
            get { return _Zone_FACH_Power; }
            set { _Zone_FACH_Power = value; }
        }
      

        public int ZoneId
        {
            get { return _ZoneId; }
            set { _ZoneId = value; }
        }
        

        public double Radius
        {
            get { return _Radius; }
           // set { _Radius = value; }
        }
       /// <summary>
        /// counstructor
       /// </summary>
       /// <param name="radius">Zone Radius</param>
        public Zone(double radius)
        {
            id++;
            _ZoneId = id;
            _Radius = radius;
            _Zone_FACH_Power = SimParameters.Get_FACH_POWER(_Radius);
        }

        public Zone() { }
       
        private double Total_DCH_Power()
        {
            _Zone_Users_Total_DCH_Power = 0;
            foreach (UE ue in List)
                _Zone_Users_Total_DCH_Power += ue.UE_DCH_Power_Needed;
            return _Zone_Users_Total_DCH_Power;
         }
        public UE this[int UserId]
        {
            get
            {
                return (UE)List[UserId];
            }
            set
            {
                List[UserId] = value;
            }
        }

        public void Remove(UE ue)
        {
            List.Remove(ue);
        }

        public void Add(UE ue)
        {
            List.Add(ue);
           _Zone_Users_Total_DCH_Power += ue.UE_DCH_Power_Needed;
        }
        
    
    }
}
