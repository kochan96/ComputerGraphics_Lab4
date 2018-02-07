using System;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class BehindCamera : Camera
    {
        public override Vector3 CameraPosition { get; set; }
        public override Vector3 CameraTarget { get; set; }
        public override Vector3 CameraUp { get; set; }
        RenderSceneObject follow;

        public float Dist{get; set;}
        public float Angle { get; set; }

        public BehindCamera(string name, RenderSceneObject follow) : base(name)
        {
            this.follow = follow;
            CameraTarget = follow.Position;
            CameraUp = follow.Up;
        }
        public override void Update()
        {
            CameraTarget = follow.Position;
            CameraPosition = CameraTarget - follow.Forward * Dist*(float)Math.Cos(Angle);
            CameraPosition = CameraPosition + follow.Up * Dist * (float)Math.Sin(Angle);
            CameraUp = follow.Up;
        }
    }
}
