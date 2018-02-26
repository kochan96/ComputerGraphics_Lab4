using Grafika_lab_4.Lights;
using Grafika_lab_4.Loader;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.SceneObjects
{
    public class Cube : RenderSceneObject
    {

        int vertexCount = 36;
        public Cube(string name):base(name)
        {
            CreateCube();
        }

        public Vector3 Color { get; set; }
        public float SpecularExponent { get; set; }

        EntityRenderer renderer = EntityRenderer.Instance;

        private void CreateCube()
        {
            Vector3[] vertices = CreateVertices();
            Vector3[] normals = CreateNormals();
            Bind();
            SetVerticesBuffer(vertices,renderer.PositonLocation);
            SetNormalsBuffer(normals,renderer.NormalLocation);
            UnBind();
        }

        private Vector3[] CreateVertices()
        {
            return new Vector3[]
            {
                //left
               new Vector3(-1.0f, -1.0f, -1.0f),
               new Vector3(-1.0f, -1.0f, 1.0f),
               new Vector3(-1.0f, 1.0f, -1.0f),
               new Vector3(-1.0f, 1.0f, -1.0f),
               new Vector3(-1.0f, -1.0f, 1.0f),
               new Vector3(-1.0f, 1.0f, 1.0f),

               //right
               new Vector3(1.0f, -1.0f, -1.0f),
               new Vector3(1.0f, 1.0f, -1.0f),
               new Vector3(1.0f, -1.0f, 1.0f),
               new Vector3(1.0f, -1.0f, 1.0f),
               new Vector3(1.0f, 1.0f, -1.0f),
               new Vector3(1.0f, 1.0f, 1.0f),

               //bottom
               new Vector3(-1.0f, -1.0f, -1.0f),
               new Vector3(1.0f, -1.0f, -1.0f),
               new Vector3(-1.0f, -1.0f, 1.0f),
               new Vector3(-1.0f, -1.0f, 1.0f),
               new Vector3(1.0f, -1.0f, -1.0f),
               new Vector3(1.0f, -1.0f, 1.0f),

               //top
               new Vector3(-1.0f, 1.0f, -1.0f),
               new Vector3(-1.0f, 1.0f, 1.0f),
               new Vector3(1.0f, 1.0f, -1.0f),
               new Vector3(1.0f, 1.0f, -1.0f),
               new Vector3(-1.0f, 1.0f, 1.0f),
               new Vector3(1.0f, 1.0f, 1.0f),

               //back
               new Vector3(-1.0f, -1.0f, -1.0f),
               new Vector3(-1.0f, 1.0f, -1.0f),
               new Vector3(1.0f, -1.0f, -1.0f),
               new Vector3(1.0f, -1.0f, -1.0f),
               new Vector3(-1.0f, 1.0f, -1.0f),
               new Vector3(1.0f, 1.0f, -1.0f),

               //front
               new Vector3(-1.0f, -1.0f, 1.0f),
               new Vector3(1.0f, -1.0f, 1.0f),
               new Vector3(-1.0f, 1.0f, 1.0f),
               new Vector3(-1.0f, 1.0f, 1.0f),
               new Vector3(1.0f, -1.0f, 1.0f),
               new Vector3(1.0f, 1.0f, 1.0f),
            };
        }

        private Vector3[] CreateNormals()
        {
            return new Vector3[]
            {
                 //left
               -Vector3.UnitX,
               -Vector3.UnitX,
               -Vector3.UnitX,
               -Vector3.UnitX,
               -Vector3.UnitX,
               -Vector3.UnitX,

               //right
               Vector3.UnitX,
               Vector3.UnitX,
               Vector3.UnitX,
               Vector3.UnitX,
               Vector3.UnitX,
               Vector3.UnitX,

               //bottom
               -Vector3.UnitY,
               -Vector3.UnitY,
               -Vector3.UnitY,
               -Vector3.UnitY,
               -Vector3.UnitY,
               -Vector3.UnitY,

               //top
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,
               Vector3.UnitY,

               //back
               -Vector3.UnitZ,
               -Vector3.UnitZ,
               -Vector3.UnitZ,
               -Vector3.UnitZ,
               -Vector3.UnitZ,
               -Vector3.UnitZ,

               //front
               Vector3.UnitZ,
               Vector3.UnitZ,
               Vector3.UnitZ,
               Vector3.UnitZ,
               Vector3.UnitZ,
               Vector3.UnitZ,
            };
        }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out normalsBuffer);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(normalsBuffer);
        }

        public override void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix, List<Light> lights, bool PhongLightningModel, bool PhongShading)
        {
            renderer.SetHasTexture(Texture != null);
            renderer.SetAmbientColor(Color);
            renderer.SetDiffuseColor(Color);
            renderer.SetSpecularColor(Color);
            renderer.SetSpecularExponent(SpecularExponent);
            renderer.EnableVertexAttribArrays();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {
            
        }

       
    }
}
