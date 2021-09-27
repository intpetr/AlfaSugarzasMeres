using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppvalll
{




    class visualizer
    {
        Bitmap completeImage;

        List<Point> coordinates = new List<Point>();

        Dictionary<Point, int> Points = new Dictionary<Point, int>();


        public visualizer(List<Point> points, int x, int y, Color color)
        {

            this.coordinates = points;
            completeImage = createImage(points, x, y, color);

        }

        private Bitmap createImage(List<Point> coordinates, int x, int y, Color color)
        {
            Bitmap image = new Bitmap(x, y);

            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Black);




            foreach (Point point in coordinates)
            {

                Color currentColor;
               



                try
                {



                    currentColor = image.GetPixel(x, y);
                }


                catch
                {
                    currentColor = Color.AliceBlue;


                }


                switch (currentColor.Name)
                {

                    case "AliceBlue":
                        
                        image.SetPixel(point.X, point.Y, Color.AntiqueWhite);

                        
                      // image.SetPixel(point.X, point.Y + x, Color.AliceBlue);
                        //image.SetPixel(point.X, point.Y - x, Color.AliceBlue);

                      //  image.SetPixel(point.X - 1, point.Y, Color.AliceBlue);
                       // image.SetPixel(point.X + 1, point.Y, Color.AliceBlue);

                        


                        break;

                    default:

                        image.SetPixel(point.X, point.Y, Color.AliceBlue);

                        break;

                }

            }

           



            return image;
        }



        public Bitmap getiImage()
        {
            return completeImage;
        }

    }


    enum Colors
    {
        




    }

}
