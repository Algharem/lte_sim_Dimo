using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lte_sim_Dimo
{
    public enum TransmissionMode
    {
        FACH,
        DCH,
        DUAL,
        Other,
    };
   public class RRC
    {
        Double threshold;
       public List<UE> DCHGroup=new List<UE>();
       public List<UE> FACHGroup=new List<UE>();
       public List<UE> OutAreaUsers = new List<UE>();
       public UE FACH_worstcase;
       public UE DCH_worstcase;
       public TransmissionMode transmission_mode;
       /// <summary>
       /// 
       /// </summary>
        public Double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

       public RRC()
       {
           threshold = 150;// Temperary as example
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="UEList"></param>
      public void Grouping(List<UE> UEList)
       {
         
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
       /// TO compare the power need if use FACH or DCH and then choose the best one and when
       /// </summary>
       /// <param name="mylist"></param>
       public bool CompareDCH_FACH(List<UE> mylist)
       {
           bool found = false;
          double DCH_Power_Sum = 0;
          string str = "";
          int id = 0;
           mylist = SortByDistanc(mylist);
           mylist.ForEach(delegate(UE ue)
           {
              // str = ue.ID + ", Out of coverge area";
               ue.IsWorstCase = false;
               if (ue.Group != UserGroup.Others &&  !found)
               {
                  

                   DCH_Power_Sum += ue.UE_DCH_Power_Needed;
                    str = ue.ID + "," + ue.UE_DCH_Power_Needed + "," + ue.UE_FACH_Power_Needed + "," + DCH_Power_Sum +"," +found;
                   
                   if ((DCH_Power_Sum > ue.UE_FACH_Power_Needed) && !found)
                   {

                       ue.IsWorstCase = true;
                       FACH_worstcase = ue;
                       found = true;
                       ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_WorstCase;
                      // return;
                   }
               }
             //Result.write(str,"details");
           });
           return found;
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="mylist"></param>
      /* public UE CompareDCH_FACH(List<UE> mylist)
       {
           double DCH_Power_Sum = 0;
           mylist = SortByDistanc(mylist);
           mylist.ForEach(delegate(UE ue)
           {
               DCH_Power_Sum += ue.UE_DCH_Power_Needed;
               if (DCH_Power_Sum < ue.UE_FACH_Power_Needed)
               {

                   ue.WorstCase = true;
                   FACH_worstcase = ue;
                   
               }
           });
           return FACH_worstcase;
       }
       */
       
       /// <summary>
       ///  This method used to get the Nearest user in a list of users
       /// </summary>
       /// <param name="mylist"> list of Mobile users</param>
       /// <returns> The Nearest user frm the BaseSitation</returns>
       public UE Nearst_UE(List<UE> mylist)
       {
           UE nearst_ue = mylist[0];
           
           mylist.ForEach(delegate(UE ue)
           {
               if (nearst_ue.DistansFromBS > ue.DistansFromBS)
                   nearst_ue = ue;
           });

           return nearst_ue;
       }

       /// <summary>
       ///  This method used to get the farest user in a list of users
       /// </summary>
       /// <param name="mylist"> list of Mobile users</param>
       /// <returns> the farest user frm the BaseSitation</returns>
       public UE Farest_UE(List<UE> mylist)
       {

           UE farest_ue ;//= new UE(); 
          // if(mylist.Count>0)   // To insure that the number of users is large than 0.
           farest_ue= mylist[0];
          
           mylist.ForEach(delegate(UE ue)
           {
               if (farest_ue.DistansFromBS < ue.DistansFromBS)
                   farest_ue = ue;
           });

           return farest_ue;
       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="userlist"></param>
       /// <returns></returns>
      public List<UE>  ChangeUersImage(List<UE> userlist)
       {
           foreach (UE ue in userlist)
               switch (ue.Group)
               {
                   case UserGroup.DCH_Group:
                       ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_DCHGroup;
                       break;
                   case UserGroup.FACH_Group: ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_icon;
                       break;
                   case UserGroup.Others: ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_Out;
                       break;
               }
           return userlist;

       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="UEList"></param>
       /// <param name="radius"></param>
       /// <returns></returns>
      public List<UE> Dividusers(Form1 form, List<UE> UEList, double radius)
      {
          //  Form1 form = new Form1();
         
              DCHGroup.Clear();
              FACHGroup.Clear();
              string s = "";
             if (CompareDCH_FACH(UEList))
             {
              threshold = FACH_worstcase.UE_FACH_Power_Needed;
              foreach (UE ue in UEList)
              {
                  string str = "";
                  if (ue.UE_FACH_Power_Needed <= threshold)
                  {
                      ue.Group = UserGroup.FACH_Group;
                      if (ue == FACH_worstcase)
                          ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_WorstCase;
                      else
                      ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_icon;
                      FACHGroup.Add(ue);
                      s = " >> User ( " + ue.Name + " ) Join FACH GROUP \n";
                  }
                  else if ((ue.UE_FACH_Power_Needed > threshold) && (ue.DistansFromBS <= radius))
                  {
                      ue.Group = UserGroup.DCH_Group;
                      ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_DCHGroup;
                      DCHGroup.Add(ue);
                      s = " >> User ( " + ue.Name + " ) Join DCH GROUP\n";
                  }
                  else
                  {
                      ue.Group = UserGroup.Others;
                      ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_Out;
                      //OtherGroup.Add(ue); // 
                      s = " >> User ( " + ue.Name + " ) Leave The BS area \n";
                  }
                  //str = ue.ID + "," + ue.UE_DCH_Power_Needed + "," + ue.UE_FACH_Power_Needed;
                //  form.display(s);
                 // Result.write(str);
              }
          }

            //  if (DCHGroup.Count > 0)
              //    DCH_worstcase = Farest_UE(DCHGroup);
              //if (FACHGroup.Count > 0)
              // FACH_worstcase = Farest_UE(FACHGroup);


            //  UEList.ForEach(delegate(UE ue)   // To Change Worst User Image in DCH and FACH Groups
                 // {
                    //  if ((ue == DCH_worstcase) || (ue == FACH_worstcase))
                     // {
                       //   ue.BackgroundImage = global::lte_sim_Dimo.Properties.Resources.UE_WorstCase;
                        //  s = " >> User ( " + ue.Name + " ) Is the Worst case now in FACH or in DCH \n";

                         // form.display(s);
                    //  }
                //  });
             // s = "----------------------------------------------------- \n";
              //form.display(s);
              return UEList;
          

      }

   }
}
       
        
