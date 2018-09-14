using OpenTK;
using System;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class MovingCamera : Camera
    {
        private  Vector3 _orientation = new Vector3((float)Math.PI, 0f, 0f);

        public float MoveSpeed = 0.2f;

        public float MouseSensitivity = 0.005f;

        private void HandleKeyboard()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.S))
            {
                Move(0f, -0.1f, 0.0f);
            }

            if (keyboardState.IsKeyDown(OpenTK.Input.Key.W))
            {
                Move(0f, 0.1f, 0.0f);
            }

            if (keyboardState.IsKeyDown(OpenTK.Input.Key.A))
            {
                Move(-0.1f, 0f, 0.0f);
            }

            if (keyboardState.IsKeyDown(OpenTK.Input.Key.D))
            {
                Move(0.1f, 0f, 0.0f);
            }

            if (keyboardState.IsKeyDown(OpenTK.Input.Key.E))
            {
                Move(0f, 0.0f, -0.1f);
            }

            if (keyboardState.IsKeyDown(OpenTK.Input.Key.Q))
            {
                Move(0f, 0.0f, 0.1f);
            }
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin(_orientation.X), 0, (float)Math.Cos(_orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            CameraPosition += offset;
        }

        private void Rotate(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            _orientation.X = (_orientation.X + x) % ((float)Math.PI * 2.0f);
            _orientation.Y = Math.Max(Math.Min(_orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }

        Vector2 lasPos = Vector2.Zero;
        private void HandleMouse()
        {
            var mouse = OpenTK.Input.Mouse.GetState();
            if (mouse.IsButtonDown(OpenTK.Input.MouseButton.Left))
            {
                Vector2 delta = lasPos - new Vector2(mouse.X, mouse.Y);
                Rotate(delta.X, delta.Y);
            }
            lasPos = new Vector2(mouse.X, mouse.Y);

        }
        public override void Update()
        {
            if (IsActive)
            {
                HandleKeyboard();
                HandleMouse();
            }

            Vector3 lookat = new Vector3();
            lookat.X = (float)(Math.Sin(_orientation.X) * Math.Cos(_orientation.Y));
            lookat.Y = (float)Math.Sin(_orientation.Y);
            lookat.Z = (float)(Math.Cos(_orientation.X) * Math.Cos(_orientation.Y));
            CameraTarget = CameraPosition + lookat;
        }
    }
}
