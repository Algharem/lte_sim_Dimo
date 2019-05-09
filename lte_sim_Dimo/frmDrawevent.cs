using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lte_sim_Dimo
{
    public partial class frmDrawevent : Form
    {
        private Graphics myGraphics;

        public delegate void PaintEventHandler(object sender, PaintEventArgs e);
        public event PaintEventHandler eventPaint;

        protected void onEventPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Red, 50);
            g.DrawRectangle(myPen, e.ClipRectangle);
            g.Dispose();
            //base.OnPaint(e);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {

            this.Refresh();

            if (eventPaint != null)
            {
                myGraphics = this.CreateGraphics();
                eventPaint(this, new PaintEventArgs(myGraphics, new Rectangle(e.X, e.Y, 10, 10)));
            }
            //base.OnMouseMove(e);
        }
        public frmDrawevent()
        {
            InitializeComponent();
        }

        private void frmDrawevent_Load(object sender, EventArgs e)
        {
             InitializeComponent();
             this.eventPaint += onEventPaint;

        }
     
  }
}
    
