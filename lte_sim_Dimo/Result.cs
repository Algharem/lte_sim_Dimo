using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
namespace lte_sim_Dimo
{
    class Result
    {

         
       static bool IsFileExit = false;
       static string time = DateTime.Now.Ticks.ToString();
       public static string path = @"E:\MBMS" +time + ".csv";
       public static string[] Rows;
       static bool tableExcit = false;
       public static DataTable table;
        public   Result()
        {
           
            
        }
        private static void MakeTable(string[] coloumnNames,String tablename)
        {
            tableExcit = true;
            // Create a DataTable. 

             table = new DataTable(tablename);

            // Create a DataColumn and set various properties. 
            //column.DataType = System.Type.GetType("System.Decimal");
            for (int i = 0; i < coloumnNames.Length; i++)
            {
                DataColumn column = new DataColumn(coloumnNames[i], System.Type.GetType("System.Double"));
                column.Caption = coloumnNames[i];
                table.Columns.Add(column);
            }
           
        }
        public static DataTable AddDataToTable(double[] r)
        {
            if(!tableExcit)
                MakeTable(new string[] { "DCH", "Dynamic_FACH", "Proposed_Dual", "DualwithZones" }, "Result");
                    
                table.Rows.Add(r[0],r[1],r[2],r[3]);
            return table;
        }


        public static void CalculateThroughput(Packet pkt, int nodeId)
        {
          //  num_pkt_snt++;
          //  string str = "Node " + pkt.NodeId + "  Payload= " + pkt.Pkt_Payload + "  FEC_size= " + pkt.Pkt_FEC + " Pkt_ack= " + pkt.Pkt_ACK;
          //  Pkt_detail.Add(str);
        }

        public static void write2txtfile(Packet rcv_pkt, string pkt_status, int slotId)
        {
            string s = rcv_pkt.NodeId + "," + rcv_pkt.Seq + "," + rcv_pkt.Pkt_Size + "," + rcv_pkt.Pkt_FEC + "," + rcv_pkt.Pkt_Payload + "," + rcv_pkt.Num_Forward + "," + rcv_pkt.Pkt_ACK + "," + pkt_status + "," + slotId;
            string HeadFieldsName = "NodeId,Seq,Size,FEC,payload,Num_Forward,ACK";
            Rows = s.Split(',');

            FileInfo fleMembers = new FileInfo(path);
            StreamWriter swrMembers;
            if (fleMembers.Exists == true)
                swrMembers = fleMembers.AppendText();
            else
            {
                swrMembers = fleMembers.CreateText();
                swrMembers.WriteLine(HeadFieldsName);
            }
            try
            {
                swrMembers.WriteLine(s);
            }
            finally
            {
                swrMembers.Close();
            }

        }
        public static void write(Packet rcv_pkt)
        {
            string s = rcv_pkt.NodeId + "," + rcv_pkt.Seq + "," + rcv_pkt.Pkt_ACK;
            string HeadFieldsName = "NodeId,Seq,ACK";
            string path2 = @"D:\pkt_ack" + ".csv";

            FileInfo fleMembers = new FileInfo(path2);
            StreamWriter swrMembers;
            if (fleMembers.Exists == true)
                swrMembers = fleMembers.AppendText();
            else
            {
                swrMembers = fleMembers.CreateText();
                swrMembers.WriteLine(HeadFieldsName);
            }
            try
            {
                swrMembers.WriteLine(s);
            }
            finally
            {
                swrMembers.Close();
            }

        }
        public static void write(string s)
        {
            string path3 = @"E:\MBMS" + ".csv";

            FileInfo fleMembers = new FileInfo(path);
            StreamWriter swrMembers;
            if (fleMembers.Exists == true)
                swrMembers = fleMembers.AppendText();
            else
            {
                swrMembers = fleMembers.CreateText();
              // swrMembers.WriteLine("UE_Id,DCH Power,FACH Power");
                swrMembers.WriteLine("DCH ,dynamic FACH ,proposed Dual,dual with Zones");
            }
            try
            {
                swrMembers.WriteLine(s);
            }
            finally
            {
                swrMembers.Close();
            }

        }

        public static void write(string s,string fileName)
        {
            string path3 = @"E:\MBMS" + fileName + time + ".csv";
            if (SimParameters.SaveUsersDetails)
            {
                FileInfo fleMembers = new FileInfo(path3);
                StreamWriter swrMembers;

                if (fleMembers.Exists == true)
                    swrMembers = fleMembers.AppendText();
                else
                {
                    swrMembers = fleMembers.CreateText();
                    swrMembers.WriteLine("UE_Id,DCH Power,FACH Power,Cumulative_DCH,FACH2-FACH1,DCH-Difference");
                    //swrMembers.WriteLine("Dual,DCH Power,FACH Power");
                }
                try
                {
                    swrMembers.WriteLine(s);
                }
                finally
                {
                    swrMembers.Close();
                }
            }
        }
        public static void PktReceive(Packet p,int nodeid)
        {
            //num_pkt_rcv++;

        }
        public static void Pktsent(Packet p, int nodeid)
        {

         //   num_pkt_snt++;
        }
        public static void FrameReceive()
        {


        }
        public static void FrameSent()
        {


        }
        public static void UnusedTimeSlot(double slotspace)
        {
          //  unused_time_slot += slotspace;

        }
        public static void UnusedTimeFrame(double unused_time_frame)
        {
            unused_time_frame += unused_time_frame;

        }
      
    }
}
