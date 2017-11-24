using System;
using System.Collections.Generic;
using System.Drawing;

namespace Study
{
    public class SimpleShapeDraw
    {
        private Form1 _form1;

        public SimpleShapeDraw(Form1 form1)
        {
            _form1 = form1;
        }
        public Color RandomColor()
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);
            return randomColor;
        }
        public void StartDraw()
        {
            var color = _form1.Options.FunState ? RandomColor(): _form1.BackColor;

            _form1.Options.GraphicsObject.Clear(color);


            var currentPoint = new CoorinateFloat((float)_form1.Size.Width / 2, (float)_form1.Size.Height / 2);
            var centerOfCircle = new Point((int)currentPoint.X - 15, (int)currentPoint.Y - 15);
            var arrow = new CustomArrow(currentPoint);

            //DrawFigure(arrow.Coordinates);
            DrawCircle(centerOfCircle);
            DrawBigCircle(currentPoint);


            var rotated1 = RotateFigure(arrow.Coordinates, new Point((int)currentPoint.X, (int)currentPoint.Y), _form1.Options.Angle1);
            _form1.DrawFigureFirstTask(rotated1, _form1.Options.MyPen1);
            _form1.Options.Angle1 = _form1.Options.Angle1 + _form1.Options.Step1;

            var rotated2 = RotateFigure(arrow.Coordinates, new Point((int)currentPoint.X, (int)currentPoint.Y), _form1.Options.Angle2);
            _form1.DrawFigureFirstTask(rotated2, _form1.Options.MyPen2);
            _form1.Options.Angle2 = _form1.Options.Angle2 + _form1.Options.Step2;

            var rotated3 = RotateFigure(arrow.Coordinates, new Point((int)currentPoint.X, (int)currentPoint.Y), _form1.Options.Angle3);
            _form1.DrawFigureFirstTask(rotated3, _form1.Options.MyPen3);
            _form1.Options.Angle3 = _form1.Options.Angle3 + _form1.Options.Step3;
        }

        public List<CoorinateFloat> RotateFigure(List<CoorinateFloat> points, Point currentPoint, double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            var result = new List<CoorinateFloat>();

            foreach (var coord in points)
            {
                //учитываем координаты точки относительно которой будем вращать
                var x = coord.ForDrawX - currentPoint.X;
                var y = coord.ForDrawY - currentPoint.Y;

                var xnew = x * cos - y * sin;
                var ynew = x * sin + y * cos;

                result.Add(new CoorinateFloat((float)xnew + currentPoint.X, (float)ynew + currentPoint.Y));
            }

            return result;
        }

        private void DrawCircle(Point centerOfCircle)
        {
            var ellipseSize = 30;
            var graphics = _form1.CreateGraphics();
            var rectangle = new Rectangle(centerOfCircle, new Size(ellipseSize, ellipseSize));

            graphics.DrawEllipse(_form1.Options.MyPen1, rectangle);
        }

        private void DrawBigCircle(CoorinateFloat centerOfCircle)
        {
            var ellipseSize = 480;
            var graphics = _form1.CreateGraphics();
            var point = new Point((int)centerOfCircle.X - ellipseSize / 2, (int)centerOfCircle.Y - ellipseSize / 2);
            var rectangle = new Rectangle(point, new Size(ellipseSize, ellipseSize));
            graphics.DrawRectangle(_form1.Options.MyPen1, rectangle);
            graphics.DrawEllipse(_form1.Options.MyPen1, rectangle);
        }
    }
}