using Grafika_lab_4.Configuration;
using Grafika_lab_4.Lights;
using Grafika_lab_4.Loader;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Grafika_lab_4.SceneObjects
{
    public class Tree : RenderSceneObject
    {

        EntityRenderer renderer = EntityRenderer.Instance;
        public RawObjModel RawModel;

        public Tree(Vector3 position) : base(position)
        {
            string modelFile = Resources.TreeModel;
            if (File.Exists(modelFile))
            {
                CreateTree(modelFile);
            }
        }

        public Tree() : this(Vector3.Zero) { }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out normalsBuffer);
            GL.GenBuffers(1, out textureBuffer);
        }

        private void CreateTree(string filePath)
        {
            RawObjModel model = RawObjLoader.LoadRawObj(filePath);
            if (model == null)
            {
                MessageBox.Show("Could not load Model" + filePath);
                return;
            }
            else if (model.Meshes == null || !model.Meshes.Any())
            {
                MessageBox.Show("Loaded model is empty" + filePath);
                return;
            }

            Bind();
            List<int> indices = new List<int>();
            foreach (Mesh mesh in model.Meshes)
            {
                indices.AddRange(mesh.Indices);
            }
            RawModel = model;
            SetVerticesBuffer(model.Vertices.ToArray(), renderer.PositionAttribute);
            SetIndicesBuffer(indices.ToArray());
            SetNormalsBuffer(model.Normals.ToArray(), renderer.NormalAttribute);
            SetTextureBuffer(model.TextureCoordinates.ToArray(), renderer.TextureCoordAttribute);
            UnBind();
        }

        public override void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix, List<Light> lights, bool PhongLightningModel, bool PhongShading)
        {
            if (RawModel != null)
            {
                int offset = 0;
                renderer.Use();
                renderer.SetModelMatrix(ModelMatrix);
                renderer.SetProjectionMatrix(projectionMatrix);
                renderer.SetViewMatrix(viewMatrix);
                renderer.SetHasTexture(Texture != null);
                renderer.SetPhongLightning(PhongLightningModel);
                renderer.SetDiscard(true);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, Texture.TextureId);
                renderer.EnableVertexAttribArrays();
                foreach (Mesh mesh in RawModel.Meshes)
                {
                    renderer.SetAmbientColor(mesh.MeshMaterial.Ka);
                    renderer.SetDiffuseColor(mesh.MeshMaterial.Kd);
                    renderer.SetSpecularColor(mesh.MeshMaterial.Ks);
                    renderer.SetSpecularExponent(mesh.MeshMaterial.Ns);
                    GL.DrawElements(BeginMode.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt, offset * sizeof(uint));
                    offset += mesh.Indices.Count;
                }
                renderer.DisableVertexAttribArrays();
            }
        }

        public override void Update(float deltatime)
        {
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(elementsBuffer);
            GL.DeleteBuffer(normalsBuffer);
            GL.DeleteBuffer(textureBuffer);
        }
    }
}
