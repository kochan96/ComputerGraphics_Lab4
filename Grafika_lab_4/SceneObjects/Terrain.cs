using Grafika_lab_4.Lights;
using Grafika_lab_4.Renderers;
using Grafika_lab_4.SceneObjects.Base;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grafika_lab_4.Configuration;
using Grafika_lab_4.Loader;

namespace Grafika_lab_4.SceneObjects
{
    public class Terrain : RenderSceneObject
    {
        private readonly float MAX_PIXEL_COLOR = 256 * 256 * 256;
        private int _vertexCountX;
        private int _vertexCountY;

        private readonly EntityRenderer renderer = EntityRenderer.Instance;

        private int _indicesCount;

        public Terrain(string HeightMap, string NormalMap = "")
        {
            Bitmap heightMap = LoadBitmap(HeightMap);
            Bitmap normalMap = LoadBitmap(NormalMap);
            {
                SetVerticesCount(heightMap, normalMap);
                CheckVertexCount();
                GenerateBuffers();
                CreateTerrain(heightMap, normalMap);
            }
        }

        public Terrain(int vertexCountX, int vertexCountY)
        {
            _vertexCountX = vertexCountX;
            _vertexCountY = vertexCountY;
            CheckVertexCount();
            GenerateBuffers();
            CreateTerrain();
        }

        private void SetVerticesCount(Bitmap heightBitmap, Bitmap normalMap)
        {
            if (heightBitmap != null)
            {
                _vertexCountX = heightBitmap.Width;
                _vertexCountY = heightBitmap.Height;
                if (normalMap != null)
                {
                    normalMap = new Bitmap(normalMap, heightBitmap.Size);
                }
            }
            else if (normalMap != null)
            {
                _vertexCountX = normalMap.Width;
                _vertexCountY = normalMap.Height;
            }
            else
            {
                _vertexCountX = 10;
                _vertexCountY = 10;
            }
        }

        private void CheckVertexCount()
        {
            if (_vertexCountX < 2)
            {
                throw new Exception("VertexCountX in Terrain can not be less than two");
            }

            if (_vertexCountY < 2)
            {
                throw new Exception("VertexCountY in Terrain can not be less than two");
            }
        }

        private Bitmap LoadBitmap(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return null;
            }

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

