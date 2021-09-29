using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AforgeCam;
using AForge.Video;
using AForge.Video.DirectShow;
using System;
using AForge.Imaging.Filters;
using AForge;
using System.Threading.Tasks;
using Lego.Ev3;
using Lego.Ev3.Core;
//using Lego.Ev3.Desktop;
using System.Runtime.CompilerServices;
using Lego.Ev3.Desktop;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using WindowsFormsAppvalll;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;

namespace AforgeCam
{
    public partial class Form1 : Form
    {
        
        float currentAngle;
        Boolean motor = false;
        Bitmap video;
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private Queue<Image> _oldImages = new Queue<Image>();
    

        

      
        //public int PixelsCountWithoutBlack { get; }

        Bitmap videoclone;

        int fps = 0;
        string eredmenyek = "";
        int delay = 8000;
        int checkCounter = 0;
        int checknumber = 0;
        string exceptions;
        bool morethan2checks = false;
        string ev3value;
        float motordegrees;
        float degreesTocm;
        System.Drawing.Color color;
        float range = (float)0.2;
        Boolean on = false;

        int finalresults = -1;

        List<int> measurements = new List<int>();
        List<Bitmap> bitmaps = new List<Bitmap>();

        Brick brick = null;
        Boolean BrickConnected = false;


        List<System.Drawing.Point> coordinates = new List<System.Drawing.Point>();

        float highestBrightnessValue = 0.5F;

        List<string> measurement = new List<string>();

        //ezt akkor kell kikommentelni ha csatlakoztatva van a lego robot
       // Brick brick = new Brick(new UsbCommunication());

        public Form1() // init
        {
            InitializeComponent();
            {
                VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
                {
                    comboBox1.Items.Add(VideoCaptureDevice.Name);
                }
                try { comboBox1.SelectedIndex = 0; }

                catch (System.ArgumentOutOfRangeException)
                {
                    //label1.Text = ("Csatlakoztasd a webkamerát és indítsd újra a programot!");
                }
                connectToBrick();
                label3.Text = "0.5";
            }

            

        }

        private void Brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            //throw new NotImplementedException();
            
            motordegrees = e.Ports[InputPort.A].SIValue;
            
              //  MessageBox.Show(e.Ports[InputPort.A].SIValue.ToString());
            //label1.Text = e.Ports[InputPort.A].SIValue.ToString();

            degreesTocm = motordegrees / 109;
            label4.Text = "Távolság: "+ Math.Round(degreesTocm);



        }

        public async void connectToBrick()
        {
            brick = new Brick(new UsbCommunication());


            try
            {
                await brick.ConnectAsync();
                BrickConnected = true;
                checkBox1.Checked = true;
                label2.Text = "Az EV3 csatlakoztatva van";

                brick.Ports[InputPort.A].SetMode(MotorMode.Degrees);
                brick.Ports[InputPort.One].SetMode(UltrasonicMode.Centimeters);

                brick.BrickChanged += Brick_BrickChanged;
            }
            catch
            {
                checkBox1.Checked = false;
                BrickConnected = false;
                label2.Text = "Nem sikerült csatlakozni az EV3 hoz";
            }
                
            
          

        }




        public void motorfok()
        {
            if (brick != null)
            {
                
                brick.Ports[InputPort.A].SetMode(MotorMode.Degrees);
                brick.Ports[InputPort.One].SetMode(UltrasonicMode.Centimeters);
                
            }

        }






        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            fps++;
             
            if(fps == 1)
            {
                
                 image((Bitmap)eventArgs.Frame.Clone());

                fps = 0;
            }

            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            
            // image((Bitmap)eventArgs.Frame.Clone());

            //bitmaps.Add((Bitmap)eventArgs.Frame.Clone());




