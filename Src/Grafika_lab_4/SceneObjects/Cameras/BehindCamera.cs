using System;
using Grafika_lab_4.SceneObjects.Base;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class BehindCamera : Camera
    {
        private readonly RenderSceneObject _follow;

        public float Dist { get; set; }

        public float Angle { get; set; }

        public BehindCamera(RenderSceneObject follow)
        {
            _follow = follow;
            CameraTarget = _follow.Position;
            CameraUp = _follow.Up;
        }
        public override void Update()
        {
            CameraTarget = _follow.Position;
            CameraPosition = CameraTarget - _follow.Forward * Dist * (float)Math.Cos(Angle);
            CameraPosition = CameraPosition + CameraUp * Dist * (float)Math.Sin(Angle);
        }
    }
}
