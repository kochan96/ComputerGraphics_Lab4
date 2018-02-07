using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Loader;
using Grafika_lab_4.SceneObjects;
using Grafika_lab_4.SceneObjects.Base;
using Grafika_lab_4.SceneObjects.Cameras;
using OpenTK;
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
        DateTime lastMeasuredFPSTime;
        DateTime lastMeasuredTime;
        int frames;
        Matrix4 ProjectionMatrix;
        List<Camera> cameras = new List<Camera>();
        int activeCameraIndex = 0;
        List<Light> lights = new List<Light>();
        List<RenderSceneObject> renderObjects = new List<RenderSceneObject>();
        Vector3 SkyColor = new Vector3(0.5f, 0.5f, 0.5f);
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
            GL.ClearColor(SkyColor.X, SkyColor.Y, SkyColor.Z, 1.0f);
            GL.Viewport(glControl.Size);
            GL.Enable(EnableCap.DepthTest);
        }

        private void CreateObjects()
        {
            Texture texture1 = TextureLoader.LoadTexture2D(Resources.GreenTexture);
            Texture texture2 = TextureLoader.LoadTexture2D(Resources.RockTexture);
            Texture texture3 = TextureLoader.LoadTexture2D(Resources.TestTexture);
            Texture skyBoxTexture1 = TextureLoader.LoadCubeMap(Resources.SkyCubeMapTextures);
            Texture sphereTexture1 = TextureLoader.LoadTexture2D(Resources.SphereTexture);
            ///Terain
            Terrain terrain = new Terrain("Terrain", Resources.RiverMountainHeightMap)
            {
                Texture = texture2,
            };
            float TerrainWidth = 100f;
            float TerrainLength = 100f;
            terrain.ScaleObject(new Vector3(TerrainWidth, TerrainLength, 5f));
            terrain.Pitch(-MathHelper.PiOver2);

            ///FirstAircraft
            Aircraft aircraft = new Aircraft("Aircraft1");
            aircraft.Speed = 1f;
            aircraft.ScaleObject(50f);
            aircraft.RotateByY(MathHelper.PiOver2);
            aircraft.RotateByZ(MathHelper.PiOver2);
            aircraft.Translate(Vector3.UnitX);
            aircraft.Semimajor = 20f;
            aircraft.Semiminor = 20f;

            ///SecondAircraft
            Aircraft aircraft2 = new Aircraft("Aircraft2");
            aircraft2.Speed = 1f;
            aircraft2.ScaleObject(50f);
            aircraft2.RotateByY(MathHelper.PiOver2);
            aircraft2.RotateByZ(MathHelper.PiOver2);
            aircraft2.Semimajor = 30f;
            aircraft2.Semiminor = 20f;

            Sphere sphere = new Sphere("spher", 100);
            sphere.ScaleObject(5);

            Sphere sphere2 = new Sphere("sphere2", 100);
            sphere2.ScaleObject(5);
            sphere2.Texture = sphereTexture1;
            sphere2.Translate(sphere2.Up * 15f);

            SkyBox skybox = new SkyBox("SkyBox");
            skybox.Texture = skyBoxTexture1;
            skybox.ScaleObject(new Vector3(TerrainWidth, TerrainLength, TerrainLength));



            ///RenderObjects
            renderObjects.Add(terrain);
            renderObjects.Add(aircraft);
            renderObjects.Add(aircraft2);
            renderObjects.Add(sphere);
            renderObjects.Add(sphere2);

            ///render last
            renderObjects.Add(skybox);

            MovingCamera moveCamera = new MovingCamera("MovingCamera")
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, 168.39f),
                MoveSpeed = 1.5f
            };
            Camera staticCamera = new StaticCamera("StaticCamera")
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, 150.39f)
            };
            Camera staticFollowCamera = new StaticCamera("StaticFollowCamera", aircraft2)
            {
                CameraPosition = new Vector3(0f, 10f, 10f)
            };
            Camera staticFollowCamera2 = new StaticCamera("StaticFollowCamera", sphere)
            {
                CameraPosition = new Vector3(0f, 10f, 10f)
            };
            BehindCamera behindCamera = new BehindCamera("behindCamera", aircraft)
            {
                Dist = 13f,
                Angle = MathHelper.PiOver6
            };

            //Cameras
            cameras.Add(moveCamera);
            cameras.Add(staticCamera);
            cameras.Add(staticFollowCamera);
            cameras.Add(staticFollowCamera2);
            cameras.Add(behindCamera);

            cameras[activeCameraIndex].IsActive = true;

            //PointLights
            Light light = new Light("MainLight")
            {
                Position = 1000f * Vector3.UnitY
            };

            //Lights
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
            foreach (var obj in renderObjects)
                obj.Update(deltaTime);

            foreach (var cam in cameras)
                cam.Update();

            foreach (var light in lights)
            {
                Matrix4 moveMatrix=Matrix4.Identity;
                if (lightRotationY >= MathHelper.TwoPi)
                {
                    lightRotationY = 0;
                    moveMatrix = Matrix4.CreateRotationZ(MathHelper.PiOver2 * deltaTime);
                }
                float angle = MathHelper.PiOver2 * deltaTime;
                moveMatrix *=Matrix4.CreateRotationY(angle);
                lightRotationY += angle;
                moveMatrix.Transpose();
                light.Position = new Vector3(moveMatrix * new Vector4(lights[0].Position, 1.0f));
            }
            glControl.Invalidate();
        }
        float lightRotationY;

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var obj in renderObjects)
            {
                obj.Renderer.Use();
                obj.Bind();
                obj.Renderer.SetModelMatrix(obj.ModelMatrix, false);
                obj.Renderer.SetViewMatrix(cameras[activeCameraIndex].GetViewMatrix(), false);
                obj.Renderer.SetProjectionMatrix(ProjectionMatrix, false);
                obj.Renderer.SetTexture(obj.Texture);
                obj.Renderer.SetLights(lights);
                obj.Render();
                obj.UnBind();
            }

            glControl.SwapBuffers();
            this.frames++;
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                cameras[activeCameraIndex].IsActive = false;
                activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
                cameras[activeCameraIndex].IsActive = true;
            }
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
            CleanUp();
            Application.Exit();
        }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanUp();
        }

        private void CleanUp()
        {
            foreach (var obj in renderObjects)
            {
                obj.Renderer.Delete();
                obj.Dispose();
            }
        }





        #endregion
    }
}
