using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DiceCodeWpf
{
    public static class CubeUtility
    {
        public static Model3DGroup CreateCubeModel()
        {
            var model = new Model3DGroup();

            var square0 = CreateSquareModel();
            var square1 = CreateSquareModel();
            var square2 = CreateSquareModel();
            var square3 = CreateSquareModel();
            var square4 = CreateSquareModel();
            var square5 = CreateSquareModel();

            ((Transform3DGroup)square0.Transform).Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 1)));
            ((Transform3DGroup)square1.Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90)));
            ((Transform3DGroup)square2.Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), -90)));
            ((Transform3DGroup)square3.Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90), new Point3D(1, 0, 0)));
            ((Transform3DGroup)square4.Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), -90), new Point3D(0, 1, 0)));
            ((Transform3DGroup)square5.Transform).Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 180), new Point3D(0, 0.5, 0)));

            model.Children.Add(square0);
            model.Children.Add(square1);
            model.Children.Add(square2);
            model.Children.Add(square3);
            model.Children.Add(square4);
            model.Children.Add(square5);

            return model;
        }

        public static void SetMaterialToCube(Model3DGroup cubeModel, int index, Visual visual)
        {
            var brush = new VisualBrush(visual);
            var material = new DiffuseMaterial(brush);

            var square = (GeometryModel3D)cubeModel.Children[index];
            square.Material = material;
            square.BackMaterial = material;
        }

        public static GeometryModel3D CreateSquareModel()
        {
            return new GeometryModel3D
            {
                Geometry = CreateSquareGeometry(),
                Transform = new Transform3DGroup(),
            };
        }

        public static MeshGeometry3D CreateSquareGeometry()
        {
            // <MeshGeometry3D Positions="0,1,0 0,0,0 1,0,0 1,1,0" TriangleIndices="0 1 2 0 2 3" TextureCoordinates="0,0 0,1 1,1 1,0"/>
            var geometry = new MeshGeometry3D();

            geometry.Positions.Add(new Point3D(0, 1, 0));
            geometry.Positions.Add(new Point3D(0, 0, 0));
            geometry.Positions.Add(new Point3D(1, 0, 0));
            geometry.Positions.Add(new Point3D(1, 1, 0));

            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(1);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(0);
            geometry.TriangleIndices.Add(2);
            geometry.TriangleIndices.Add(3);

            geometry.TextureCoordinates.Add(new Point(0, 0));
            geometry.TextureCoordinates.Add(new Point(0, 1));
            geometry.TextureCoordinates.Add(new Point(1, 1));
            geometry.TextureCoordinates.Add(new Point(1, 0));

            return geometry;
        }

        public static RotateTransform3D CreateAngleRotateTransformX(double angle)
        {
            return new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), angle));
        }

        public static void Rotate(this MatrixTransform3D transform, Vector3D axis, double angle)
        {
            var matrix = transform.Matrix;
            matrix.Rotate(new Quaternion(axis, angle));
            transform.Matrix = matrix;
        }

        public static void RotateAt(this MatrixTransform3D transform, Vector3D axis, double angle, Point3D center)
        {
            var matrix = transform.Matrix;
            matrix.RotateAt(new Quaternion(axis, angle), center);
            transform.Matrix = matrix;
        }
    }
}
