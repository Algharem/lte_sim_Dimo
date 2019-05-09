using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace lte_sim_Dimo
{
    public partial class Form1 : Form
    {
        List<UE> UserList = new List<UE>();
        List<UE> MBMS_UserList = new List<UE>();// list of users who currently in MBMS session group
        List<UE> HandOver_UserList = new List<UE>();// list of users who leave the current cell
        List<Point> randList = new List<Point>();
        int userspeed = 10;
        int SimulationTime;
        int xcenter, ycenter;
        private Graphics myGraphics;
        Draw mydraw;
        int x, y;
        double radius;
        RRC rrc;
        Movement movement;
        DataTable table;
        public Point BS_location = new Point();
       // Series series_dch = new Series("DCH");
        Series series_dynamic_fach = new Series("Dynamic_FACH");
        Series series_proposed_dual = new Series("Proposed_Dual");
        Series series_dual_wthzone = new Series("DualWthZones");
       
        public Form1()
        {
            InitializeComponent();
          

        }
        public  void display(string s)
        {
         //  this.txtOutput.AppendText(s);
        //this.txtOutput.Refresh();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
             rrc = new RRC();
             UpDown_ZoneNumber.Enabled = false;
            radius = Convert.ToInt16(txtRadius.Text);
            button1.Enabled = false;
            xcenter = (int)CellPanel.Width / 2;
            ycenter = (int)CellPanel.Height / 2;
            BS_location.X = xcenter;
            BS_location.Y = ycenter;
             movement = new Movement(BS_location, radius);
            myGraphics = CellPanel.CreateGraphics();
            mydraw = new Draw(CellPanel.Width, CellPanel.Height);

             //his.Refresh();
            // series_dch.Name = "DCH";
            // series_proposed_dual.Name = "DUAL";
            // series_dynamic_fach.Name = "FACH";
            // chart1.Series.Add(series_dch);
              chart1.Series.Add(series_dynamic_fach);
            chart1.Series.Add(series_proposed_dual);
            chart1.Series.Add(series_dual_wthzone);
            
        }

        private void InitaiteParameters()
        {
            if (chkbx_savedetails.Checked)
                SimParameters.SaveUsersDetails = true;
            else
                SimParameters.SaveUsersDetails = false;

            if (chkbx_SavefinalToExcel.Checked)
                SimParameters.SavefinallResults = true;
            else
                SimParameters.SavefinallResults = false;
            if (chkbx_zoneResult.Checked)
                SimParameters.SaveZonesDetails = true;
            else
                SimParameters.SaveZonesDetails = false;
            SimParameters.eNodeB_ZonesNumber = (int)UpDown_ZoneNumber.Value;
        }
       

       
        /// <summary>
        ///    remove UE from the cell
        /// </summary>
        

        private void RemoveUsers()
        {
           
            CellPanel.Controls.Clear();
           

        }
        private void intitiatedUePosition()
        {
            UserList.Clear();
            UE.UserId = 0;
            this.CellPanel.Update();//.Refresh();
            int user_num = Convert.ToInt16(txtNumOfUers.Text);
            int indexname = 1,count=0 ;
            int user_celledge =Convert.ToInt16(user_num * (UpDown_CellEdgeUsers.Value / 100));
            int users_center = user_num - user_celledge;
            for (int i = 0; i < user_num; i++)
            {
                count++;
                UE ue = new UE(BS_location);
                if (radioButton2.Checked)
                {
                    if(count<=user_celledge)
                        ue.Location = movement.GetRandPoint_CellEdge();
                    else
                        ue.Location = movement.GetRandPoint_CellCenter();
                }
                else
                 ue.Location = movement.GetRandPoint();// for normal distribution
                ue.Name = "ue" + indexname;
                ue.Size = new System.Drawing.Size(17, 29);
                ue.Tag = i;
                ue.AccessibleDefaultActionDescription = ue.Name;
                this.CellPanel.Controls.Add(ue);
                toolTip1.SetToolTip(ue, ue.ID.ToString());
                toolTip1.ShowAlways=true;
                
                UserList.Add(ue);
                LstBoxUsers.Items.Add(ue.Tag);
                randList.Add(ue.Location);
                indexname++;
            }
            mydraw.onEventPaint(myGraphics, (double)radius, Color.Black);
            listBox1.DataSource = randList;
           // UE.GetWorstCase(UserList);// GET the worst case user
            
        }
        

        private void button1_Click(object sender, EventArgs e)
        {


              //   Result.path =@"E:\MBMS_" + DateTime.Now.Ticks.ToString() + ".csv";
                 
               // lstBox_WorstCase.Items.Clear();
               // txtOutput.Text = "";
                int time = 0;
                InitaiteParameters();
                SimulationTime = Convert.ToInt16(txtSimulationTime.Text);
                userspeed =(int) NumUpDown_userspeed.Value;
                progressBar1.Maximum = 100;// SimulationTime;
                progressBar1.Value = 0;
                string s = "DCH,FACH,DUAL,Used";
              //  Result.write(s);
               
                while (time < SimulationTime)
                {
                    this.CellPanel.Refresh();
                  
                   
                   
                    foreach (UE ue in UserList)
                    {
                        ue.Location = movement.GetRandPoint(ue.Location, userspeed);
                        this.Refresh();
                       
                    }
                       UserList = rrc.Dividusers(this,UserList, radius*4);// multiply by 4 to get the actual radius which is 1000m
                       
                    time++;
                    //if (rrc.FACH_worstcase!=null)
                    //mydraw.onEventPaint(myGraphics, rrc.FACH_worstcase.DistansFromBS/4, Color.Blue);
                    mydraw.onEventPaint(myGraphics, (double)radius, Color.Black);
                    lblprogress.Text = Convert.ToString(progressBar1.Value) + " %";// time /(SimulationTime/100))+" %";
                    progressBar1.Value+=100/SimulationTime;
                    //rrc.CompareDCH_FACH(UserList);
                    Power power = new Power(UserList);
                   mydraw.onEventPaint(myGraphics, power.SelectedUser.DistansFromBS / 4, Color.Red);
                    s = power.DCH_Power + " ," + power.Dynamic_FACH_Power + "," + power.DUAL_Power_Proposed + "," + power.DUAL_Power_Zonation;
                     //Result.write(s);
                    DrawChart(time, power.DCH_Power, power.Dynamic_FACH_Power, power.DUAL_Power_Proposed,power.DUAL_Power_Zonation);
                 //   lstBox_WorstCase.Items.Add(s);
                    Result.AddDataToTable(new double[]{power.DCH_Power, power.Dynamic_FACH_Power, power.DUAL_Power_Proposed,power.DUAL_Power_Zonation});
                    
            }
             //   dgRes.DataSource = Result.table;  
                dgResult.DataSource = Result.table;
         
                lblprogress.Text = Convert.ToString(progressBar1.Value) + " %";// time /(SimulationTime/100))+" %";
                

        }
       
        private void DrawChart(int t, double dch, double fach, double dual,double zone)
        {

        
            chart1.Series["Dynamic_FACH"].ChartType = SeriesChartType.Spline;
            chart1.Series["Proposed_Dual"].ChartType = SeriesChartType.Spline;
            chart1.Series["DualWthZones"].ChartType = SeriesChartType.Spline;
            chart1.Series["Dynamic_FACH"].Color = Color.Blue;
            chart1.Series["Proposed_Dual"].Color = Color.Red;
            chart1.Series["DualWthZones"].Color = Color.Green;
         
           // chart1.Series["DCH"].Points.AddXY(t,dch);
            chart1.Series["Dynamic_FACH"].Points.AddXY(t, fach);
            chart1.Series["Proposed_Dual"].Points.AddXY(t, dual);
            chart1.Series["DualWthZones"].Points.AddXY(t, zone);
           // double[] x = { 1, 2, 3, 4, 5, 6, 7, 8 };
          ///  double[] y = { 0.1, .3, .4, .5, .6, 1, 2, 2, 2 };
          //  for (int i = 0; i < x.Length; i++)

           
       
        }


        private void button2_Click(object sender, EventArgs e)
        {
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LstBoxUsers_MouseClick(object sender, MouseEventArgs e)
        {
            int index = (int)LstBoxUsers.SelectedItem;
            if (UserList[index].DistanceTrace.Count > 0)
                LstBoxTrace.DataSource = UserList[index].DistanceTrace.ToArray();
           // chart1.DataSource = UserList[index].DistanceTrace;
           // chart1.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            RemoveUsers();
            this.Refresh();
            intitiatedUePosition();
            rrc.CompareDCH_FACH(UserList);
            double radius = Math.Sqrt(x * x + y * y);



            button1.Enabled = true;
        }

        private void EdgeUpDown_ValueChanged(object sender, EventArgs e)
        {
            centerUpDown.Value = 100 - UpDown_ZoneNumber.Value;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpDown_CellEdgeUsers.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpDown_CellEdgeUsers.Enabled = false;
        }

       

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void rdbtn_line_CheckedChanged(object sender, EventArgs e)
        {
            //rdbtn_Column.Checked = false;

            chart1.Series["Dynamic_FACH"].ChartType = SeriesChartType.Spline;
            chart1.Series["Proposed_Dual"].ChartType = SeriesChartType.Spline;
            chart1.Series["DualWthZones"].ChartType = SeriesChartType.Spline;
          
           
              
        }

        private void rdbtn_Column_CheckedChanged(object sender, EventArgs e)
        {
           // rdbtn_line.Checked = false;
             chart1.Series["Dynamic_FACH"].ChartType = SeriesChartType.Column;
            chart1.Series["Proposed_Dual"].ChartType = SeriesChartType.Column;
            chart1.Series["DualWthZones"].ChartType = SeriesChartType.Column;

        }

        private void chkbx_savedetails_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void rdbtn_drawzones_CheckedChanged(object sender, EventArgs e)
        {
           
           // this.Refresh();
        }

        private void btnDrawZones_Click(object sender, EventArgs e)
        {
            int zoneNumber=(int)UpDown_ZoneNumber.Value;
            for (int k = 1; k <= zoneNumber; k++)
                mydraw.onEventPaint(myGraphics, k * 25, Color.DarkBlue);

        }

      

       
        
         


    }
}