            _oldImages.Enqueue(pictureBox1.Image);
            if (_oldImages.Count > 3)
            {
                var oldest = _oldImages.Dequeue();
                oldest.Dispose();

            }else
            {
                highestBrightnessValue = getHighestBrightness((Bitmap)eventArgs.Frame.Clone());
            }

        }
      


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            //MessageBox.Show(exceptions);
            motorfok();
            //MessageBox.Show(ev3value);

            
            MessageBox.Show(eredmenyek);
            //brick.Ports[InputPort.One].SetMode(MotorMode.Degrees);
        }


      
        private void button3_Click(object sender, EventArgs e)
        {
    


        }

        //4545
        private void button1_Click_1(object sender, EventArgs e)
        {

            try
            {
               
                FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[0].MonikerString);
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);




                FinalVideo.Start();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                label2.Text = "A kamera nicsen csatlakoztatva. Indítsd újra a programot!";
            }



            Thread thread = new Thread(async() =>
            {
                System.Threading.Thread.Sleep(Int32.Parse(textBox1.Text)*1000);
                try
                {


                    FinalVideo.Stop();
                }
                catch
                {
                    label2.Text = "A mérés nem volt elindítva";
                }
               // listBox1.Items.Add("Távolság: " + degreesTocm + " Pixelek " + finalresults);

                measurements.Clear();
                

            });
            thread.Start();

        }




        private void button2_Click_1(object sender, EventArgs e)
        {

            if (FinalVideo != null)
            {
                FinalVideo.Stop();
            }
              
            // label1.Text = count.ToString();
            int count2 = 0;





            listBox1.Items.Add("Távolság: "+degreesTocm +"  " + " Pixelek " + finalresults + "  " + "  Ido: "+ textBox1.Text);
            saveData();
            finalresults = 0;

            measurements.Clear();

            //saveData();


            /*
                        foreach (string number in measurement)
                        {

                            listBox1.Items.Add("meres:" + number.ToString());
                        }
                        bitmaps.Clear();
            */
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
           /* delay = (trackBar1.Value * 10);
            label2.Text = (trackBar1.Value.ToString());
           */
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
          
        }


        public async Task image(Bitmap bmp)
        {

           // deleteOldFrames();
          

            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat);
            int count = 0;
            LockBitmap source = new LockBitmap(bmp),
                target = new LockBitmap(newBitmap);
            source.LockBits();
            target.LockBits();

          //  System.Drawing.Color white = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    System.Drawing.Color color = source.GetPixel(x, y);

                    if (color.GetBrightness() > highestBrightnessValue)
                    {

                        coordinates.Add(new System.Drawing.Point(x, y));
                        count++;
                    }
                }
            }
            source.UnlockBits();
            target.UnlockBits();
            // newBitmap.Save("d:\\result.png");

            finalresults += count;
           measurement.Add(count+"  "+degreesTocm.ToString());
           // saveData();
           // label1.Text = count.ToString();
        }

        public void deleteOldFrames()
        {
            if (pictureBox1.Image != null)
            {
                _oldImages.Enqueue(pictureBox1.Image);
                if (_oldImages.Count > 3)
                {
                    var oldest = _oldImages.Dequeue();
                    oldest.Dispose();
                }
            }
        }


        public float getHighestBrightness(Bitmap bmp)
        {

            

            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat);
            float highestPixel = 0;
            LockBitmap source = new LockBitmap(bmp),
               
                target = new LockBitmap(newBitmap);
            source.LockBits();
            target.LockBits();

            for (int y = 0; y < source.Height; y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    System.Drawing.Color color = source.GetPixel(x, y);

                   

                    if(color.GetBrightness() > highestPixel)
                    {
                        highestPixel = color.GetBrightness();
                    }

                }
            }
            source.UnlockBits();
            target.UnlockBits();


            return highestPixel;




        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
             currentAngle = motordegrees-50;

            // await brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, 30);
            await brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, -50, 350, false);
            
            /*while (motordegrees < currentAngle)
            {
                await brick.DirectCommand.StopMotorAsync(OutputPort.A, true);
            }
            */

            // await brick.DirectCommand.TurnMotorAtPowerAsync(OutputPort.A, 50);
            //  brick.DirectCommand.StopMotorAsync(OutputPort.A, true);
        }

        private async void button4_Click_1(object sender, EventArgs e)
        {
           // await brick.DirectCommand.StopMotorAsync(OutputPort.A, true);
            await brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.A, 50, 350, false);
        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
           
            highestBrightnessValue = (float)trackBar1.Value / 10;
            label3.Text = highestBrightnessValue.ToString();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            //Point point = new System.Drawing 
            // coordinates.Add(new System.Drawing.Point(5, 5));
            Thread thread = new Thread(async () =>
            {

                pictureBox1.Image = new visualizer(coordinates, 8000, 8000, System.Drawing.Color.Red).getiImage();

            });
            thread.Start();


           


            




            
            
            
            

           // MessageBox.Show(coordinates.ElementAt(coordinates.Count).X.ToString());
        }

        private async void button6_Click(object sender, EventArgs e)
        {
            Console.WriteLine("megnyomtad a gmobot");



            TextWriter txt = new StreamWriter("C:\\demo\\demo.txt");
            txt.Write(eredmenyek);
            txt.Close();

            Bitmap thisimage;
            thisimage = (Bitmap)pictureBox1.Image;
            thisimage.Save("kep", ImageFormat.Bmp);







            //pictureBox1.Image.Save("myfile.bmp", ImageFormat.Bmp);

            //saveDataToTxt();
        }

        public void saveData()
        {
            eredmenyek = eredmenyek+ "\nTávolság " + degreesTocm + " Pixelek  " + finalresults + " Idő " + textBox1.Text;

        }



        public void saveDataToTxt()
        {
            // File.WriteAllText("adatok.txt", eredmenyek);

        }

        private async void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(BrickConnected == false)
            {

                 connectToBrick();
                

            }
            else
            {
                label2.Text = "Az EV3 már csatlakoztatva van.";
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}


