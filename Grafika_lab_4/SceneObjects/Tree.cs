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
    public class Tree : RenderSceneObject
    {

        #region Fields
        EntityRenderer renderer = EntityRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }

        public RawObjModel RawModel;

        #endregion

        public Tree(string name, Vector3 position) : base(name, position)
        {
            string modelFile = Resources.TreeModel;
            if (File.Exists(modelFile))
            {
                CreateTree(modelFile);
            }
            else
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.FileMissingError) + modelFile);



        }
        public Tree(string name) : this(name, Vector3.Zero) { }


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
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadModelError) + filePath);
                return;
            }
            else if (model.Meshes == null || model.Meshes.Count == 0)
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadedEmptyModelError) + filePath);
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
            SetTextureBuffer(model.TextureCoordinates.ToArray());
            UnBind();
        }

        public override void Render()
        {
            if (RawModel != null)
            {
                int offset = 0;
                renderer.SetHasTexture(Texture != null);
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
