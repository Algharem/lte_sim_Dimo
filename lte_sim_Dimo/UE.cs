using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lte_sim_Dimo
{
    public enum UserGroup
    {
        DCH_Group,
        FACH_Group,
        Others,
    };
    public partial class UE : UserControl
    {
        /// <summary>
        /// public and private fields
        /// </summary>
        #region fields declarator
        private  Point _BsPositionPoint;
        private double _DistansFromBS;
        public static int UserId = 0;
        private int _ID;

     
        private UserGroup group;

        //BackgroundImage
        private Point _Location;
        private List<Point> _UeTracePoints = new List<Point>();
        private List<double> _DistanceTrace = new List<double>();
        private double _UE_DCH_Power_Needed;// power need from BS to reach UE 
        private double _UE_FACH_Power_Needed; // FACH power which needed to receive this user
        private static double _WorstCaseValue = 0;
        private Boolean _IsWorstCase = false;
        private static UE _UE_WorstCase;

       // private Image
        public delegate void ChangePositon(Point newPoint);
        public event ChangePositon eventChangePosition;
        #endregion
        /// <summary>
        /// properties
        /// </summary>
        #region
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public  Image UserImage 
      {
          get
          {
              return this.BackgroundImage;
          }
            set{
                this.BackgroundImage = value;
            }
             
      }
        public UserGroup Group
        {
            get { return group; }
            set { group = value; }
        }
        public static double WorstCaseValue
        {
            get { return UE._WorstCaseValue; }
            set { UE._WorstCaseValue = value; }
        }

        public static UE UE_WorstCase
        {
            get { return UE._UE_WorstCase; }
            set { UE._UE_WorstCase = value; }
        }
        public Boolean IsWorstCase
        {
            get { return _IsWorstCase; }
            set { _IsWorstCase = value; }
        }
        public double UE_DCH_Power_Needed
        {
            get { return _UE_DCH_Power_Needed; }
            //set { _UE_DCH_Power_Needed = value; }
        }

        public double UE_FACH_Power_Needed
        {
            get { return _UE_FACH_Power_Needed; }
            //set { _UE_FACH_Power_Needed = value; }
        }
        public List<double> DistanceTrace
        {
            get { return _DistanceTrace; }
            //set { _DistanceTrace = value; }
        }
        public List<Point> UeTracePoints
        {
            get { return _UeTracePoints; }
            //set { _UeTracePoints = value; }
        }

       

        public Point BsPositionPoint
        {
            get { return _BsPositionPoint; }

        }

        public double DistansFromBS
        {
            get { return _DistansFromBS; }
            //set { _DistansFromBS = value; }
        }
        #endregion

       // Bitmap worstUeImage = new Bitmap( global::lte_sim_Dimo.Properties.Resources.UE_WorstCase);
        //string  UeImage = "global::lte_sim_Dimo.Properties.Resources.UE_icon";

        public UE(Point BS_Position )
        {
           // this.pictureBox1.Image = global::lte_sim_Dimo.Properties.Resources.UE_icon;

            UE.UserId++;
            this.ID = UE.UserId;
            _BsPositionPoint = BS_Position;
            InitializeComponent();
            eventChangePosition += UE_eventChangePosition;
           // toolTip1.SetToolTip(this, this.Name);
        }
        public UE()
        {

            //_BsPositionPoint = BS_Position;
            InitializeComponent();
            eventChangePosition += UE_eventChangePosition;
        }

      /*  public static void  GetWorstCase(List<UE> Current_UserList)
        {
            double temp = Current_UserList[0]._DistansFromBS;
            UE tempUe = Current_UserList[0];
            //bool isworstcase = false;
            foreach (UE ue in Current_UserList)
            {
                //isworstcase = false;
                ue.WorstCase = false;
                if (temp < ue._DistansFromBS)
                {
                    temp = ue._DistansFromBS;
                    tempUe = ue;
                }
            }
            
            _WorstCaseValue = tempUe._DistansFromBS;
            
            UE._UE_WorstCase = tempUe;

            
        }
       * */
      
        void UE_eventChangePosition(Point newPoint)
        {
          
               int  x = Math.Abs(newPoint.X - _BsPositionPoint.X);
               int  y = Math.Abs(newPoint.Y - _BsPositionPoint.Y);
               _DistansFromBS =4* Math.Round(Math.Sqrt(x * x + y * y),2);// each pixel equal 4 meter
               _UE_DCH_Power_Needed = SimParameters.Get_DCH_POWER(_DistansFromBS);// Math.Log10(Math.Exp(_DistansFromBS / 1000));//=LOG10(EXP(I2/1000))
               _UE_FACH_Power_Needed = SimParameters.Get_FACH_POWER(_DistansFromBS);// 8 * _UE_DCH_Power_Needed; //Math.Exp(1/Math.Sqrt(_DistansFromBS));
               _DistanceTrace.Add(_DistansFromBS);
               _UeTracePoints.Add(newPoint);
            
               this.IsWorstCase = false;
               
        }



        private void UE_Move(object sender, EventArgs e)
        {
            UE_eventChangePosition(this.Location);
           // toolTip1.Show(this.ID,this);
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void UE_Load(object sender, EventArgs e)
        {

        }

        private void UE_MouseClick(object sender, MouseEventArgs e)
        {
           // MessageBox.Show(this.ID.ToString());
        }

      

               
    }
}
