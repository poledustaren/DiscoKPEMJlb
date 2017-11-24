using System.Drawing;

namespace Study
{
    public class Options
    {
        public Options()
        {
            MyPen1 = new Pen(Color.Red, 3);
            MyPen2 = new Pen(Color.Blue, 3);
            MyPen3 = new Pen(Color.Green, 3);
        }
        public bool FunState { get;set; }
        public double Angle3 { get; set; }
        public double Angle2 { get; set; }
        public double Angle1 { get; set; }
        public Pen MyPen1 { get; set; }
        public Pen MyPen2 { get; set; }
        public Pen MyPen3 { get; set; }
        public Graphics GraphicsObject { get; set; }
        public double Step1 { get; set; }
        public double Step2 { get; set; }
        public double Step3 { get; set; }
    }
}