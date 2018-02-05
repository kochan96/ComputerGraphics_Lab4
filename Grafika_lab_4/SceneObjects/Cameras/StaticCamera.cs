using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class StaticCamera : Camera
    {

        public override Vector3 CameraPosition { get; set; }
        public override Vector3 CameraTarget { get; set; }
        public override Vector3 CameraUp { get; set; }
        public RenderSceneObject Follow { get; set; }
        public StaticCamera(string name) : base(name)
        {
            CameraUp = Vector3.UnitY;
            CameraTarget = Vector3.Zero;
            CameraPosition = Vector3.Zero;
        }
        public StaticCamera(string name, RenderSceneObject follow) : base(name)
        {
            CameraUp = Vector3.UnitY;
            CameraPosition = Vector3.Zero;
            this.Follow = follow;
            CameraTarget = follow.Position;
        }

        public override void Update()
        {
            if (Follow != null)
            {
                CameraTarget = Follow.Position;
            }
        }
    }
}
