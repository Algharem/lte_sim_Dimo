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
    class Draw
    {
       
        public delegate void PaintEventHandler(Graphics gcircle,int x,int y);
        public event PaintEventHandler eventPaint;
        int xcenter, ycenter;
         public Draw(int width,int height)
        {
            xcenter = (int)width / 2;
            ycenter = (int)height / 2;
          // eventPaint += onEventPaint;

        }

         public  void Draw_Grahp(Graphics myGraphics,int x,int y)
        {

           

            if (eventPaint != null)
            {
               
                //eventPaint(myGraphics,x,y);
            }
            
        }
        public void onEventPaint(Graphics gcircle,double radius,Color color)//( PaintEventArgs e)
        {
            Pen myPen = new Pen(Color.Red, 2);
            
            for (double i = 1; i < 360.0; i ++)//i+= 0.1
            {
                int X = (int)(xcenter + radius * System.Math.Cos(i));
                int Y = (int)(ycenter + radius * System.Math.Sin(i));
                PutPixel(gcircle, X, Y, color);
            }
          //  gcircle.Dispose();
        }
        void PutPixel(Graphics g, int x, int y, Color c)
        {
            Bitmap bm = new Bitmap(20, 20);

            //bm.SetResolution(1.);
            bm.SetPixel(10, 10, c);
            
            g.DrawImageUnscaled(bm, x, y);
        }

    }
}
