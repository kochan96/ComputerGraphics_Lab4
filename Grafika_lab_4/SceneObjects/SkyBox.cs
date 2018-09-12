using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Grafika_lab_4.SceneObjects
{
    public class SkyBox : RenderSceneObject
    {
        private const int _vertexCount = 36;
        private readonly float _size;
        private readonly SkyBoxRenderer renderer = SkyBoxRenderer.Instance;

        public SkyBox(float Size = 105)
        {
            _size = Size;
            Vector3[] vertices = CreateVertices();
            Bind();
            SetVerticesBuffer(vertices, renderer.PositionAttribute);
            UnBind();
        }

        private Vector3[] CreateVertices()
        {
            return new Vector3[]
            {
               new Vector3(-_size, -_size, -_size),
               new Vector3(-_size, -_size, _size),
               new Vector3(-_size, _size, -_size),
               new Vector3(-_size, _size, -_size),
               new Vector3(-_size, -_size, _size),
               new Vector3(-_size, _size, _size),

               new Vector3(_size, -_size, -_size),
               new Vector3(_size, _size, -_size),
               new Vector3(_size, -_size, _size),
               new Vector3(_size, -_size, _size),
               new Vector3(_size, _size, -_size),
               new Vector3(_size, _size, _size),

               new Vector3(-_size, -_size, -_size),
               new Vector3(_size, -_size, -_size),
               new Vector3(-_size, -_size, _size),
               new Vector3(-_size, -_size, _size),
               new Vector3(_size, -_size, -_size),
               new Vector3(_size, -_size, _size),

               new Vector3(-_size, _size, -_size),
               new Vector3(-_size, _size, _size),
               new Vector3(_size, _size, -_size),
               new Vector3(_size, _size, -_size),
               new Vector3(-_size, _size, _size),
               new Vector3(_size, _size, _size),

               new Vector3(-_size, -_size, -_size),
               new Vector3(-_size, _size, -_size),
               new Vector3(_size, -_size, -_size),
               new Vector3(_size, -_size, -_size),
               new Vector3(-_size, _size, -_size),
               new Vector3(_size, _size, -_size),

               new Vector3(-_size, -_size, _size),
               new Vector3(_size, -_size, _size),
               new Vector3(-_size, _size, _size),
               new Vector3(-_size, _size, _size),
               new Vector3(_size, -_size, _size),
               new Vector3(_size, _size, _size),
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
            renderer.SetViewMatrix(viewMatrix);
            renderer.SetProjectionMatrix(projectionMatrix);
            renderer.EnableVertexAttribArrays();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {
        }
    }
}
