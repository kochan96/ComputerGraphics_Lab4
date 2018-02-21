using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.SceneObjects
{
    public class ControlableAircraft : Aircraft
    {
        public ControlableAircraft(string name) : base(name)
        {

        }

        float accelerate = 0.1f;
        float rolled = 0.0f;
        float up = 0.0f;
        float rotationSpeed=1.0f;
        float rotateLeftRight = 0.02f;
        public override void Update(float deltatime)
        {

            float angle =deltatime* rotationSpeed;

            Vector3 oldPosition = Position;
            Translate(-oldPosition);
            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard.IsKeyDown(OpenTK.Input.Key.W))
                Speed += accelerate;
            if (keyboard.IsKeyDown(OpenTK.Input.Key.S))
            {
                Speed -= accelerate;
                Speed = Speed < 0.0f ? 0.0f : Speed;
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.A))
            {
                if (rolled >= -MathHelper.PiOver4)
                {
                    RotateAndChange(-angle, Forward);
                    rolled -= angle;
                }
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.D))
            {
                if (rolled <= MathHelper.PiOver4)
                {
                    RotateAndChange(angle, Forward);
                    rolled += angle;
                }
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.E))
            {
                if (up <= MathHelper.PiOver4)
                {
                    RotateAndChange(angle, Right);
                    up += angle;
                }
            }
            if (keyboard.IsKeyDown(OpenTK.Input.Key.Q))
            {
                if(up >=-MathHelper.PiOver4)
                {
                    RotateAndChange(-angle, Right);
                    up -= angle;
                }
            }

            float rotateY = Helper.MapValue(rolled, -MathHelper.PiOver4, MathHelper.PiOver4, -rotateLeftRight, rotateLeftRight);
            RotateAndChange(-rotateY, Vector3.UnitY);
            Vector3 newPosition = oldPosition + Forward * Speed * deltatime;
            Translate(newPosition);

            HandleLights(newPosition);
        }

        private Quaternion CreateQuaterion(float angle,Vector3 axis)
        {
            float sin = (float)Math.Sin(angle / 2);

            Quaternion q = new Quaternion();
            q.X = axis.X * sin;
            q.Y = axis.Y * sin;
            q.Z = axis.Z * sin;
            q.W = (float)Math.Cos(angle / 2);

            return q;
        }
    }
}
