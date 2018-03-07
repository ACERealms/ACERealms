using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Collision
{
    public class BBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public void AdjustBBox(Vector3 v)
        {
            if (v.X < Min.X) Min.X = v.X;
            if (v.Y < Min.Y) Min.Y = v.Y;
            if (v.Z < Min.Z) Min.Z = v.Z;

            if (v.X > Max.X) Max.X = v.X;
            if (v.Y > Max.Y) Max.Y = v.Y;
            if (v.Z > Max.Z) Max.Z = v.Z;
        }

        public void BuildBoundingBox(BBox bbox)
        {
            if (Min.X < bbox.Min.X) bbox.Min.X = Min.X;
            if (Min.Y < bbox.Min.Y) bbox.Min.Y = Min.Y;
            if (Min.Z < bbox.Min.Z) bbox.Min.Z = Min.Z;

            if (Max.X > bbox.Max.X) bbox.Max.X = Max.X;
            if (Max.Y > bbox.Max.Y) bbox.Max.Y = Max.Y;
            if (Max.Z > bbox.Max.Z) bbox.Max.Z = Max.Z;
        }

        public void ConvertToGlobal(Position pos)
        {
            var transform = Matrix4x4.CreateFromQuaternion(pos.Frame.Orientation) * Matrix4x4.CreateTranslation(pos.Frame.Origin); ;
            Min = Vector3.Transform(Min, transform);
            Max = Vector3.Transform(Max, transform);
            // adjust?
        }

        public Vector3 GetCenter()
        {
            return new Vector3((Max.X - Min.X) * 0.5f, (Max.Y - Min.Y) * 0.5f, (Max.Z - Min.Z) * 0.5f);
        }

        public List<Vector3> GetCorners()
        {
            // cache?
            return new List<Vector3>()
            {
                new Vector3(Min.X, Min.Y, Min.Z),
                new Vector3(Min.X, Min.Y, Max.Z),
                new Vector3(Min.X, Max.Y, Min.Z),
                new Vector3(Min.X, Max.Y, Max.Z),
                new Vector3(Max.X, Min.Y, Min.Z),
                new Vector3(Max.X, Min.Y, Max.Z),
                new Vector3(Max.X, Max.Y, Min.Z),
                new Vector3(Max.X, Max.Y, Max.Z),
            };
        }

        public void InitForAdjustment()
        {
            Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public void LocalToGlobal(BBox fromBox, Position fromPos, Position toPos)
        {
            Min = toPos.LocalToGlobal(fromPos, fromBox.Min);
            Max = toPos.LocalToGlobal(fromPos, fromBox.Max);
        }

        public void LocalToLocal(BBox fromBox, Position fromPos, Position toPos)
        {
            Min = toPos.LocalToLocal(fromPos, fromBox.Min);
            Max = toPos.LocalToLocal(fromPos, fromBox.Max);
        }
    }
}
