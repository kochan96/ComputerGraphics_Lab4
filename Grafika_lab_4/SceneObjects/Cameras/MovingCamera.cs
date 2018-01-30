using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class MovingCamera:Camera
    {
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.01f;

        public override Matrix4 GetViewMatrix()
        {
            Vector3 lookat = new Vector3();

            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            return Matrix4.LookAt(CameraPosition, CameraPosition + lookat, Vector3.UnitY);
        }

        public override void Update()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.S))
                CameraPosition.Z += MoveSpeed;
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.W))
                CameraPosition.Z -= MoveSpeed;
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.A))
                CameraPosition.X -= MoveSpeed;
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.D))
                CameraPosition.X += MoveSpeed;
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.Q))
                CameraPosition.Y += MoveSpeed;
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.E))
                CameraPosition.Y -= MoveSpeed;

        }
    }
}
