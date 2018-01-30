using Grafika_lab_4.Lights;
using Grafika_lab_4.SceneObjects;
using Grafika_lab_4.SceneObjects.Cameras;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
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
        int frames;
        DateTime lastMeasuredTime;
        Terrain terrain;
        Matrix4 ProjectionMatrix;
        List<Camera> cameras=new List<Camera>();
        int activeCamera=0;
        List<Light> lights = new List<Light>();
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
            ProjectionMatrix =Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl.Width / (float)glControl.Height, 1f, 1000f);
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
            terrain = new Terrain(TerrainHeightMap);
            terrain.Texture = new Textures.Texture(TerrainTexture);
            terrain.ModelMatrix = Matrix4.CreateScale(new Vector3(800f, 800,40f)) * Matrix4.CreateRotationX(-MathHelper.PiOver2);
            MovingCamera moveCamera = new MovingCamera();
            moveCamera.CameraPosition =new  Vector3(-2.2f, 45.8f, 168.39f);
            moveCamera.MoveSpeed = 1.5f;
            Camera staticCamera = new StaticCamera();
            staticCamera.CameraPosition = new Vector3(-2.2f, 45.8f, 150.39f);
            cameras.Add(moveCamera);
            cameras.Add(staticCamera);

            PointLight light = new PointLight();
            light.Position = Vector3.UnitY;
            lights.Add(light);
        }

        private void SetTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 15;
            timer.Tick += Timer_Tick;
            lastMeasuredTime = DateTime.Now;
            timer.Start();
        }

        #endregion

        #region Render
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Subtract(lastMeasuredTime) >= TimeSpan.FromSeconds(1))
            {
                Text = $"FPS: {this.frames}";
                this.frames = 0;
                lastMeasuredTime = DateTime.Now;
            }

            foreach (var cam in cameras)
                cam.Update();

            glControl.Invalidate();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            terrain.Renderer.Use();
            terrain.Renderer.SetModelMatrix(terrain.ModelMatrix);
            terrain.Renderer.SetViewMatrix(cameras[activeCamera].GetViewMatrix());
            terrain.Renderer.SetProjectionMatrix(ProjectionMatrix);
            terrain.Renderer.SetTexture(terrain.Texture);
            terrain.Renderer.SetLightPosition(lights[0].Position);
            terrain.Renderer.SetLightColor(lights[0].LightColor);
            terrain.Render();

            glControl.SwapBuffers();
            this.frames++;
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(glControl.Size);
            ProjectionMatrix=Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl.Width / (float)glControl.Height, 1f, 1000f);
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
