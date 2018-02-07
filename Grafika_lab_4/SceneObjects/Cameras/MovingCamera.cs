using OpenTK;
using System;

namespace Grafika_lab_4.SceneObjects.Cameras
{
    public class MovingCamera : Camera
    {

        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.005f;
        Vector3 cameraPosition;
        public override Vector3 CameraPosition { get { return cameraPosition; } set { cameraPosition = value; } }
        public override Vector3 CameraTarget { get; set; }
        public override Vector3 CameraUp { get; set; }

        public MovingCamera(string name) : base(name)
        {
            cameraPosition = Vector3.Zero;
            CameraTarget = Vector3.Zero;
            CameraUp = Vector3.UnitY;
        }

        private void HandleKeyboard()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.S))
                Move(0f, -0.1f, 0.0f);
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.W))
                Move(0f, 0.1f, 0.0f);
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.A))
                Move(-0.1f, 0f, 0.0f);
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.D))
                Move(0.1f, 0f, 0.0f);
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.E))
                Move(0f, 0.0f, -0.1f);
            if (keyboardState.IsKeyDown(OpenTK.Input.Key.Q))
                Move(0f, 0.0f, 0.1f);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
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

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
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
            lookat.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookat.Y = (float)Math.Sin((float)Orientation.Y);
            lookat.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            CameraTarget = CameraPosition + lookat;
        }
    }
}
