using Grafika_lab_4.Renderers;
using Grafika_lab_4.Textures;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafika_lab_4.SceneObjects
{
    public class Terrain
    {
        #region Fields
        readonly float MAX_PIXEL_COLOR = 256 * 256 * 256;
        int vertexArray;
        int vertexBuffer;
        int elementsBuffer;
        int vertexCountX;
        int vertexCountY;
        int indicesCount;
        int textureBuffer;
        int normalsBuffer;
        public TerrainRenderer Renderer { get; }
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Texture Texture { get; set; }
        #endregion

        #region Constructors

        public Terrain(string HeightMap, string NormalMap = "")
        {
            Bitmap heightMap = LoadBitmap(HeightMap);
            Bitmap normalMap = LoadBitmap(NormalMap);

            if (heightMap != null)
            {
                vertexCountX = heightMap.Width;
                vertexCountY = heightMap.Height;
                if (normalMap != null)
                    normalMap = new Bitmap(normalMap, heightMap.Size);
            }
            else if (normalMap != null)
            {
                vertexCountX = normalMap.Width;
                vertexCountY = normalMap.Height;
            }
            else
            {
                vertexCountX = 10;
                vertexCountY = 10;
            }
            Renderer = new TerrainRenderer();
            CheckVertexCount();
            GenerateBuffers();
            CreateTerrain(heightMap, normalMap);
        }

        public Terrain(int vertexCount) : this(vertexCount, vertexCount) { }

        public Terrain(int vertexCountX, int vertexCountY)
        {
            this.vertexCountX = vertexCountX;
            this.vertexCountY = vertexCountY;
            CheckVertexCount();

            Renderer = new TerrainRenderer();
            GenerateBuffers();
            CreateTerrain(null, null);
        }

        private void CheckVertexCount()
        {
            if (vertexCountX < 2)
                throw new Exception("VertexCountX in Terrain can not be less than two");

            if (vertexCountY < 2)
                throw new Exception("VertexCountY in Terrain can not be less than two");
        }

        private Bitmap LoadBitmap(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return null;

            try
            {
                return new Bitmap(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        private void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out textureBuffer);
            GL.GenBuffers(1, out normalsBuffer);
        }

        private void CreateTerrain(Bitmap heightmap, Bitmap normalMap)
        {
            Vector3[] vertices = CreateVertices(heightmap);
            int[] indices = CreateIndices();
            Vector2[] textcoord = CreateTextureCoordinates(vertices);
            Vector3[] normals;
            if (normalMap != null)
                normals = CreateNormals(normalMap);
            else
                normals = CreateNormals(vertices,indices);

            GL.BindVertexArray(vertexArray);
            SetVerticesBuffer(vertices);
            SetIndicesBuffer(indices);
            SetTextureBuffer(textcoord);
            SetNormalsBuffer(normals);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        #region Vertices
        private Vector3[] CreateVertices(Bitmap bmp)
        {

            Vector3[] vertices = new Vector3[vertexCountX * vertexCountY];
            float spacingX = 2 / (float)(vertexCountX - 1);
            float spacingY = 2 / (float)(vertexCountY - 1);
            int index = 0;
            for (int i = 0; i < vertexCountY; i++)
            {
                for (int j = 0; j < vertexCountX; j++)
                {
                    vertices[index] = new Vector3(-1 + j * spacingX, -1 + i * spacingY, GetHeight(bmp, j, i));
                    index++;
                }
            }
            return vertices;
        }

        private float GetHeight(Bitmap bmp, int x, int y)
        {
            if (bmp == null || x < 0 || x >= bmp.Height || y < 0 || y >= bmp.Width)
                return 0.0f;

            Color pixel = bmp.GetPixel(x, y);
            float value = pixel.R * pixel.G * pixel.B;
            value -= MAX_PIXEL_COLOR / 2f;
            value /= MAX_PIXEL_COLOR / 2f;
            return value;
        }

        private void SetVerticesBuffer(Vector3[] vertices)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.PositionDataLocation, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        #endregion

        #region Indices
        private int[] CreateIndices()
        {
            indicesCount = 2 * vertexCountX + (2 * vertexCountX + 2) * (vertexCountY - 2);
            int[] indices = new int[indicesCount];
            int index = 0;
            bool finish = false;
            for (int i = 0; !finish;)
            {
                for (int j = 0; j < vertexCountX; j++)
                {
                    //fragment of strip (bottom->up)
                    int vertex = i * vertexCountX + j;
                    indices[index++] = vertex;
                    indices[index++] = vertex + vertexCountX;
                }

                i = i + 1;

                if (i < vertexCountY - 1)//if not last row
                {
                    //repeat last vertex of this strip and first vertex of next row
                    indices[index++] = (i + 1) * vertexCountX - 1;
                    indices[index++] = i * vertexCountX;
                }
                else
                {
                    finish = true;
                }
            }

            return indices;
        }

        private void SetIndicesBuffer(int[] indices)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
        }
        #endregion

        #region Texture
        private Vector2[] CreateTextureCoordinates(Vector3[] vertices)
        {
            Vector2[] textcoord = new Vector2[vertices.Length];
            for (int i = 0; i < textcoord.Length; i++)
            {
                textcoord[i] = new Vector2(1.0f - ((vertices[i].X + 1) / 2), 1.0f - ((vertices[i].Y + 1) / 2));
            }

            return textcoord;
        }

        private void SetTextureBuffer(Vector2[] textcoord)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, textcoord.Length * Vector2.SizeInBytes, textcoord, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.TextCoordLocation, 2, VertexAttribPointerType.Float, true, 0, 0);
        }

        #endregion

        #region Normals
        private Vector3[] CreateNormals(Vector3[] vertices, int[] indices)
        {
            Vector3[] normals = new Vector3[vertices.Length];

            int Xmax = vertexCountX;
            int YMax = vertexCountY;
            for(int i=0;i<normals.Length;i++)
            {
                int nextX = i + 1;
                int nextY = i + vertexCountX;

                if (nextX % vertexCountX == 0)
                    nextX -= vertexCountX;

                if (nextY >= vertexCountX * vertexCountY)
                    nextY = 0;


                float dhx = vertices[nextX].Z - vertices[i].Z;
                float dhy = vertices[nextY].Z - vertices[i].Z;

                normals[i] = Vector3.UnitZ + Vector3.UnitZ * dhx + Vector3.UnitY * dhy;
                normals[i] = normals[i].Normalized();
            }

            return normals;
        }

        private Vector3[] CreateNormals(Bitmap normalMap)
        {
            Vector3[] normals = new Vector3[vertexCountX * vertexCountY];
            int index = 0;
            for (int i = 0; i < vertexCountY; i++)
            {
                for (int j = 0; j < vertexCountX; j++)
                {
                    Color pixel = normalMap.GetPixel(j, i);
                    float half = 255f / 2.0f;
                    normals[index].X = (pixel.R - half) / half;
                    normals[index].Y = (pixel.G - half) / half;
                    normals[index].Z = (pixel.B - half) / half;
                    index++;
                }
            }


            return normals;
        }

        private void SetNormalsBuffer(Vector3[] normals)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalsBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * Vector3.SizeInBytes, normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Renderer.NormalsDataLocation, 3, VertexAttribPointerType.Float, true, 0, 0);
        }
        #endregion


        public void Render()
        {
            Renderer.EnableVertexAttribArrays();
            GL.BindVertexArray(vertexArray);
            GL.DrawElements(BeginMode.TriangleStrip, indicesCount, DrawElementsType.UnsignedInt, 0);
            Renderer.DisableVertexAttribArrays();
        }
    }
}
