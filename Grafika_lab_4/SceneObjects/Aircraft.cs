using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Loader;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Grafika_lab_4.SceneObjects
{
    public class Aircraft : RenderSceneObject
    {

        #region Fields
        EntityRenderer renderer = EntityRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }

        public RawObjModel RawModel;
        /*
        protected override Vector3 DefaultForward { get { return Vector3.UnitX; } }
        protected override Vector3 DefaultUp { get { return Vector3.UnitZ; } }
        protected override Vector3 DefaultRight { get { return -Vector3.UnitY; } }*/

        public float Speed { get; set; }
        public float HumanControlSpeed { get; set; }

        bool humanControl;
        public bool HumanControl
        {
            get
            {
                return humanControl;
            }
            set
            {
                humanControl = value;
                if (humanControl == false)
                    Reset();
                else
                    EllipseY = Position.Y;

            }
        }

        protected Light[] light=new Light[2];
        public Light[] Light { get { return light; } }

        #endregion

        public Aircraft(string name, Vector3 position) : base(name, position)
        {
            string modelFile = Resources.AircraftModel;
            if (File.Exists(modelFile))
            {
                CreateAircraft(modelFile);
            }
            else
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.FileMissingError) + modelFile);

            light[0] = new Light(Name + " rightLight")
            {
                AmbientIntensity = 0.0f,
                DiffuseIntensity = 0.5f,
                SpecularIntensity = 0.3f,
                Color = Vector3.One,
                Direction = Forward,
                Position = Position,
                ConeAngle = 15f,
                Attenuation = Vector3.UnitX,
                LightType = LightTypes.SpotLight
            };

            light[1] = new Light(Name + " leftLight")
            {
                AmbientIntensity = 0.0f,
                DiffuseIntensity = 0.5f,
                SpecularIntensity = 0.3f,
                Color = Vector3.One,
                Direction = Forward,
                Position = Position,
                ConeAngle = 15f,
                Attenuation = Vector3.UnitX,
                LightType = LightTypes.SpotLight
            };

        }
        public Aircraft(string name) : this(name, Vector3.Zero) { }


        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out normalsBuffer);
        }


        private void CreateAircraft(string filePath)
        {
            RawObjModel model = RawObjLoader.LoadRawObj(filePath);
            if (model == null)
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadModelError) + Resources.AircraftModel);
                return;
            }
            else if (model.Meshes == null || model.Meshes.Count == 0)
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadedEmptyModelError) + Resources.AircraftModel);
                return;
            }

            Bind();
            List<int> indices = new List<int>();
            foreach (Mesh mesh in model.Meshes)
            {
                indices.AddRange(mesh.Indices);
            }
            RawModel = model;
            SetVerticesBuffer(model.Vertices.ToArray());
            SetIndicesBuffer(indices.ToArray());
            SetNormalsBuffer(model.Normals.ToArray());

            UnBind();
        }

        public override void Render()
        {
            if (RawModel != null)
            {
                int offset = 0;
                Renderer.EnableVertexAttribArrays();
                renderer.SetHasTexture(Texture != null);
                foreach (Mesh mesh in RawModel.Meshes)
                {
                    renderer.SetAmbientColor(mesh.MeshMaterial.Ka);
                    renderer.SetDiffuseColor(mesh.MeshMaterial.Kd);
                    renderer.SetSpecularColor(mesh.MeshMaterial.Ks);
                    renderer.SetSpecularExponenet(mesh.MeshMaterial.Ns);
                    GL.DrawElements(BeginMode.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt, offset * sizeof(uint));
                    offset += mesh.Indices.Count;
                }
                Renderer.DisableVertexAttribArrays();
            }
        }
        /// <summary>
        /// X-Axis elipsse
        /// </summary>
        public float Semiminor { get; set; }
        /// <summary>
        /// Z-axis of ellipse
        /// </summary>
        public float Semimajor { get; set; }

        

        public override void Update(float deltatime)
        {
           
            Vector3 position = HumanControl ? UpdateHuman(deltatime) : Ellipse(deltatime);
            HandleLights(position);
        }

        private void Reset()
        {
            ResetRotation();
            alpha = 0;
            rolled = 0.0f;
            up = 0.0f;

        }

        float EllipseY;
        public Vector2 CenterOfElipse = Vector2.Zero;
        float alpha = 0;
        private Vector3 Ellipse(float deltatime)
        {
            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard.IsKeyDown(OpenTK.Input.Key.W))
                Speed += accelerate;
            if (keyboard.IsKeyDown(OpenTK.Input.Key.S))
            {
                Speed -= accelerate;
                Speed = Speed < 0.0f ? 0.0f : Speed;
            }

            float factor = Speed * deltatime;
            alpha += factor;
            float X = CenterOfElipse.X - (Semiminor * (float)Math.Cos(alpha));
            float Y = CenterOfElipse.Y - (Semimajor * (float)Math.Sin(alpha));
            Vector3 old = Position;
            Vector3 position = new Vector3(X, EllipseY, Y);

            Translate(-old);
            Yaw(-Speed * deltatime);
            Translate(position);
            return position;
        }

        float offset;
        float offsetChange = 0.01f;
        float maxOffset = 0.35f;
        bool lightsOn = true;
        protected void HandleLights(Vector3 position)
        {
            var state = OpenTK.Input.Keyboard.GetState();
            if (offset<= maxOffset && state.IsKeyDown(OpenTK.Input.Key.R))
            {
                offset += offsetChange;
            }
            if (offset >= -maxOffset && state.IsKeyDown(OpenTK.Input.Key.T))
            {
                offset -= offsetChange;
            }
            if (state.IsKeyDown(OpenTK.Input.Key.F))
            {
                if (lightsOn)
                {
                    light[0].Color = Vector3.Zero;
                    light[1].Color = Vector3.Zero;
                }
                else
                {
                    light[0].Color = Vector3.One;
                    light[1].Color = Vector3.One;
                }
                lightsOn = !lightsOn;
            }

            light[0].Position = position+ Right * 2f;
            light[0].Direction = (Forward + offset * Right).Normalized();

            light[1].Position = position- Right * 2f;
            light[1].Direction = (Forward + offset * Right).Normalized();
        }

        float accelerateHuman = 0.1f;
        float accelerate = 0.006f;
        float rolled = 0.0f;
        float up = 0.0f;
        float rotationSpeed = 1.0f;
        float rotateLeftRight = 0.02f;
        private Vector3 UpdateHuman(float deltatime)
        {
            float angle = deltatime * rotationSpeed;

            Vector3 oldPosition = Position;
            Translate(-oldPosition);
            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard.IsKeyDown(OpenTK.Input.Key.W))
                HumanControlSpeed += accelerateHuman;
            if (keyboard.IsKeyDown(OpenTK.Input.Key.S))
            {
                HumanControlSpeed -= accelerateHuman;
                HumanControlSpeed = HumanControlSpeed < 0.0f ? 0.0f : HumanControlSpeed;
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
                if (up >= -MathHelper.PiOver4)
                {
                    RotateAndChange(-angle, Right);
                    up -= angle;
                }
            }

            float rotateY = Helper.MapValue(rolled, -MathHelper.PiOver4, MathHelper.PiOver4, -rotateLeftRight, rotateLeftRight);
            RotateAndChange(-rotateY, Vector3.UnitY);
            Vector3 newPosition = oldPosition + Forward * HumanControlSpeed * deltatime;
            Translate(newPosition);
            return newPosition;
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(elementsBuffer);
            GL.DeleteBuffer(normalsBuffer);
        }
    }
}
