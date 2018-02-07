using Grafika_lab_4.Configuration;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Grafika_lab_4.SceneObjects
{
    public class Sphere : RenderSceneObject
    {
        protected int colorBuffer;
        int Total;
        int indicesCount;

        public Sphere(string name, int RowColumnNumberOfPoints) : base(name)
        {
            Total = RowColumnNumberOfPoints;
            CreateSphere(null);
        }

        private void CreateSphere(Bitmap bmp)
        {
            Vector3[] vertices = CreateVertices();
            int[] indices = CreateIndices();
            Vector3[] normals = CreateNormals(vertices,indices);
            Vector3[] colors = CreateColors();
            Vector2[] textureCoord = CreateTextureCoordinates(vertices);
            Bind();
            SetVerticesBuffer(vertices);
            SetIndicesBuffer(indices);
            SetTextureBuffer(textureCoord);
            SetNormalsBuffer(normals);
            SetColorBuffer(colors);
            UnBind();
        }

        public float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        private Vector3[] CreateVertices()
        {
            Vector3[] vertices = new Vector3[(Total+1) * (Total+1)];
            for (int i = 0; i < Total+1; i++)
            {
                float lon = Map(i, 0, Total, -MathHelper.Pi, MathHelper.Pi);
                for (int j = 0; j < Total+1; j++)
                {
                    float lat = Map(j, 0, Total, -MathHelper.PiOver2, MathHelper.PiOver2);
                    float x = (float)(Math.Sin(lon) * Math.Cos(lat));
                    float y = (float)(Math.Sin(lon) * Math.Sin(lat));
                    float z = (float)Math.Cos(lon);
                    vertices[i * (Total+1) + j] = new Vector3(x, y, z);
                }
            }

            return vertices;
        }

        private Vector2[] CreateTextureCoordinates(Vector3[] vertices)
        {
            Vector2[] textcoord = new Vector2[vertices.Length];
            for (int i = 0; i < textcoord.Length; i++)
            {
                textcoord[i] = new Vector2(1.0f - ((vertices[i].X + 1) / 2), 1.0f - ((vertices[i].Y + 1) / 2));
            }

            return textcoord;
        }

        int[] CreateIndices()
        {
            indicesCount = 2 * (Total+1) + (2 * (Total+1) + 2) * ((Total+1) - 2);
            int[] indices = new int[indicesCount];
            int index = 0;
            for (int i = 0; i<Total;i++)
            {
                for (int j = 0; j < Total+1; j++)
                {
                    //fragment of strip (bottom->up)
                    int vertex = i * Total + j;
                    indices[index++] = vertex;
                    indices[index++] = vertex + Total+1;
                }
            }

            return indices;
        }



        private Vector3[] CreateNormals(Vector3[] vertices,int[] indices)
        {
            Vector3[] normals=new Vector3[vertices.Length];
            for (int i = 0; i < indices.Length-2; i += 3)
            {
                Vector3 v1 = vertices[indices[i]];
                Vector3 v2 = vertices[indices[i + 1]];
                Vector3 v3 = vertices[indices[i + 2]];

                // The normal is the cross product of two sides of the triangle
                normals[indices[i]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[indices[i + 1]] += Vector3.Cross(v2 - v1, v3 - v1);
                normals[indices[i + 2]] += Vector3.Cross(v2 - v1, v3 - v1);
            }

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = normals[i].Normalized();
            }

            return normals;
        }

        Random rnd = new Random();
        private Vector3[] CreateColors()
        {
            return Enumerable.Repeat(Vector3.Zero,(Total+1)*(Total+1))
                .Select(x => new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble())).ToArray();
        }

        StaticRenderer renderer = StaticRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out colorBuffer);
            GL.GenBuffers(1, out normalsBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out textureBuffer);
        }

        private void SetColorBuffer(Vector3[] colors)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * Vector3.SizeInBytes, colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(renderer.ColorDataLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(colorBuffer);
            GL.DeleteBuffer(textureBuffer);
            GL.DeleteBuffer(normalsBuffer);
            GL.DeleteBuffer(elementsBuffer);
        }

        public override void Render()
        {
            renderer.SetHasTexture(Texture != null);
            renderer.EnableVertexAttribArrays();
            GL.DrawElements(BeginMode.TriangleStrip,indicesCount,DrawElementsType.UnsignedInt,0);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {
            Roll(5f * deltatime);
            Pitch(5f * deltatime);
        }


    }
}
