using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects;
using Grafika_lab_4.SceneObjects.Base;
using Grafika_lab_4.SceneObjects.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Grafika_lab_4
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region Fields
        DateTime lastMeasuredFPSTime;
        DateTime lastMeasuredTime;
        int frames;
        Matrix4 ProjectionMatrix;
        List<Camera> cameras = new List<Camera>();
        int activeCamera = 0;
        List<Light> lights = new List<Light>();
        List<RenderSceneObject> renderObjects = new List<RenderSceneObject>();
        readonly string TerrainTexture = "Resources/Terrain/TerrainTexture2.png";
        readonly string TerrainHeightMap = "Resources/Terrain/HeightMap.png";
        #endregion;

        #region InitProgram
        private void MainWindow_Load(object sender, EventArgs e)
        {
            InitProgram();
        }

        private void InitProgram()
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl.Width / (float)glControl.Height, 1f, 1000f);
            glControl.MakeCurrent();
            SetGLParameter();
            CreateObjects();
            SetTimer();
        }
        private void SetGLParameter()
        {
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Viewport(glControl.Size);
            GL.Enable(EnableCap.DepthTest);
        }

        private void CreateObjects()
        {
            Terrain terrain = new Terrain(TerrainHeightMap)
            {
                Texture = new Textures.Texture(TerrainTexture)
            };
            terrain.Scale(new Vector3(800f, 800f, 40f));
            terrain.RotateByX(-MathHelper.PiOver2);
            Aircraft aircraft = new Aircraft();
            aircraft.Scale(new Vector3(10f, 10f, 10f));

            renderObjects.Add(terrain);
           // renderObjects.Add(aircraft);
            

            MovingCamera moveCamera = new MovingCamera
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, 168.39f),
                MoveSpeed = 1.5f
            };
            Camera staticCamera = new StaticCamera
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, 150.39f)
            };
            cameras.Add(moveCamera);
            cameras.Add(staticCamera);

            PointLight light = new PointLight
            {
                Position = 200000f * Vector3.UnitY
            };
            lights.Add(light);
        }

        private void SetTimer()
        {
            Timer timer = new Timer
            {
                Interval = 10
            };
            timer.Tick += Timer_Tick;
            lastMeasuredTime = DateTime.Now;
            lastMeasuredFPSTime = DateTime.Now;
            timer.Start();
        }

        #endregion

        #region Render
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(lastMeasuredFPSTime) >= TimeSpan.FromSeconds(1))
            {
                Text = $"FPS: {this.frames}";
                this.frames = 0;
                lastMeasuredFPSTime = DateTime.Now;
            }


            float deltaTime = (float)DateTime.Now.Subtract(lastMeasuredTime).TotalSeconds;
            lastMeasuredTime = DateTime.Now;

            foreach (var cam in cameras)
                cam.Update();

            foreach (var obj in renderObjects)
                obj.Update(deltaTime);

            lights[0].Position = new Vector3(Matrix4.CreateRotationX(-0.2f * deltaTime) * new Vector4(lights[0].Position, 1.0f));

            glControl.Invalidate();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var obj in renderObjects)
            {
                obj.Renderer.Use();
                obj.Bind();
                obj.Renderer.SetModelMatrix(obj.ModelMatrix);
                obj.Renderer.SetViewMatrix(cameras[activeCamera].GetViewMatrix());
                obj.Renderer.SetProjectionMatrix(ProjectionMatrix);
                if (obj.Texture != null)
                    obj.Renderer.SetTexture(obj.Texture);
                obj.Renderer.SetLightPosition(lights[0].Position);
                obj.Renderer.SetLightColor(lights[0].LightColor);
                obj.Render();
                obj.UnBind();
            }

            glControl.SwapBuffers();
            this.frames++;
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(glControl.Size);
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl.Width / (float)glControl.Height, 1f, 1000f);
        }
        #endregion

        #region Menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
                activeCamera = (activeCamera + 1) % cameras.Count;
        }
    }
}
