using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HandRotationLeap
{
    public static class CubeUtility
    {
        public static Model3DGroup CreateCubeModel(ICollection<KeyValuePair<string, Visual>> squareInfoes)
        {
            return new Model3DGroup
            {
                Children = new Model3DCollection(squareInfoes.Select(CreateSquareModel)),
            };
        }

        public static GeometryModel3D CreateSquareModel(KeyValuePair<string, Visual> squareInfo)
        {
            var brush = new VisualBrush(squareInfo.Value);
            var material = new DiffuseMaterial(brush);

            return new GeometryModel3D
            {
                Geometry = CreateSquareGeometry(squareInfo.Key),
                Material = material,
                BackMaterial = material,
            };
        }

        public static Model3DGroup CreateCubeModel(ICollection<KeyValuePair<string, string>> squareInfoes)
        {
            return new Model3DGroup
            {
                Children = new Model3DCollection(squareInfoes.Select(CreateSquareModel)),
            };
        }

        public static GeometryModel3D CreateSquareModel(KeyValuePair<string, string> squareInfo)
        {
            var brush = new VisualBrush();
            BindingOperations.SetBinding(brush, VisualBrush.VisualProperty, new Binding { ElementName = squareInfo.Value });
            var material = new DiffuseMaterial(brush);

            return new GeometryModel3D
            {
                Geometry = CreateSquareGeometry(squareInfo.Key),
                Material = material,
                BackMaterial = material,
            };
        }

        public static MeshGeometry3D CreateSquareGeometry(string positions)
        {
            return new MeshGeometry3D
            {
                Positions = Point3DCollection.Parse(positions),
                TriangleIndices = Int32Collection.Parse("0,1,2 0,2,3"),
                TextureCoordinates = PointCollection.Parse("0,0 0,1 1,1 1,0"),
            };
        }
    }
}
