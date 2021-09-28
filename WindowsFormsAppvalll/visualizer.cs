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

        Dictionary<Point, int> coordinatesAndValues = new Dictionary<Point, int>();

        Dictionary<Point, int> Points = new Dictionary<Point, int>();


        public visualizer(List<Point> points, int x, int y, Color color)
        {

            this.coordinates = points;

            prepareList(points);
            
            createImage(coordinatesAndValues, x, y, color);

        }

        public void prepareList(List<Point> points)
        {
            Console.WriteLine("it started");

            while (points.Count > 2)
            {
                Console.WriteLine(points.Count);
                Point currentPoint = points.First();

                points.Remove(points.First());

                int matches = 1;

                for (int i = 0; i < points.Count; i++)
                {
                    
                    if (points[i].X == currentPoint.X && points[i].Y == currentPoint.Y)
                    {
                        points.Remove(points[i]);
                        matches++;
                    }


                }

                coordinatesAndValues.Add(currentPoint, matches);

                currentPoint.X = 0;
                currentPoint.Y = 0;
               
            }
            
        }



       


        





        private async Task createImage(Dictionary<Point, int> coordinates, int x, int y, Color color)
        {
            Bitmap image = new Bitmap(x, y);

            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Black);



            foreach (var coordinate in coordinates)
            {



                //image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Red);



                
                switch (coordinate.Value)
                {

                    case 0:

                        image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Green);
                        break;

                    case 1:

                        image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Gray);
                        break;

                    case 2:

                        image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Purple);
                        break;

                    case 3:

                        image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Orange);
                        break;

                    case 4:

                        image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Red);
                        break;

                    default:

                        image.SetPixel(coordinate.Key.X, coordinate.Key.Y, Color.Coral);

                        break;


                }
                



            }


            completeImage = image;


           // Graphics g = Graphics.FromImage(image);
            //g.Clear(Color.Black);



            /*
            foreach (Point point in coordinates)
            {

                Color currentColor;
               



                try
                {



                    currentColor = image.GetPixel(x, y);
                }


                catch
                {
                    currentColor = Color.White;


                }


                switch (currentColor.Name)
                {

                    case "Black":
                        
                        image.SetPixel(point.X, point.Y, Color.Black);



                        image.SetPixel(point.X-1, point.Y ,Color.Black);
                        image.SetPixel(point.X + 1, point.Y, Color.Black);




                        break;

                    default:

                        image.SetPixel(point.X, point.Y, Color.AliceBlue);

                        break;

                }

            }

           



            completeImage =  image;

            */
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