using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Grafika_lab_4.SceneObjects
{
    public class Sphere : RenderSceneObject
    {
        protected int _colorBuffer;
        private readonly int _total;
        private int _indicesCount;
        private float time = 0.0f;
        private EntityRenderer renderer = EntityRenderer.Instance;

        public Vector3 InitalPosition { get; set; }

        public Sphere(int RowColumnNumberOfPoints)
        {
            _total = RowColumnNumberOfPoints;
            CreateSphere();
        }

        private void CreateSphere(Bitmap bmp = null)
        {
            Vector3[] vertices = CreateVertices();
            int[] indices = CreateIndices();
            Vector3[] normals = CreateNormals(vertices);
            Bind();
            SetVerticesBuffer(vertices, renderer.PositionAttribute);
            SetIndicesBuffer(indices);
            SetNormalsBuffer(normals, renderer.NormalAttribute);
            UnBind();
        }

        private Vector3[] CreateVertices()
        {
            Vector3[] vertices = new Vector3[(_total + 1) * (_total + 1)];
            for (int i = 0; i < _total + 1; i++)
            {
                float lon = Extensions.MapValue(i, 0, _total - 1, -MathHelper.Pi, MathHelper.Pi);
                for (int j = 0; j < _total + 1; j++)
                {
                    float lat = Extensions.MapValue(j, 0, _total - 1, -MathHelper.PiOver2, MathHelper.PiOver2);
                    float x = (float)(Math.Sin(lon) * Math.Cos(lat));
                    float y = (float)(Math.Sin(lon) * Math.Sin(lat));
                    float z = (float)Math.Cos(lon);
                    vertices[i * (_total + 1) + j] = new Vector3(x, y, z);
                }
            }

            return vertices;
        }

        private int[] CreateIndices()
        {
            _indicesCount = 2 * (_total + 1) + (2 * (_total + 1) + 2) * ((_total + 1) - 2);
            int[] indices = new int[_indicesCount];
            int index = 0;
            for (int i = 0; i < _total; i++)
            {
                for (int j = 0; j < _total + 1; j++)
                {
                    //fragment of strip (bottom->up)
                    int vertex = i * _total + j;
                    indices[index++] = vertex;
                    indices[index++] = vertex + _total + 1;
                }
            }

            return indices;
        }

        private Vector3[] CreateNormals(Vector3[] vertices)
        {
            return vertices.Select(v => v.Normalized()).ToArray();
        }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out normalsBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out textureBuffer);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(textureBuffer);
            GL.DeleteBuffer(normalsBuffer);
            GL.DeleteBuffer(elementsBuffer);
        }
        public Vector3 Color { get; set; }

        public float SpecularExponent { get; set; }

        public override void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix, List<Light> lights, bool PhongLightningModel, bool PhongShading)
        {
            renderer.SetModelMatrix(ModelMatrix);
            renderer.SetProjectionMatrix(projectionMatrix);
            renderer.SetViewMatrix(viewMatrix);
            renderer.SetLights(lights);
            renderer.SetHasTexture(Texture != null);
            renderer.SetAmbientColor(Color);
            renderer.SetDiffuseColor(Color);
            renderer.SetSpecularColor(Color);
            renderer.SetSpecularExponent(SpecularExponent);
            renderer.SetPhongLightning(PhongLightningModel);
            renderer.EnableVertexAttribArrays();
            GL.DrawElements(BeginMode.TriangleStrip, _indicesCount, DrawElementsType.UnsignedInt, 0);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {
            time = time + deltatime;
            time = time % MathHelper.Pi;
            float Y = InitalPosition.Y + 30 * (float)Math.Sin(15 * time);
            Vector3 nextPosition = new Vector3(InitalPosition.X, Y, InitalPosition.Z);
            Translate(nextPosition - Position);
        }
    }
}
