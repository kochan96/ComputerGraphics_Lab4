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
        public Vector3 CameraPosition=-Vector3.UnitZ;
        public Vector3 CameraTarget=Vector3.Zero;
        public Vector3 CameraUp=Vector3.UnitY;
        public abstract Matrix4 GetViewMatrix();

        public abstract void Update();
    }
}
