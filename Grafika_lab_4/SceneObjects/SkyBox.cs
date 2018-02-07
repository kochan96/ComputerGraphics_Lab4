﻿using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Grafika_lab_4.SceneObjects
{
    public class SkyBox : RenderSceneObject
    {
        int vertexCount=36;
        float Size = 105.0f;

        public SkyBox(string name) : base(name)
        {
            CreateSkyBox();
        }

        SkyBoxRenderer renderer = SkyBoxRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }


        private void CreateSkyBox()
        {
            Vector3[] vertices = CreateVertices();
            Bind();
            SetVerticesBuffer(vertices);
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

        public override void Render()
        {
            renderer.EnableVertexAttribArrays();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {

        }


    }
}