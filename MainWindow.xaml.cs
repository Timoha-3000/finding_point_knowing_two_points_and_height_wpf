using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task_finding_point_knowing_two_points_and_height_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point3D> point3Ds = new List<Point3D>();
        List<Brush> brush = new List<Brush>()
            {
                Brushes.Red,
                Brushes.Purple,
                Brushes.Green,
                Brushes.Blue,
                Brushes.Black,
                Brushes.Yellow,
                Brushes.Orange,
                Brushes.Lime,
                Brushes.Brown,
                Brushes.Magenta,
            };
        int i = 0, j = 0;

        public MainWindow()
        {
            InitializeComponent();

            for (double i = 0; i < 10; i += 0.2)
            {
                foreach (Point3D point in Task_finding_point_knowing_two_points_and_height(new Point3D(0, 10, 0), new Point3D(10, 10, 0), 2, 5, i))
                    point3Ds.Add(point);
            }

            string line = "";

            foreach (var item in point3Ds)
            {
                line += " " + item.ToString() + "\n";
            }

            Console.WriteLine("\n\nhi\n\n" + line);
        }

        public void But_click(object sender, RoutedEventArgs e)
        {
            if (j >= brush.Count)
                j = 0;
            if (i >= point3Ds.Count)
                i = 0;
            
            int couf = 30;

            Line ellipse = new Line();
            ellipse.X1 = point3Ds[i].X * couf;
            ellipse.Y1 = point3Ds[i].Z * couf;
            ellipse.X2 = point3Ds[i + 1].X * couf;
            ellipse.Y2 = point3Ds[i + 1].Z * couf;

            ellipse.Stroke = brush[j];
            ellipse.StrokeThickness = 2;
            MainGrid.Children.Add(ellipse);
            Console.WriteLine("{0:C2}","i - " + i + $" ||| j - {j} |||  x = {point3Ds[i].X}  ||| z = {point3Ds[i].Z}  |||  x = {point3Ds[i+1].X}  ||| z = {point3Ds[i+1].Z}");
            i+=2;
            j++;
        }

        /// <summary>
        /// Андрей, я не знаю как это описать, если бы мы знали как это описать, мы не знаем как это описать
        /// </summary>
        /// <param name="A"> первая точка основания перемычки</param>
        /// <param name="B"> вторая точка основания перемычки</param>
        /// <param name="h"> малый отступ </param>
        /// <param name="H"> большой отступ </param>
        /// <param name="z"> высота, на которой мы ищем точки</param>
        /// <returns></returns>
        public List<Point3D> Task_finding_point_knowing_two_points_and_height(Point3D A, Point3D B, double h, double H, double z)
        {
            List<Point3D> finalPointList = new List<Point3D>();
            List<Point3D> listWithPointsA = new List<Point3D>();
            List<Point3D> listWithPointsB = new List<Point3D>();
            List<double> OffSet = new List<double>() { 0, h, (H + h), (H + h + h) };
            int countOfLine = 4;

            for (int i = countOfLine; i > 0; i--)
            {
                listWithPointsA.Add(new Point3D(A.X, A.Y, A.Z + OffSet[i - 1]));
                listWithPointsB.Add(new Point3D(B.X, B.Y, B.Z + OffSet[i - 1]));
            }

            Point3D intersectionPoint = FindIntersectionPoint(listWithPointsA[0], listWithPointsB[2]);

            if (z > intersectionPoint.Z - h && z < intersectionPoint.Z)
            {
                if (z > intersectionPoint.Z - h / 2)
                {
                    finalPointList.Add(FindingPointOnLine(listWithPointsA[3], listWithPointsB[1], z));
                    finalPointList.Add(FindingPointOnLine(listWithPointsA[1], listWithPointsB[3], z));
                }
                else
                {
                    finalPointList.Add(FindingPointOnLine(listWithPointsA[2], listWithPointsB[0], z));
                    finalPointList.Add(FindingPointOnLine(listWithPointsA[0], listWithPointsB[2], z));
                }
            }
            else
            {
                finalPointList.Add(FindingPointOnLine(listWithPointsA[0], listWithPointsB[2], z));
                finalPointList.Add(FindingPointOnLine(listWithPointsA[1], listWithPointsB[3], z));
                finalPointList.Add(FindingPointOnLine(listWithPointsA[2], listWithPointsB[0], z));
                finalPointList.Add(FindingPointOnLine(listWithPointsA[3], listWithPointsB[1], z));
            }

            finalPointList.Sort((p1, p2) =>
            {
                int result = p1.X.CompareTo(p2.X);
                if (result == 0)
                {
                    result = p1.Y.CompareTo(p2.Y);
                }
                return result;
            });

            return finalPointList;
        }

        /// <summary>
        /// Ищет точку (точку необходимо найти на высоте z) на прямой, которую находит по двум точка.
        /// Использует параметрическое уравнение прямой
        /// </summary>
        /// <param name="beginPoint"> начальная точка </param>
        /// <param name="endPoint"> конечная точка </param>
        /// <param name="z"> необходимая высота, на к-ой ищем точку на прямой </param>
        /// <returns></returns>
        public Point3D FindingPointOnLine(Point3D beginPoint, Point3D endPoint, double z)
        {
            double t_Parameter = (z - beginPoint.Z) / (endPoint.Z - beginPoint.Z);
            double x = beginPoint.X + t_Parameter * (endPoint.X - beginPoint.X);
            double y = beginPoint.Y + t_Parameter * (endPoint.Y - beginPoint.Y);

            return new Point3D(x, y, z);
        }

        public Point3D FindIntersectionPoint(Point3D beginPoint, Point3D endPoint)
        {
            return new Point3D((beginPoint.X + endPoint.X) / 2, (beginPoint.Y + endPoint.Y) / 2, (beginPoint.Z + endPoint.Z) / 2);
        }
    }
}
