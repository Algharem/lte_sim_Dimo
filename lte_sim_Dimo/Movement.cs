using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace lte_sim_Dimo
{
  public  class Movement
    {
      double MiniX, MaxX, MiniY, MaxY;
      double Radius;
     private Point BaseStation_Position;
      List<Point> randList = new List<Point>();

      /// <summary>
      /// Generate the BS area [Center ,and the border]
      /// </summary>
      /// <param name="BS_position">The center of the BaseStation</param>
      /// <param name="radius">The distane fron the BS center to the radius</param>

      public Movement(Point BS_position, double radius)
      {
          Radius = radius;
          BaseStation_Position=BS_position;
           MiniX = BS_position.X - radius;
           MaxX = BS_position.X + radius;
           MiniY = BS_position.Y - radius;
           MaxY = BS_position.Y + radius;
      }

      /// <summary>
      /// Generate the users movement in 8 random way and by determine speed
      /// </summary>
      /// <param name="oldPoint">represent the current position of user</param>
      /// <param name="userspeed">user speed</param>
      /// <returns></returns>
      public Point GetRandPoint(Point oldPoint, int userspeed)
      {
          Point p = new Point();
          p = oldPoint;
          Random rand = new Random();
          int randWay = rand.Next(1, 8);

          switch (randWay)
          {
              case 0: p = oldPoint;
                  break;
              case 4: p.X += userspeed;
                  p.Y += userspeed;
                  break;
              case 2: p.X -= userspeed;
                  p.Y -= userspeed;
                  break;
              case 3: p.X += userspeed;
                  p.Y -= userspeed;
                  break;
              case 1: p.X -= userspeed;
                  p.Y += userspeed;
                  break;
              case 5: p.X += userspeed;

                  break;
              case 6: p.X -= userspeed;

                  break;
              case 7:
                  p.Y -= userspeed;
                  break;
              case 8:
                  p.Y += userspeed;
                  break;
          }
          //if ((p.X < CellPanel.Width/2 && p.X > 0) && (p.Y < CellPanel.Height/2 && p.Y > 0))
          // return p;
          //else
          // return oldPoint;
          return p;

      }
      /// <summary>
      /// Get The user distance from the BS
      /// </summary>
      /// <param name="newPoint">The new user Position </param>
      /// <returns></returns>
      private double Distance(Point newPoint)
      {

          int x = Math.Abs(newPoint.X - BaseStation_Position.X);
          int y = Math.Abs(newPoint.Y - BaseStation_Position.Y);
          
          return Radius = Math.Round(Math.Sqrt(x * x + y * y), 2);
         

      }

      /// <summary>
      ///  Generate a random position for users at first time
      /// </summary>
      /// <returns></returns>
      public Point GetRandPoint()
      {

          Point RandPoint = new Point();
          // Point p = new Point();
          do
          {
              Random rand = new Random();
              // double r = Math.Round(rand.NextDouble(), 1) * 10;
             for(int i=0;i<6;i++)
             {
                 RandPoint.X = rand.Next((int)MiniX , (int)MaxX );//xcenter-radius,xcenter+radius);// CellPanel.Width);
                 RandPoint.Y = rand.Next((int)MiniY, (int)MaxY); //  RandPoint.Y = rand.Next((int)MiniY,(int) MaxY);
             }
          }
          while (InPointList(RandPoint) == true);// to make sure that each user has different position
          randList.Add(RandPoint);
          return RandPoint;

      }

      bool InPointList(Point NewPoint)
      {
          bool found = false;
          foreach (Point p in randList)
              if (p == NewPoint)
                  found = true;

          return found;
      }





      internal Point GetRandPoint_CellEdge()
      {
          Point RandPoint = new Point();
          // Point p = new Point();
          do
          {
              Random rand = new Random();
              // double r = Math.Round(rand.NextDouble(), 1) * 10;
              for (int i = 0; i < 6; i++)
              {
                  int r = rand.Next(150, 250);
                  int theta = rand.Next(0, 360);
                  RandPoint.X = Convert.ToInt16(r * Math.Cos(theta));
                  RandPoint.Y = Convert.ToInt16(r * Math.Sin(theta));
              }
             
          }
          while (InPointList(RandPoint) == true);// to make sure that each user has different position
          randList.Add(RandPoint);
          RandPoint = GetActualPoint(RandPoint);
          return RandPoint;

               
       
      }
      /// <summary>
      /// To adjust the point from (0,0) center to BS center like (290,250)
      /// </summary>
      /// <param name="oldpoint"></param>
      /// <returns></returns>

      private Point GetActualPoint(Point oldpoint)
      {
         /// if(oldpoint.X>0)
           oldpoint.X = oldpoint.X + BaseStation_Position.X;
          //else
            //  oldpoint.X = oldpoint.X + BaseStation_Position.X;
          if(oldpoint.Y>0)
              oldpoint.Y = BaseStation_Position.Y-oldpoint.Y;
          else
               oldpoint.Y = Math.Abs(oldpoint.Y) + BaseStation_Position.Y;
          
          return oldpoint;
      }
      /// <summary>
      /// 
      /// </summary>
      private Point GetRandCellEdgePoint()// no need for this until now
        {
             Point p = new Point();
         
          Random rand = new Random();
          int randWay = rand.Next(1, 4);

          switch (randWay)
          {
              case 1: p.X = rand.Next((int)MiniX, (int)MiniX+100);
                       p.Y = rand.Next((int)MiniY, (int)MaxY);
                  break;
              case 2: p.X = rand.Next((int)MaxX-100, (int)MaxX);
                       p.Y = rand.Next((int)MiniY, (int)MaxY);
                  break;
             case 3:  p.X = rand.Next((int)MiniX, (int)MaxX);
                       p.Y = rand.Next((int)MiniY, (int)MiniY+100);
                  break;
             case 4:  p.X = rand.Next((int)MiniX, (int)MaxX);
                      p.Y = rand.Next((int)MaxY-100, (int)MaxY);
                  break;
          }

          return p;
           
        }
    
     internal Point GetRandPoint_CellCenter()
      {
          Point RandPoint = new Point();
          // Point p = new Point();
          do
          {
              Random rand = new Random();
              // double r = Math.Round(rand.NextDouble(), 1) * 10;
              for (int i = 0; i < 6; i++)
              {
                  int mini = (int)(MiniX + 0.7 * Radius );
                  int max=(int)(MaxX - 0.7 * Radius );
                  RandPoint.X = rand.Next(mini, max);//xcenter-radius,xcenter+radius);// CellPanel.Width);
                  RandPoint.Y = rand.Next((int)(MiniY + 0.7 * Radius ), (int)(MaxY - 0.7 * Radius )); //  RandPoint.Y = rand.Next((int)MiniY,(int) MaxY);
              }
          }
          while (InPointList(RandPoint) == true);// to make sure that each user has different position
          randList.Add(RandPoint);
          return RandPoint;
      }
    }
}
