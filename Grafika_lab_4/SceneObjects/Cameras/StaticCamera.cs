using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class StaticCamera : Camera
    {
        public override Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(CameraPosition, CameraTarget, CameraUp);
        }

        public override void Update()
        {
            
        }
    }
}
