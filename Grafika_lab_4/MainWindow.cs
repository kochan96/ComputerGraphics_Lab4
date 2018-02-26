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
        Light EditLight;
        List<Camera> cameras = new List<Camera>();
        int activeCameraIndex = 0;
        List<Light> lights = new List<Light>();
        List<RenderSceneObject> renderObjects = new List<RenderSceneObject>();
        Vector3 SkyColor = new Vector3(0.5f, 0.5f, 0.5f);
        bool PhongLightinngModel=true;
        bool PhongShading = true;
        Aircraft aircraft;
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
            Texture skyBoxTexture1 = TextureLoader.LoadCubeMap(Resources.SkyCubeMapTextures);
            Texture TreeTexture = TextureLoader.LoadTexture2D(Resources.TreeTexture);
            ///Terain
            Terrain terrain = new Terrain("Terrain", Resources.MountainsHeightMap)
            {
                Texture = texture1,
            };
            float TerrainLength = 300f;
            float TerrainHeight = 20f;

            terrain.ScaleObject(new Vector3(TerrainLength, TerrainLength, TerrainHeight));
            terrain.Pitch(-MathHelper.PiOver2);
            

            ///FirstAircraft
            aircraft = new Aircraft("Aircraft1");
            aircraft.Speed = 0.4f;
            aircraft.HumanControlSpeed = 20f;
            aircraft.ScaleObject(100f);
            aircraft.RotateByY(MathHelper.PiOver2);
            aircraft.RotateByZ(MathHelper.PiOver2);
            aircraft.Translate(Vector3.UnitX);
            aircraft.Semimajor = 50f;
            aircraft.Semiminor =50f;
            aircraft.Translate(aircraft.Up * 8f);
            aircraft.HumanControl = true;
            aircraft.HumanControl = false;

            /* ControlableAircraft controlAircraft = new ControlableAircraft("PlayerAircrft");
             controlAircraft.Speed = 0.5f;
             controlAircraft.ScaleObject(50f);
             controlAircraft.RotateByY(MathHelper.PiOver2);
             controlAircraft.RotateByZ(MathHelper.PiOver2);
             controlAircraft.RotateAndChange(MathHelper.Pi, Vector3.UnitY);
             controlAircraft.Translate(-20f * controlAircraft.Forward);
             controlAircraft.Translate(aircraft.Up * 5f);*/



            Sphere sphere = new Sphere("spher", 100);
            sphere.ScaleObject(5);
            sphere.Color = Vector3.UnitX;
            sphere.SpecularExponent = 10f;
            sphere.SetPosition(new Vector3(-50f, 8f, 50f));

            Sphere sphere2 = new Sphere("sphere2", 100);
            sphere2.ScaleObject(5);
            sphere2.Color = Vector3.UnitY;
            sphere2.SpecularExponent = 500f;
            sphere2.SetPosition(new Vector3(50f, 8f, 50f));

            Cube cube = new Cube("cube");
            cube.Color = Vector3.One;
            cube.SpecularExponent = 1f;
            cube.ScaleObject(new Vector3(1.2f, 10f,1.2f));
            cube.SetPosition(new Vector3(60f, 5f, 0f));

            Sphere sphere3 = new Sphere("sphere2", 100);
            sphere3.ScaleObject(5);
            sphere3.Color = Vector3.UnitY;
            sphere3.SpecularExponent = 1000f;
            sphere3.Translate(cube.Position);
            sphere3.Translate(cube.Up * (cube.Scale.Y+1.5f));

            SkyBox skybox = new SkyBox("SkyBox",TerrainLength);
            skybox.Texture = skyBoxTexture1;


            ///RenderObjects
            renderObjects.Add(terrain);
            renderObjects.Add(aircraft);

            for(int i = 0; i < 500; i++)
            {
                Tree tree = new Tree("");
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

            MovingCamera moveCamera = new MovingCamera("MovingCamera")
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, 90.39f),
                MoveSpeed = 1.5f
            };
            Camera staticCamera = new StaticCamera("StaticCamera")
            {
                CameraPosition = new Vector3(-2.2f, 45.8f, -90.39f)
            };
            Camera staticFollowCamera = new StaticCamera("StaticFollowCamera", aircraft)
            {
                CameraPosition = new Vector3(0f, 20f, 10f)
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

            //Lights
            Light light = new Light("Light")
            {
                Position = 50f* Vector3.UnitY,
                Color=Vector3.One,
                LightType=LightTypes.PointLight,
                AmbientIntensity=0.1f,
                DiffuseIntensity=0.8f,
                SpecularIntensity=0.1f
            };

          

            light.Direction = (Vector3.Zero - light.Position).Normalized();
            lights.Add(light);
            lights.AddRange(aircraft.Light);
            //lights.AddRange(controlAircraft.Light);
            //
            
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



            glControl.Invalidate();
        }

        Random rnd = new Random();

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            foreach (var obj in renderObjects)
            {
                obj.Bind();
                obj.Render(cameras[activeCameraIndex].GetViewMatrix(),ProjectionMatrix,lights,PhongLightinngModel,PhongShading);
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
            else if(e.KeyCode==Keys.H)
            {
                aircraft.HumanControl = !aircraft.HumanControl;
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
                obj.Dispose();
            }
        }






        #endregion

        private void CombobBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShadingCombobBox == sender)
            {
                PhongShading = ShadingCombobBox.SelectedIndex==0;
            }else if(LightningComboBox==sender)
            {
                PhongLightinngModel = LightningComboBox.SelectedIndex == 0;
            }


        }

    }
}
