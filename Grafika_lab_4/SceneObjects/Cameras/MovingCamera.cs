using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class MovingCamera : Camera
    {

        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        Vector3 cameraPosition;
        public override Vector3 CameraPosition { get { return cameraPosition; } set { cameraPosition = value; } }
        public override Vector3 CameraTarget { get; set; }
        public override Vector3 CameraUp { get; set; }

        public MovingCamera(string name):base(name)
        {
            cameraPosition = Vector3.Zero;
            CameraTarget = Vector3.Zero;
            CameraUp = Vector3.UnitY;
        }

        public override void Update()
        {
            if (IsActive)
            {
                var keyboardState = OpenTK.Input.Keyboard.GetState();
                if (keyboardState.IsKeyDown(OpenTK.Input.Key.S))
                    cameraPosition.Z += MoveSpeed;
                if (keyboardState.IsKeyDown(OpenTK.Input.Key.W))
                    cameraPosition.Z -= MoveSpeed;
                if (keyboardState.IsKeyDown(OpenTK.Input.Key.A))
                    cameraPosition.X -= MoveSpeed;
                if (keyboardState.IsKeyDown(OpenTK.Input.Key.D))
                    cameraPosition.X += MoveSpeed;
                if (keyboardState.IsKeyDown(OpenTK.Input.Key.Q))
                    cameraPosition.Y += MoveSpeed;
                if (keyboardState.IsKeyDown(OpenTK.Input.Key.E))
                    cameraPosition.Y -= MoveSpeed;
            }

            Vector3 lookat = new Vector3();
            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            CameraTarget = CameraPosition + lookat;
        }
    }
}
