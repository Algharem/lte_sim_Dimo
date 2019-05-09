using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lte_sim_Dimo
{
    public class Power
    {
        private double _DCH_Power = 0;
        private double _Dynamic_FACH_Power = 0;
        private double _DUAL_Power_Proposed = 0;
        private double _DUAL_Power_Zonation = 0;
        public UE SelectedUser = new UE();
        public double DUAL_Power_Zonation
        {
            get { return _DUAL_Power_Zonation; }
            set { _DUAL_Power_Zonation = value; }
        }
       // private double _Dual_DCH_Power = 0;
       // private double _Dual_FACH_Power = 0;
        private TransmissionMode _PropagationMode;
        static int Index=0;
        Zones zones;

        public TransmissionMode PropagationMode
        {
            get { return _PropagationMode; }
            set { _PropagationMode = value; }
        }

        public double DCH_Power
        {
            get { return _DCH_Power; }
            set { _DCH_Power = value; }
        }
   

        public double Dynamic_FACH_Power
        {
            get { return _Dynamic_FACH_Power; }
            set { _Dynamic_FACH_Power = value; }
        }
        

        public double DUAL_Power_Proposed
        {
            get { return _DUAL_Power_Proposed; }
            set { _DUAL_Power_Proposed = value; }
        }

        public Power()
        {
        }
        public Power(List<UE> MBMSlistuser)
        {
           Dual_Transmission_Power(MBMSlistuser);
           

          //  FACH_Transmission_Power(MBMSlistuser);
           // DCH_Transmission_Power(MBMSlistuser);
            //ChooseTransmissionMode();
        }


        private void ZoningAlghorithm(List<UE> userslist)
        {
            zones = new Zones(userslist,SimParameters.eNodeB_ZonesNumber);

        }
        /// <summary>
        /// Sort the Users group by distance from the BS
        /// </summary>
        /// <param name="mylist">List of users which want to be sorted/param>
        /// <returns>sorted List</returns>

        public List<UE> SortByDistanc(List<UE> mylist)
        {

            mylist.Sort(delegate(UE ue1, UE ue2)
            {
                return ue1.DistansFromBS.CompareTo(ue2.DistansFromBS);
            });
            return mylist;
        }


       
     
        /// <summary>
        /// This method calculate the total power which needed for mbms each itteration (1 sec).calculation is base on Dynamic FACH,Proposed Dual,exiting dual which uses zones.
        /// </summary>
        /// <param name="mylist"></param>
        /// <returns></returns>
        public void Dual_Transmission_Power(List<UE> mylist)
        {
            bool found = false;
          //  List<double> Bestdual = new List<double>();
            List<UE> TempUeList = new List<UE>();
            double sum = 0, fach_power = 0, cumulative_DCH = 0;
            double first_user_fach = 0,  different_first_next_fach = 0;
            mylist = SortByDistanc(mylist);
            _PropagationMode = TransmissionMode.DCH;
            Index++;
            Result.write("TTI >>"+Index.ToString(), "details_" + mylist.Count.ToString() + " users_");
            mylist.ForEach(delegate(UE ue)
            {
                string s = ue.ID + ", Out of Coverge Area ";
                ue.IsWorstCase = false;
                if (ue.Group != UserGroup.Others)
                {
                    fach_power = ue.UE_FACH_Power_Needed;
                    cumulative_DCH += ue.UE_DCH_Power_Needed;
                    different_first_next_fach = ue.UE_FACH_Power_Needed - first_user_fach;
                    double dif = ue.UE_DCH_Power_Needed - different_first_next_fach;
                    //dual = false;
                        sum += ue.UE_DCH_Power_Needed;
                        if (sum > ue.UE_FACH_Power_Needed)   // if ((sum > ue.UE_FACH_Power_Needed) && !found)
                        {
                            ue.IsWorstCase = true;
                            TempUeList.Add(ue);
                            found = true;
                          //  _Dual_FACH_Power = ue.UE_FACH_Power_Needed;
                            _PropagationMode = TransmissionMode.DUAL;
                            ue.Group = UserGroup.FACH_Group;
                        }
              
                   
                    s = ue.ID + "," + ue.UE_DCH_Power_Needed + "," + ue.UE_FACH_Power_Needed + "," + cumulative_DCH + "," + different_first_next_fach + "," + dif + "," + found;
                    first_user_fach = ue.UE_FACH_Power_Needed;
                }

                Result.write(s, "details_" + mylist.Count.ToString() + " users_");
            });

            Result.write("UE_Id,DCH Power,FACH Power,Cumulative_DCH,FACH2-FACH1,DCH-Difference", "details_" + mylist.Count.ToString() + " users_");
            Result.write("0,0,0,0,0,0,0,0", "details_" + mylist.Count.ToString() + " users_");
            _DCH_Power =  sum;
            _Dynamic_FACH_Power = fach_power;

            if (found && TempUeList.Count>1)
                _DUAL_Power_Proposed = DualTransmissionPower2(TempUeList);// _Dual_FACH_Power;// +_Dual_DCH_Power;
            else if (_Dynamic_FACH_Power < _DCH_Power)
                _DUAL_Power_Proposed = _Dynamic_FACH_Power;
            else
                _DUAL_Power_Proposed = _DCH_Power;
           
            ZoningAlghorithm(mylist);
            _DUAL_Power_Zonation = zones.Zones_Power;
            string str = _DCH_Power + "," + _Dynamic_FACH_Power + "," + _DUAL_Power_Proposed + "," + zones.Zones_Power+","+SelectedUser.ID.ToString();
           if(SimParameters.SavefinallResults)
            Result.write(str);
        }

       

        private double DualTransmissionPower2(List<UE> mylist)
        {
            List<double> Bestdual = new List<double>();
            double sum = 0, Maximum_value = 0, Dual_power = 0,temp=0;
            bool found = false;
            Maximum_value = FACH_Transmission_Power(mylist);
            temp = Maximum_value;
            Result.write(Index.ToString() + "---------", "dual");
            for (int i = 0; i < mylist.Count - 1; i++)
            {
                //sum = 0;
                Dual_power = mylist[i].UE_FACH_Power_Needed;
                for (int j = i + 1; j < mylist.Count; j++)
                    Dual_power += mylist[j].UE_DCH_Power_Needed;// sum of the Fach power fot the current user and the sum of DCH for the all next users.
                if (Dual_power < temp)
                {
                    temp = Dual_power;
                    Bestdual.Add(temp);
                    SelectedUser = mylist[i];
                    Result.write(Dual_power.ToString() + "," + temp.ToString() + "," + Maximum_value.ToString(), "dual");

                    found = true;
                }
            }

            if (found)
                return Bestdual.Min();
            else
                return Maximum_value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MBMSlistuser"></param>
        /// <returns></returns>
        public double FACH_Transmission_Power(List<UE> MBMSlistuser)
        {
           double largestFACH = 0;
            MBMSlistuser.ForEach(delegate(UE ue)
            {
               if(ue.Group!=UserGroup.Others)
                   if (largestFACH < ue.UE_FACH_Power_Needed)
                   {
                       largestFACH = ue.UE_FACH_Power_Needed;                      
                   }                
               });

            _Dynamic_FACH_Power= largestFACH;
            return largestFACH;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MBMSlistuser"></param>
        /// <returns></returns>
        public void DCH_Transmission_Power(List<UE> MBMSlistuser)
        {

            //bool found = false;
            double DCH_Power_Sum = 0;
           

            MBMSlistuser.ForEach(delegate(UE ue)
            {
                if (ue.Group != UserGroup.Others)
                   {
                       DCH_Power_Sum += ue.UE_DCH_Power_Needed;
                        //found = true;
                    }
            });

          
                DCH_Power= DCH_Power_Sum;
           
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MBMSlistuser"></param>
        public double DCH_SumPower_Nextusers(List<UE> MBMSlistuser,UE current_ue)
        {

            bool found = false;
            double DCH_Power_Sum = 0;


            MBMSlistuser.ForEach(delegate(UE ue)
            {
                if (ue.Group == UserGroup.FACH_Group && ue.UE_FACH_Power_Needed>current_ue.UE_FACH_Power_Needed)
                {
                    DCH_Power_Sum += ue.UE_DCH_Power_Needed;
                    //found = true;
                }
            });
            return DCH_Power_Sum;


           // DCH_Power = DCH_Power_Sum;


        }


        public void ChooseTransmissionMode()
        {
           // if ((_Dynamic_FACH_Power < _DCH_Power) && (_Dynamic_FACH_Power < _DUAL_Power_Proposed))
           //     _PropagationMode= TransmissionMode.FACH;
          //  else if (_DUAL_Power_Proposed < _DCH_Power && _DUAL_Power_Proposed < FACH_Power)
           //     _PropagationMode = TransmissionMode.DUAL;
          //  else
           //     _PropagationMode = TransmissionMode.DCH;
        }


    }
}
