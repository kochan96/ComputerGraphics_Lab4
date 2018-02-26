using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Grafika_lab_4.SceneObjects
{
    public class SkyBox : RenderSceneObject
    {
        int vertexCount=36;
        float Size;
        public SkyBox(string name,float Size=105) : base(name)
        {
            this.Size = Size;
            CreateSkyBox();
        }

        SkyBoxRenderer renderer = SkyBoxRenderer.Instance;


        private void CreateSkyBox()
        {
            Vector3[] vertices = CreateVertices();
            Bind();
            SetVerticesBuffer(vertices,renderer.PositionLocation);
            UnBind();
        }

        private Vector3[] CreateVertices()
        {
            return new Vector3[]
            {
               new Vector3(-Size, -Size, -Size),
               new Vector3(-Size, -Size, Size),
               new Vector3(-Size, Size, -Size),
               new Vector3(-Size, Size, -Size),
               new Vector3(-Size, -Size, Size),
               new Vector3(-Size, Size, Size),

               new Vector3(Size, -Size, -Size),
               new Vector3(Size, Size, -Size),
               new Vector3(Size, -Size, Size),
               new Vector3(Size, -Size, Size),
               new Vector3(Size, Size, -Size),
               new Vector3(Size, Size, Size),

               new Vector3(-Size, -Size, -Size),
               new Vector3(Size, -Size, -Size),
               new Vector3(-Size, -Size, Size),
               new Vector3(-Size, -Size, Size),
               new Vector3(Size, -Size, -Size),
               new Vector3(Size, -Size, Size),

               new Vector3(-Size, Size, -Size),
               new Vector3(-Size, Size, Size),
               new Vector3(Size, Size, -Size),
               new Vector3(Size, Size, -Size),
               new Vector3(-Size, Size, Size),
               new Vector3(Size, Size, Size),

               new Vector3(-Size, -Size, -Size),
               new Vector3(-Size, Size, -Size),
               new Vector3(Size, -Size, -Size),
               new Vector3(Size, -Size, -Size),
               new Vector3(-Size, Size, -Size),
               new Vector3(Size, Size, -Size),

               new Vector3(-Size, -Size, Size),
               new Vector3(Size, -Size, Size),
               new Vector3(-Size, Size, Size),
               new Vector3(-Size, Size, Size),
               new Vector3(Size, -Size, Size),
               new Vector3(Size, Size, Size),
            };
        }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
        }

        public override void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix, List<Light> lights, bool PhongLightningModel, bool PhongShading)
        {
            renderer.Use();
            renderer.EnableVertexAttribArrays();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {

        }


    }
}
