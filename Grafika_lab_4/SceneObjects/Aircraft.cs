using Grafika_lab_4.Configuration;
using Grafika_lab_4.Loader;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafika_lab_4.SceneObjects
{
    public class Aircraft : RenderSceneObject
    {

        #region Fields
        AircraftRenderer renderer = AircraftRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }

        public RawObjModel RawModel;
        /*
        protected override Vector3 DefaultForward { get { return Vector3.UnitX; } }
        protected override Vector3 DefaultUp { get { return Vector3.UnitZ; } }
        protected override Vector3 DefaultRight { get { return -Vector3.UnitY; } }*/

        public float Speed { get; set; }

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

        float Semiminor = 70.0f;
        float Semimajor = 10.0f;

        Vector2 CenterOfElipse=Vector2.Zero;
        float alpha = 0;

        public override void Update(float deltatime)
        {

            alpha += Speed * deltatime;
            float X = CenterOfElipse.X-(Semiminor * (float)Math.Cos(alpha));
            float Y = CenterOfElipse.Y-(Semimajor * (float)Math.Sin(alpha));
            Vector3 position = new Vector3(X, 0, Y);
            Translate(position-Position);
            Vector3 old = Position;
            Translate(-old);
            Yaw(-Speed * deltatime);
            Translate(old);


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
