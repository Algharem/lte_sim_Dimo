using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lte_sim_Dimo
{
    public class Packet
    {
        int Size;		// simulated packet size
        int MacSize = 12;    //optimal 6 byte
        public static int[] seq;	// unique id
        int pkt_seq;
        int FECsize = 2;       //Forwoe=rd error correction
        public static int[] fec_size;
        byte ack = 0;       // this use by SS to 
        int CodecSize = 3;       // byte
        int PayloadSize = 20;    //byte2
        public static int[] Payload_size;
        double ts_;		// timestamp: for q-delay measurement
        int num_forwards;	// how many times this pkt was forwarded
        double txtime;// tx time for this packet in sec
        int nodeid;
        public Packet(int node_id)
        {
            // UID++;
            nodeid = node_id;
            seq[node_id - 1]++;// = UID;
            num_forwards = 1;
            pkt_seq = seq[node_id - 1];
            PayloadSize = Payload_size[node_id - 1];
            FECsize = fec_size[node_id - 1];
        }
        public int NodeId
        {
            get { return nodeid; }
        }

        /*  public Packet(byte pkt_ack,int pkt_seq,int node_id)
             {
                 ack = pkt_ack;
                 seq[node_id-1] = pkt_seq;
                 nodeid = node_id;
             }*/
        public byte Pkt_ACK
        {
            set { ack = value; }
            get { return ack; }
        }

        public int Seq
        {
            get { return pkt_seq; }
        }

        //===================================


        //  public Packet RetransmitPKT()
        //  {
        //      Packet pkt = new Packet(1);
        //       return pkt;
        // }
        //====================================
        public int Pkt_Size
        {
            get
            {
                Size = PayloadSize + FECsize + CodecSize + MacSize;
                return Size;
            }
        }
        //====================================
        public int Pkt_Payload
        {
            get
            {

                return PayloadSize;
            }
        }
        //====================================
        public int Pkt_FEC
        {
            get
            {
                return FECsize;
            }
        }
        //=======================================
        public int Num_Forward
        {
            get
            {
                return num_forwards;
            }
            set { num_forwards = value; }
        }
    }
}