        private void CreateTerrain(Bitmap heightmap = null, Bitmap normalMap = null)
        {
            Vector3[] vertices = CreateVertices(heightmap);
            int[] indices = CreateIndices();
            Vector2[] textcoord = CreateTextureCoordinates(vertices);
            Vector3[] normals = normalMap != null ? CreateNormals(normalMap, vertices) : CreateNormals(vertices, indices);

            Bind();
            SetVerticesBuffer(vertices, renderer.PositionAttribute);
            SetIndicesBuffer(indices);
            SetTextureBuffer(textcoord, renderer.TextureCoordAttribute);
            SetNormalsBuffer(normals, renderer.NormalAttribute);

            UnBind();
        }
        private Vector3[] CreateVertices(Bitmap bmp)
        {

            Vector3[] vertices = new Vector3[_vertexCountX * _vertexCountY];
            float spacingX = 2 / (float)(_vertexCountX - 1);
            float spacingY = 2 / (float)(_vertexCountY - 1);
            int index = 0;
            for (int i = 0; i < _vertexCountY; i++)
            {
                for (int j = 0; j < _vertexCountX; j++)
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
            {
                return 0.0f;
            }

            Color pixel = bmp.GetPixel(x, y);
            float value = pixel.R * pixel.G * pixel.B;
            //value -= MAX_PIXEL_COLOR / 2f;
            value /= MAX_PIXEL_COLOR / 2f;
            return value;
        }

        private int[] CreateIndices()
        {
            _indicesCount = 2 * _vertexCountX + (2 * _vertexCountX + 2) * (_vertexCountY - 2);
            int[] indices = new int[_indicesCount];
            int index = 0;
            bool finish = false;
            for (int i = 0; !finish;)
            {
                for (int j = 0; j < _vertexCountX; j++)
                {
                    //fragment of strip (bottom->up)
                    int vertex = i * _vertexCountX + j;
                    indices[index++] = vertex;
                    indices[index++] = vertex + _vertexCountX;
                }

                i = i + 1;

                if (i < _vertexCountY - 1)//if not last row
                {
                    //repeat last vertex of this strip and first vertex of next row
                    indices[index++] = (i + 1) * _vertexCountX - 1;
                    indices[index++] = i * _vertexCountX;
                }
                else
                {
                    finish = true;
                }
            }

            return indices;
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

        private Vector3[] CreateNormals(Vector3[] vertices, int[] indices)
        {
            Vector3[] normals = new Vector3[vertices.Length];

            int Xmax = _vertexCountX;
            int YMax = _vertexCountY;
            for (int i = 0; i < normals.Length; i++)
            {
                GetDisorder(vertices, i, out float dhx, out float dhy);
                normals[i] = Vector3.UnitZ + Vector3.UnitX * dhx + Vector3.UnitY * dhy;
                normals[i] = normals[i].Normalized();
            }

            return normals;
        }

        private void GetDisorder(Vector3[] vertices, int index, out float dhx, out float dhy)
        {
            int nextX = index + 1;
            int nextY = index + _vertexCountX;

            if (nextX % _vertexCountX == 0)
            {
                nextX -= _vertexCountX;
            }

            if (nextY >= _vertexCountX * _vertexCountY)
            {
                nextY = 0;
            }

            dhx = vertices[nextX].Z - vertices[index].Z;
            dhy = vertices[nextY].Z - vertices[index].Z;
        }

        private Vector3[] CreateNormals(Bitmap normalMap, Vector3[] vertices)
        {
            Vector3[] normals = new Vector3[_vertexCountX * _vertexCountY];
            int index = 0;
            for (int i = 0; i < _vertexCountY; i++)
            {
                for (int j = 0; j < _vertexCountX; j++)
                {
                    Color pixel = normalMap.GetPixel(j, i);
                    float half = 256f / 2.0f;
                    Vector3 normal = new Vector3();

                    normal.X = (pixel.R - half) / half;
                    normal.Y = (pixel.G - half) / half;
                    normal.Z = (pixel.B - half) / half;
                    normal /= normal.Z;
                    GetDisorder(vertices, index, out float dhx, out float dhy);
                    normal = normal + new Vector3(1, 0, -normal.X) * dhx + new Vector3(0, 1, -normal.Y) * dhy;
                    normal.Normalize();
                    normals[index++] = normal;
                }
            }

            return normals;
        }

        protected override void GenerateBuffers()
        {
            GL.GenVertexArrays(1, out vertexArray);
            GL.GenBuffers(1, out vertexBuffer);
            GL.GenBuffers(1, out elementsBuffer);
            GL.GenBuffers(1, out textureBuffer);
            GL.GenBuffers(1, out normalsBuffer);
        }

        public override void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix, List<Light> lights, bool PhongLightningModel, bool PhongShading)
        {
            renderer.Use();
            renderer.SetModelMatrix(ModelMatrix);
            renderer.SetProjectionMatrix(projectionMatrix);
            renderer.SetViewMatrix(viewMatrix);
            renderer.SetLights(lights);
            renderer.SetPhongLightning(PhongLightningModel);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture.TextureId);
            renderer.SetAmbientColor(Vector3.One);
            renderer.SetDiffuseColor(Vector3.One);
            renderer.SetSpecularColor(Vector3.One);
            renderer.SetHasTexture(Texture != null);
            renderer.SetPhongShading(PhongShading);
            renderer.EnableVertexAttribArrays();
            GL.DrawElements(BeginMode.TriangleStrip, _indicesCount, DrawElementsType.UnsignedInt, 0);
            renderer.DisableVertexAttribArrays();
        }

        public override void Update(float deltatime)
        {
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(vertexArray);
            GL.DeleteBuffer(vertexBuffer);
            GL.DeleteBuffer(elementsBuffer);
            GL.DeleteBuffer(textureBuffer);
            GL.DeleteBuffer(normalsBuffer);
        }
    }
}
