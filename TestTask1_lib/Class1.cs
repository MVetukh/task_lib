using GeometryLibrary;
using VisualStudio.TestTools.UnitTesting;

namespace GeometryLibrary
{
    public abstract class Shape
    {
        public abstract double Area { get; }

        public static bool IsRightAngledTriangle(double sideA, double sideB, double sideC)
        {
            double hypotenuse = Math.Max(sideA, Math.Max(sideB, sideC));
            double leg1 = hypotenuse == sideA ? sideB : sideA;
            double leg2 = hypotenuse == sideC ? sideB : sideC;
            return Math.Abs(hypotenuse * hypotenuse - (leg1 * leg1 + leg2 * leg2)) < 0.0001;
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; private set; }

        public Circle(double radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius must be greater than zero.");
            Radius = radius;
        }

        public override double Area => Math.PI * Radius * Radius;
    }

    public class Triangle : Shape
    {
        public double SideA { get; private set; }
        public double SideB { get; private set; }
        public double SideC { get; private set; }

        public Triangle(double sideA, double sideB, double sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
                throw new ArgumentException("Sides must be greater than zero.");
            if (sideA + sideB <= sideC || sideA + sideC <= sideB || sideB + sideC <= sideA)
                throw new ArgumentException("Triangle inequality theorem violated.");

            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }

        public override double Area
        {
            get
            {
                double s = (SideA + SideB + SideC) / 2;
                return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
            }
        }

        public bool IsRightAngled => IsRightAngledTriangle(SideA, SideB, SideC);
    }

    public static class ShapeFactory
    {
        public static Shape CreateShape(params double[] sides)
        {
            switch (sides.Length)
            {
                case 1:
                    return new Circle(sides[0]);
                case 3:
                    return new Triangle(sides[0], sides[1], sides[2]);
                default:
                    throw new ArgumentException("Unsupported shape.");
            }
        }
    }
}

// Пример юнит-теста

[TestClass]
public class ShapeTests
{
    [TestMethod]
    public void Circle_Area_Test()
    {
        // Arrange
        var circle = new Circle(2);

        // Act
        var area = circle.Area;

        // Assert
        Assert.AreEqual(Math.PI * 4, area, 0.001);
    }

    [TestMethod]
    public void Triangle_Area_Test()
    {
        // Arrange
        var triangle = new Triangle(3, 4, 5);

        // Act
        var area = triangle.Area;

        // Assert
        Assert.AreEqual(6, area, 0.001);
    }

    [TestMethod]
    public void IsRightAngledTriangle_Test()
    {
        // Assert
        Assert.IsTrue(Triangle.IsRightAngledTriangle(3, 4, 5));
    }
}