using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//TODO: 1) добавить метод сдвиг стрелочки и циферблата в другую область экрана 
//TODO: убрать деформацию

//TODO: 2) нарисовать график функции 
//TODo: задается интервал a,b в интерфейс и есть функция (cx^2-1)/(x+1)+(1/x)

//TODO:3) алгоритм 3  выпуклой плоскости

namespace Study
{
    public partial class Form1 : Form
    {
        private readonly Options _options;
        private readonly SimpleShapeDraw _simpleShapeDraw;
        public Size OldSize { get; set; }
        public Options Options
        {
            
            get { return _options; }
        }

        public SimpleShapeDraw SimpleShapeDraw
        {
            get { return _simpleShapeDraw; }
        }

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            _options = new Options();
            _simpleShapeDraw = new SimpleShapeDraw(this);
            Options.GraphicsObject = CreateGraphics();
            OldSize = Size;
        }
       
        


        private void button1_Click(object sender, EventArgs e)
        {
            

            if (backgroundWorker1.IsBusy)
            {
                discotecButton.Visible = false;
                button2.Visible = true;
                button3.Visible = true;
                backgroundWorker1.CancelAsync();
            }
            else
            {
                button2.Visible = false;
                button3.Visible = false;
                discotecButton.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            Random randomGen = new Random();

            Options.Step1 = (double)randomGen.Next(1, 3) / 10;
            Options.Step2 = (double)randomGen.Next(1, 3) / 10;
            Options.Step3 = (double)randomGen.Next(1, 3) / 10;
            

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    
                    Options.GraphicsObject.Clear(DefaultBackColor);
                    e.Cancel = true;
                    break;
                }
                else
                {
                    
                    _simpleShapeDraw.StartDraw();
                    Thread.Sleep(50);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = !groupBox3.Visible;
            if(!groupBox3.Visible)
                Options.GraphicsObject.Clear(DefaultBackColor);
            button3.Visible = !button3.Visible;
            button1.Visible = !button1.Visible;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Options.GraphicsObject.Clear(DefaultBackColor);
            var currentPoint = new CoorinateFloat((float)Size.Width / 2, (float)Size.Height / 2);
            var coordsList = Function2Task(currentPoint);
            DrawFigureTask2(coordsList, _options.MyPen1);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Visible = !button2.Visible;
            button1.Visible = !button1.Visible;

            Options.GraphicsObject.Clear(DefaultBackColor);

            var coordinates = DrawPoints();
            var niceFigure = ConvexPolygon(coordinates);
            DrawFigure2(niceFigure,Options.MyPen1);

        }

        private static List<CoorinateFloat> ConvexPolygon(List<CoorinateFloat> coordinates)
        {
            var firstPoint = FindStartPoint(coordinates);
            var pointsList = SortedPointsByCos(coordinates, firstPoint);
            var niceFigure = new List<CoorinateFloat>();
            niceFigure.Add(firstPoint);
            niceFigure.Add(pointsList.First());

            var firstPointForAlgo = firstPoint;
            var currentVektor = new Vektor(firstPointForAlgo, pointsList.First());

            foreach (var point in pointsList)
            {
                var nextVektor = new Vektor(firstPointForAlgo, point);

                if (VektorOperation.IsNiceComposition(currentVektor, nextVektor))
                {
                    currentVektor = nextVektor;
                    firstPointForAlgo = point;
                    niceFigure.Add(point);
                }
            }
            niceFigure.Add(firstPoint);
            return niceFigure;
        }


        private static List<CoorinateFloat> SortedPointsByCos(List<CoorinateFloat> coordinates, CoorinateFloat firstPoint)
        {
            var vektorList = GetVektors(coordinates, firstPoint);
            var verticalVektor = new Vektor(firstPoint, new CoorinateFloat(firstPoint.X, firstPoint.Y+1));
            var sortedVektors = vektorList.OrderByDescending(z => VektorOperation.Cosinus(verticalVektor, z)).Select(z=>z.PointEnd).ToList();
            return sortedVektors;
        }

        private static List<Vektor> GetVektors(List<CoorinateFloat> coordinates, CoorinateFloat firstPoint)
        {
            var vektorList = new List<Vektor>();
            foreach (var point in coordinates.Where(z => z != firstPoint))
            {
                var vektor = new Vektor(firstPoint, point);
                vektorList.Add(vektor);
            }
            return vektorList;
        }

        public static class VektorOperation
        {
            public static float Cosinus(Vektor first, Vektor second)
            {
                var result = (first.OX * second.OX + first.OY * second.OY) / (Math.Abs(first.OX + second.OX) *
                             Math.Abs(first.OY + second.OY));
                return result;
            }
          

            public static bool IsNiceComposition(Vektor vek1, Vektor vek2)
            {
                var result = vek1.OX * vek2.OX + vek1.OY * vek2.OY;
                if (result > 0)
                    return true;
                return false;
            }
        }
        public class Vektor
        {
            public float OX { get;set;}
            public float OY { get;set; }
            public CoorinateFloat PointEnd { get; set; }
           
            public Vektor(float Ox, float Oy)
            {
                OX = Ox;
                OY = Oy;
            }
            public Vektor(CoorinateFloat f, CoorinateFloat s)
            {
                OX =  f.X- s.X;
                OY =  f.Y- s.Y;
                PointEnd = s;
            }
        }

        private static CoorinateFloat FindStartPoint(List<CoorinateFloat> coordinates)
        {
            var minCoordX = coordinates.Min(z => z.X);
            var findFirstPoint = coordinates.Where(z => z.X == minCoordX);
            var firstPoint = new CoorinateFloat(0, 0);

            if (findFirstPoint.Count() > 1)
            {
                var minY = findFirstPoint.Min(z => z.Y);
                firstPoint = findFirstPoint.FirstOrDefault(z => z.Y == minY);
            }
            else
            {
                firstPoint = findFirstPoint.FirstOrDefault();
            }
            return firstPoint;
        }

        private List<CoorinateFloat> DrawPoints()
        {
            var centerPoint = new CoorinateFloat((float) Size.Width / 2, (float) Size.Height / 2);


            var coordinates = new List<CoorinateFloat>();

            var point = new CoorinateFloat(centerPoint);

            Brush aBrush = (Brush) Brushes.Black;
            for (var i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var cellX = dataGridView1.Rows[i].Cells[0].Value;
                var cellY = dataGridView1.Rows[i].Cells[1].Value;
                if (cellX != null && cellY != null)
                {
                    var x = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    var y = int.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    coordinates.Add(point.Coordinate(x, y));
                }
            }
            foreach (var dot in coordinates)
            {
                Options.GraphicsObject.FillRectangle(aBrush, dot.ForDrawX, dot.ForDrawY, 3, 3);
            }
            return coordinates;
        }

        private void discotecButton_Click(object sender, EventArgs e)
        {
            Options.FunState = !Options.FunState;
        }

        
        public List<CoorinateFloat> Function2Task( CoorinateFloat centerPoint)
        {
            var goodWith = Size.Width - OldSize.Width;
            var goodHeight = Size.Height - OldSize.Height;

            var haveA = float.TryParse(task2fromA.Text, out float a);
            var haveB = float.TryParse(task2toB.Text, out float b);
            var haveC = float.TryParse(task2constC.Text, out float c);

            var coordinates = new List<CoorinateFloat>();

            if (!haveA || !haveB || !haveC)
                return coordinates;
           
            var dot = new CoorinateFloat(centerPoint);

            for (float i = a; i <= b; i=(float)Math.Round(i+1f/100f,2))
            {
                
                if (i == 0 || (i + 1) == 0)
                {
                    coordinates.Add(dot.Coordinate(0, 0,true));
                }
                else
                {
                    var y = (c * (i * i) - 1) / (i + 1) + (1 / i)*20;
                    var x = i*20;
                    coordinates.Add(dot.Coordinate(x, y));
                }
              
            }

            return coordinates;
        }

        public void DrawFigure2(List<CoorinateFloat> points, Pen pen)
        {
            var fromPoint = points.First();

            foreach (var toPoint in points)
            {

                Options.GraphicsObject.DrawLine(pen, fromPoint.ForDrawX, fromPoint.ForDrawY, toPoint.ForDrawX, toPoint.ForDrawY);
                fromPoint = toPoint;
            }
        }
        public void DrawFigure(List<CoorinateFloat> points, Pen pen)
        {
            var goodWith = Size.Width / OldSize.Width;

            var fromPoint = points.First();

            foreach (var toPoint in points)
            {
                if (toPoint == fromPoint || toPoint.BadPoint || fromPoint.BadPoint)
                {
                    fromPoint = toPoint;
                    continue;
                }
                var point =new PointF(fromPoint.ForDrawX, fromPoint.ForDrawY);
                var pointTo = new PointF(toPoint.ForDrawX, toPoint.ForDrawY);
                Options.GraphicsObject.DrawLine(pen, point, pointTo);
                fromPoint = toPoint;
            }
        }
        public void DrawFigureTask2(List<CoorinateFloat> points, Pen pen)
        {
            var goodWith = Size.Width / OldSize.Width;

            var fromPoint = points.First();

            foreach (var toPoint in points)
            {
                if (toPoint == fromPoint || toPoint.BadPoint || fromPoint.BadPoint)
                {
                    fromPoint = toPoint;
                    continue;
                }
                var point = new PointF(fromPoint.ForDrawX, fromPoint.ForDrawY);
                var pointTo = new PointF(toPoint.ForDrawX, toPoint.ForDrawY);
                Options.GraphicsObject.DrawLine(pen, point, pointTo);
                fromPoint = toPoint;
            }
        }
        public void DrawFigureFirstTask(List<CoorinateFloat> points, Pen pen)
        {
           
            var fromPoint = points.First();

            foreach (var toPoint in points)
            {
                if (toPoint == fromPoint || toPoint.BadPoint || fromPoint.BadPoint)
                {
                    fromPoint = toPoint;
                    continue;
                }

                Options.GraphicsObject.DrawLine(pen, fromPoint.X  , fromPoint.Y , toPoint.X  , toPoint.Y );
                fromPoint = toPoint;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add("20", "60");
            dataGridView1.Rows.Add("60", "100");
            dataGridView1.Rows.Add("100", "120");
            dataGridView1.Rows.Add("80", "80");
            dataGridView1.Rows.Add("140", "80");
            dataGridView1.Rows.Add("120", "40");
            dataGridView1.Rows.Add("70", "20");
           

        }
    }
}

