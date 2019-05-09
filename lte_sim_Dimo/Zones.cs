using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace lte_sim_Dimo
{
   public  class Zones:CollectionBase
    {
        private double _Zones_Power = 0;

    
       private double minimumPower=0 ;
       private Zone _SelectedZone = new Zone();

       /// <summary>
       /// Selected zone in which each users located out this zone will used DCH channel.
       /// </summary>
       public Zone SelectedZone
       {
           get { return _SelectedZone; }
           set { _SelectedZone = value; }
       }


      public double Zones_Power
      {
          get { return _Zones_Power; }
          set { _Zones_Power = value; }
      }
       public Zones(List<UE> userList, int NumberofZone)
       {
           Zone.id = 0;
           for (int i = 1; i <= NumberofZone; i++)
           {
               Zone zone = new Zone(100 * i);
               this.Add(zone);

               
           }
           DistributeUserOverZone(userList);
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="userList"></param>
       private void DistributeUserOverZone(List<UE> userList)
       {
           
              // foreach (Zone z in this)
               for (int i = 1; i < this.Count; i++)
               {
                   foreach (UE ue in userList)
                   {
                       if (i == 1 && ue.UE_FACH_Power_Needed <= this[0].Zone_FACH_Power)
                           this[0].Add(ue);
                       else if (ue.UE_FACH_Power_Needed <= this[i].Zone_FACH_Power && ue.UE_FACH_Power_Needed > this[i - 1].Zone_FACH_Power)
                           this[i].Add(ue);
                       //
                   }
               }

               Get_Best_Power();
               if (SimParameters.SaveZonesDetails)   
                 print();
       }
       /// <summary>
       /// print zoning Results
       /// </summary>
       private void print()
       {
           string s = "";
           foreach (Zone z in this)
           {
               s = "Zone(" + z.ZoneId.ToString() + ") FACH =, " + z.Zone_FACH_Power.ToString() + ",Total DCH= " + z.Zone_Users_Total_DCH_Power.ToString() + ", No. of users=" + z.Count.ToString() ;
               Result.write(s, "Zones");
             //  Result.write("-------zone("+z.ZoneId.ToString()+" ----------------", "Zones");
               foreach (UE ue in z)
                   Result.write(ue.ID.ToString() + ", " + ue.UE_FACH_Power_Needed + "," + ue.UE_DCH_Power_Needed+","+ue.DistansFromBS, "Zones");
                //   Result.write(ue.ID.ToString()+",FACH= "+ue.UE_FACH_Power_Needed+", DCH= "+ue.UE_DCH_Power_Needed, "Zones");
           }
           s = "Selected Zone(" + _SelectedZone.ZoneId.ToString() + "), mini Power= " + minimumPower + ",Total DCH= " + _Zones_Power.ToString();

           Result.write(s, "Zones");
           Result.write("----,-----,-------,----,----", "Zones");
       }
       /// <summary>
       /// calculate the best total power using Dual transmission alghorithm with Zoning  
       /// </summary>
       
       public void  Get_Best_Power()
       {
           double totalPower = 0;
           
        //   bool found=false;
           foreach (Zone z in this)
               _Zones_Power += z.Zone_Users_Total_DCH_Power;
           minimumPower = _Zones_Power;
           for (int i = 0; i < this.Count-1; i++)
           {
               totalPower = this[i].Zone_FACH_Power;
               for (int j = i+1; j < this.Count; j++)
                     totalPower+= this[j].Zone_Users_Total_DCH_Power;
               if (totalPower < minimumPower)
               {
                   minimumPower = totalPower;
                   _SelectedZone = this[i];
                    
               }
           }
           if (minimumPower > this[this.Count - 1].Zone_FACH_Power)
           {
               minimumPower = this[this.Count - 1].Zone_FACH_Power;
               _SelectedZone = this[this.Count - 1];
           }

           _Zones_Power= minimumPower;
       }
       
       public void Add(Zone zone)
       {
           List.Add(zone);
       }
       public void Remove(Zone zone)
       {
           List.Remove(zone);
       }
       public Zone this[int ZoneId]
       {
           get
           {
               return (Zone)List[ZoneId];
           }
           set
           {
               List[ZoneId] = value;
           }
       }
    }
}
