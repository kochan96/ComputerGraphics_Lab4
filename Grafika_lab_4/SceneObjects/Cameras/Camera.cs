using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public abstract class Camera
    {
        public Camera(string name)
        {
            Name = name;
        }

        public bool IsActive { get; set;}
        public string Name { get; set;}
        public abstract Vector3 CameraPosition { get; set;}
        public abstract Vector3 CameraTarget { get; set; }
        public abstract Vector3 CameraUp { get; set; }
        public  Matrix4 GetViewMatrix()
        {
            Vector3 zaxis = (CameraPosition - CameraTarget).Normalized();    // The "forward" vector.
            Vector3 xaxis = Vector3.Cross(CameraUp, zaxis).Normalized();// The "right" vector.
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis);     // The "up" vector.

            // Create a 4x4 view matrix from the right, up, forward and eye position vectors
            Matrix4 viewMatrix = new Matrix4(
                xaxis.X, yaxis.X, zaxis.X, 0,
                xaxis.Y, yaxis.Y, zaxis.Z, 0,
                xaxis.Z, yaxis.Z, zaxis.Z, 0,
                -Vector3.Dot(xaxis, CameraPosition), -Vector3.Dot(yaxis, CameraPosition), -Vector3.Dot(zaxis, CameraPosition), 1);

            return viewMatrix;
        }

        public abstract void Update();
    }
}
