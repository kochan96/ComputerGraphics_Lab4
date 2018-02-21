using Grafika_lab_4.Configuration;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
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
            Vector3[] normals = CreateNormals(vertices);
            Bind();
            SetVerticesBuffer(vertices);
            SetIndicesBuffer(indices);
            SetNormalsBuffer(normals);
            UnBind();
        }



        private Vector3[] CreateVertices()
        {
            Vector3[] vertices = new Vector3[(Total + 1) * (Total + 1)];
            for (int i = 0; i < Total + 1; i++)
            {
                float lon = Helper.MapValue(i, 0, Total, -MathHelper.Pi, MathHelper.Pi);
                for (int j = 0; j < Total + 1; j++)
                {
                    float lat = Helper.MapValue(j, 0, Total, -MathHelper.PiOver2, MathHelper.PiOver2);
                    float x = (float)(Math.Sin(lon) * Math.Cos(lat));
                    float y = (float)(Math.Sin(lon) * Math.Sin(lat));
                    float z = (float)Math.Cos(lon);
                    vertices[i * (Total + 1) + j] = new Vector3(x, y, z);
                }
            }

            return vertices;
        }


        int[] CreateIndices()
        {
            indicesCount = 2 * (Total + 1) + (2 * (Total + 1) + 2) * ((Total + 1) - 2);
            int[] indices = new int[indicesCount];
            int index = 0;
            for (int i = 0; i < Total; i++)
            {
                for (int j = 0; j < Total + 1; j++)
                {
                    //fragment of strip (bottom->up)
                    int vertex = i * Total + j;
                    indices[index++] = vertex;
                    indices[index++] = vertex + Total + 1;
                }
            }

            return indices;
        }



        private Vector3[] CreateNormals(Vector3[] vertices)
        {
            return vertices.Select(v => v.Normalized()).ToArray();
        }


        EntityRenderer renderer = EntityRenderer.Instance;
        public override Renderer Renderer { get { return renderer; } }

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

        public float SpecularExponent { get; set;}

        public override void Render()
        {
            renderer.SetHasTexture(Texture != null);
            renderer.SetAmbientColor(Color);
            renderer.SetDiffuseColor(Color);
            renderer.SetSpecularColor(Color);
            renderer.SetSpecularExponenet(SpecularExponent);
            renderer.EnableVertexAttribArrays();
            GL.DrawElements(BeginMode.TriangleStrip, indicesCount, DrawElementsType.UnsignedInt, 0);
            renderer.DisableVertexAttribArrays();
        }

        Vector3 position;
        bool firstMove=true;
        float time = 0.0f;
        public override void Update(float deltatime)
        {
            if(firstMove)
            {
                firstMove = false;
                position = Position;
            }
            time += deltatime;
            time %= MathHelper.Pi;
            float Y = position.Y + (float)Math.Sin(time);
            Vector3 nextPosition = new Vector3(position.X, Y, position.Z);
            Translate(nextPosition - Position);

        }


    }
}
