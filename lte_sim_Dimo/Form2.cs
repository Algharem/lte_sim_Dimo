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
    public partial class Form2 : Form
    {
        int xcenter, ycenter;
        private Graphics myGraphics;
        public delegate void PaintEventHandler(object sender, PaintEventArgs e);
        public event PaintEventHandler eventPaint;


        public Form2()
        {
            InitializeComponent();
            this.eventPaint += onEventPaint;
          //  eventPaint(this, new PaintEventArgs(myGraphics, new Rectangle(e.X, e.Y, xcenter, ycenter)));

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            xcenter = (int)Width / 2;
            ycenter = (int)Height / 2;

        }
        protected void onEventPaint(object sender, PaintEventArgs e)
        {
            double x, y, radius;
            x = Math.Abs(e.ClipRectangle.X - xcenter);
            y = Math.Abs(e.ClipRectangle.Y - ycenter);
            radius = Math.Sqrt(x * x + y * y);
            Graphics g = e.Graphics;
            Pen myPen = new Pen(Color.Red, 2);
            g.DrawLine(myPen, e.ClipRectangle.X, e.ClipRectangle.Y, xcenter, ycenter);// g.DrawEllipse(myPen, e.ClipRectangle);
            // g.DrawEllipse(myPen, e.ClipRectangle);
            for (double i = 0.0; i < 360.0; i += 0.1)
            {
                double angle = i;// *System.Math.PI / 180;
                int X = (int)(xcenter + radius * System.Math.Cos(angle));
                int Y = (int)(ycenter + radius * System.Math.Sin(angle));

                PutPixel(g, X, Y, Color.Blue);
                //System.Threading.Thread.Sleep(1); // If you want to draw circle very slowly.
            }
            g.Dispose();
            lblLength.Text = radius.ToString();
            //base.OnPaint(e);
        }
        void PutPixel(Graphics g, int x, int y, Color c)
        {
            Bitmap bm = new Bitmap(10, 10);

            bm.SetPixel(5, 5, c);
            g.DrawImageUnscaled(bm, x, y);
        }



    /*    private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics myGraphics = e.Graphics;

            myGraphics.Clear(Color.White);
            double radius = 5;
            //for (int j = 1; j <= 25; j++)
            // {
            radius = 200;
            for (double i = 0.0; i < 360.0; i += 0.1)
            {
                double angle = i;// *System.Math.PI / 180;
                int x = (int)(150 + radius * System.Math.Cos(angle));
                int y = (int)(150 + radius * System.Math.Sin(angle));

                // PutPixel(myGraphics, x, y, Color.Blue);
                //System.Threading.Thread.Sleep(1); // If you want to draw circle very slowly.
            }
            //}
            // PutPixel(myGraphics, Width/2, Height/2, Color.Black);
            myGraphics.Dispose();
        }
        */
        
        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            // xcenter = e.X;
            // ycenter = e.Y;

            this.Refresh();

            if (eventPaint != null)
            {
                myGraphics = this.CreateGraphics();
                eventPaint(this, new PaintEventArgs(myGraphics, new Rectangle(e.X, e.Y, xcenter, ycenter)));
            }
            //base.OnMouseMove(e);
        }

       


    }

}
 