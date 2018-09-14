using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Loader;
using Grafika_lab_4.Renderers.Structs;
using Grafika_lab_4.SceneObjects;
using Grafika_lab_4.SceneObjects.Base;
using Grafika_lab_4.SceneObjects.Cameras;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics;

namespace Grafika_lab_4
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private DateTime lastMeasuredFPSTime;
        private DateTime lastMeasuredTime;
        private int frames;
        private Matrix4 ProjectionMatrix;
        private List<Camera> cameras = new List<Camera>();
        private int activeCameraIndex = 0;
        private List<Light> lights = new List<Light>();
        private List<RenderSceneObject> renderObjects = new List<RenderSceneObject>();
        private Vector3 SkyColor = new Vector3(0.5f, 0.5f, 0.5f);
        private bool PhongLightinngModel = true;
        private bool PhongShading = true;
        private Aircraft aircraft;
        private Terrain terrain;
        private Texture texture1;
        private Texture texture2;

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
            texture1 = TextureLoader.LoadTexture2D(Resources.GreenTexture);
            texture2 = TextureLoader.LoadTexture2D(Resources.RockTexture);
            Texture skyBoxTexture1 = TextureLoader.LoadCubeMap(Resources.SkyCubeMapTextures);
            Texture TreeTexture = TextureLoader.LoadTexture2D(Resources.TreeTexture);
            ///Terain
            terrain = new Terrain(Resources.MountainsHeightMap)
            {
                Texture = texture1,
            };
            float TerrainLength = 300f;
            float TerrainHeight = 20f;

            terrain.ScaleObject(new Vector3(TerrainLength, TerrainLength, TerrainHeight));
            terrain.Pitch(-MathHelper.PiOver2);


            ///FirstAircraft
            aircraft = new Aircraft();
            aircraft.Speed = 0.6f;
            aircraft.HumanControlSpeed = 20f;
            aircraft.ScaleObject(100f);
            aircraft.RotateByY(MathHelper.PiOver2);
            aircraft.RotateByZ(MathHelper.PiOver2);
            aircraft.Translate(Vector3.UnitX);
            aircraft.Semimajor = 50f;
            aircraft.Semiminor = 50f;
            aircraft.Translate(aircraft.Up * 8f);
            aircraft.EllipseY = aircraft.Position.Y;

            Sphere sphere = new Sphere(20);
            sphere.ScaleObject(5);
            sphere.Color = Vector3.UnitX;
            sphere.SpecularExponent = 10f;
            sphere.SetPosition(new Vector3(-50f, 8f, 50f));
            sphere.InitalPosition = sphere.Position;

            Sphere sphere2 = new Sphere(20);
            sphere2.ScaleObject(5);
            sphere2.Color = Vector3.UnitY;
            sphere2.SpecularExponent = 500f;
            sphere2.SetPosition(new Vector3(50f, 8f, 50f));
            sphere2.InitalPosition = sphere2.Position;

            Cube cube = new Cube();
            cube.Color = Vector3.One;
            cube.SpecularExponent = 1f;
            cube.ScaleObject(new Vector3(1.2f, 10f, 1.2f));
            cube.SetPosition(new Vector3(60f, 5f, 0f));

            Sphere sphere3 = new Sphere(20);
            sphere3.ScaleObject(5);
            sphere3.Color = Vector3.UnitY;
            sphere3.SpecularExponent = 1000f;
            sphere3.Translate(cube.Position);
            sphere3.Translate(cube.Up * (cube.Scale.Y + 4.5f));
            sphere3.InitalPosition = sphere3.Position;

            SkyBox skybox = new SkyBox(TerrainLength);
            skybox.Texture = skyBoxTexture1;

            ///RenderObjects
            renderObjects.Add(terrain);
            renderObjects.Add(aircraft);

            for (int i = 0; i < 500; i++)
            {
                Tree tree = new Tree();
                tree.Texture = TreeTexture;
                float X = rnd.Next(2 * (int)TerrainLength);
                X -= TerrainLength / 2;
                X *= 2;
                float Z = rnd.Next(2 * (int)TerrainLength);
                Z -= TerrainLength / 2;
                Z *= 2;
                tree.SetPosition(new Vector3(X, 2f, Z));
                renderObjects.Add(tree);
            }

            renderObjects.Add(sphere);
            renderObjects.Add(sphere2);
            renderObjects.Add(cube);
            renderObjects.Add(sphere3);

            ///render last
            renderObjects.Add(skybox);

           /* MovingCamera moveCamera = new MovingCamera()
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, 90.39f),
                MoveSpeed = 1.5f
            };*/

            Camera staticCamera = new Camera()
            {
                CameraPosition = new Vector3(30f, 51.8f, -71.13f)
            };

            Camera staticFollowCamera = new StaticFollowCamera(aircraft)
            {
                CameraPosition = new Vector3(0f, 20f, 10f)
            };

            Camera staticFollowCamera2 = new Camera()
            {
                CameraPosition = new Vector3(sphere.Position.X, sphere.Position.Y + 15f, sphere.Position.Z - 20),
            };

            staticFollowCamera2.CameraTarget = sphere.Position;

            BehindCamera behindCamera = new BehindCamera(aircraft)
            {
                Dist = 20f,
                Angle = MathHelper.PiOver6
            };

            //Cameras
            //cameras.Add(moveCamera);

            cameras.Add(staticCamera);
            cameras.Add(staticFollowCamera);
            cameras.Add(staticFollowCamera2);
            cameras.Add(behindCamera);

            cameras[activeCameraIndex].IsActive = true;

            //Lights
            Light light = new Light()
            {
                Position = 50f * Vector3.UnitY,
                Color = Vector3.One,
                LightType = LightTypes.PointLight,
                AmbientIntensity = 0.1f,
                DiffuseIntensity = 0.8f,
                SpecularIntensity = 0.1f,
            };

            light.Direction = (Vector3.Zero - light.Position).Normalized();
            lights.Add(light);
            lights.AddRange(aircraft.Light);
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
            {
                obj.Update(deltaTime);
            }

            foreach (var cam in cameras)
            {
                cam.Update();
            }

            glControl.Invalidate();
        }

        private Random rnd = new Random();

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var obj in renderObjects)
            {
                obj.Bind();
                obj.Render(cameras[activeCameraIndex].GetViewMatrix(), ProjectionMatrix, lights, PhongLightinngModel, PhongShading);
                obj.UnBind();
            }

            glControl.SwapBuffers();
            frames++;
        }
        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.C:
                    {
                        cameras[activeCameraIndex].IsActive = false;
                        activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
                        cameras[activeCameraIndex].IsActive = true;
                    }
                    break;
                case Keys.H:
                    {
                        aircraft.HumanControl = !aircraft.HumanControl;
                        break;
                    }
                case Keys.P:
                    {
                        PhongShading = !PhongShading;
                        break;
                    }
                case Keys.L:
                    {
                        PhongLightinngModel = !PhongLightinngModel;
                        break;
                    }
                case Keys.G:
                    {
                        terrain.Texture = terrain.Texture == texture1 ? texture2 : texture1;
                        break;
                    }
            }
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GL.Viewport(glControl.Size);
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, glControl.Width / (float)glControl.Height, 1f, 1000f);
        }

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
                obj.Dispose();
            }
        }

        private void Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Resources.HelpMessage);
        }
    }
}
