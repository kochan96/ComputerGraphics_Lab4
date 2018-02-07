using Grafika_lab_4.Configuration;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Grafika_lab_4.Loader
{
    public static class RawObjLoader
    {

        public static RawObjModel LoadRawObj(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.FileMissingError) + filePath);
                return null;
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            List<Vector2> TexturesList = new List<Vector2>();
            List<Vector3> NormalsList = new List<Vector3>();
            List<int> IndicesList = new List<int>();
            List<string> MeshMatNamesList = new List<string>();
            List<Material> MaterialsList = new List<Material>();

            RawObjModel obj = new RawObjModel();

            bool listening = false;
            string meshname = "";
            Mesh tmpMesh;

            string line;
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] currentLine = line.Split(' ');

                    // Generate a Mesh Object or Prepare for an object to be created
                    if (line.StartsWith("o ") || line.StartsWith("g ") || currentLine[0] == "g")
                    {
                        if (!listening)
                        {
                            listening = true;
                            if (currentLine.Length == 1)
                            {
                                meshname = "unnamed";
                            }
                            else
                            {
                                meshname = currentLine[1];
                            }
                        }
                        else
                        {
                            // Generate the mesh to put into the array
                            if (IndicesList.Count > 0)
                            {
                                tmpMesh = new Mesh
                                {
                                    Indices = IndicesList,
                                    MeshName = meshname
                                };

                                IndicesList = new List<int>();
                                obj.Meshes.Add(tmpMesh);

                                if (currentLine.Length > 1)
                                    meshname = currentLine[1];
                                else
                                    meshname = "none";
                            }
                        }
                    }
                    // Generate a Vertex Position
                    else if (line.StartsWith("v "))
                    {
                        Vector3 position;
                        position.X = float.Parse(currentLine[1]);
                        position.Y = float.Parse(currentLine[2]);
                        position.Z = float.Parse(currentLine[3]);
                        obj.Vertices.Add(position);

                        //add temporary normals and texture
                        obj.Normals.Add(Vector3.Zero);
                        obj.TextureCoordinates.Add(Vector2.Zero);
                    }
                    // Generate a Vertex Texture Coordinate
                    else if (line.StartsWith("vt "))
                    {
                        Vector2 texture;
                        texture.X = float.Parse(currentLine[1]);
                        texture.Y = float.Parse(currentLine[2]);
                        TexturesList.Add(texture);
                    }
                    // Generate a Vertex Normal;
                    else if (line.StartsWith("vn "))
                    {
                        Vector3 normal;
                        normal.X = float.Parse(currentLine[1]);
                        normal.Y = float.Parse(currentLine[2]);
                        normal.Z = float.Parse(currentLine[3]);
                        NormalsList.Add(normal);
                    }
                    // Generate a Face (vertices & indices)
                    else if (line.StartsWith("f "))
                    {
                        string[] vertex1 = currentLine[1].Split('/');
                        string[] vertex2 = currentLine[2].Split('/');
                        string[] vertex3 = currentLine[3].Split('/');
                        ProcessVertex(vertex1, TexturesList, NormalsList, IndicesList, obj);
                        ProcessVertex(vertex2, TexturesList, NormalsList, IndicesList, obj);
                        ProcessVertex(vertex3, TexturesList, NormalsList, IndicesList, obj);

                    }
                    else if (line.StartsWith("usemtl "))
                    {
                        MeshMatNamesList.Add(currentLine[1]);

                        // Create new Mesh, if Material changes within a group
                        if (IndicesList.Count > 0)
                        {
                            // Create Mesh
                            tmpMesh = new Mesh
                            {
                                MeshName = meshname,
                                Indices = IndicesList,
                            };

                            int i = 2;
                            while (true)
                            {
                                tmpMesh.MeshName = meshname + "_" + i.ToString();
                                ++i;

                                foreach (var m in obj.Meshes)
                                    if (m.MeshName == tmpMesh.MeshName)
                                        continue;

                                break;
                            }

                            // Insert Mesh
                            obj.Meshes.Add(tmpMesh);

                            // Cleanup
                            IndicesList = new List<int>();
                        }
                    }
                    else if (line.StartsWith("mtllib "))
                    {
                        MaterialsList = LoadMaterials(currentLine[1]);
                    }
                }

                //Deal with last mesh
                if (IndicesList.Count > 0)
                {
                    tmpMesh = new Mesh
                    {
                        Indices = IndicesList,
                        MeshName = meshname
                    };
                    obj.Meshes.Add(tmpMesh);
                }

                sr.Close();

                if (MaterialsList != null)
                {
                    for (int i = 0; i < MeshMatNamesList.Count; i++)
                    {
                        string matname = MeshMatNamesList[i];
                        for (int j = 0; j < MaterialsList.Count; j++)
                        {
                            if (MaterialsList[j].Name == matname)
                            {
                                Mesh mesh = obj.Meshes[i];
                                mesh.MeshMaterial = MaterialsList[j];
                                obj.Meshes[i] = mesh;

                                break;
                            }
                        }
                    }
                }

                return obj;

            }
            catch (Exception ex)
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.LoadModelError) + ex.Message);
                return null;
            }

        }

        private static void ProcessVertex(string[] vertexData, List<Vector2> texturesList, List<Vector3> normalsList, List<int> indicesList, RawObjModel model)
        {
            int vertex = int.Parse(vertexData[0]) - 1;
            int texture = int.Parse(vertexData[1]) - 1;
            int normal = int.Parse(vertexData[2]) - 1;

            indicesList.Add(vertex);
            model.Normals[vertex] = normalsList[normal];
            model.TextureCoordinates[vertex] = texturesList[texture];
        }

        private static List<Material> LoadMaterials(string filePath)
        {
            filePath = Resources.MaterialsFolder + filePath;
            if (!File.Exists(filePath))
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.FileMissingError) + filePath);
                return null;
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            Material tmpMaterial = new Material();
            List<Material> Materials = new List<Material>();
            bool listening = false;
            string line;
            try
            {
                // Go through each line looking for material variables
                while ((line = sr.ReadLine()) != null)
                {
                    string[] currentLine = line.Split(' ');

                    // new material and material name
                    if (line.StartsWith("newmtl"))
                    {
                        if (!listening)
                        {
                            listening = true;
                            if (currentLine.Length > 1)
                            {
                                tmpMaterial.Name = currentLine[1];
                            }
                            else
                            {
                                tmpMaterial.Name = "none";
                            }
                        }
                        else
                        {
                            // Generate the material

                            // Add  Material
                            Materials.Add(tmpMaterial);

                            // Clear Loaded Material
                            tmpMaterial = new Material();

                            if (currentLine.Length > 1)
                            {
                                tmpMaterial.Name = currentLine[1];
                            }
                            else
                            {
                                tmpMaterial.Name = "none";
                            }
                        }
                    }
                    //Ambient
                    else if (line.StartsWith("Ka "))
                    {
                        tmpMaterial.Ka.X = float.Parse(currentLine[1]);
                        tmpMaterial.Ka.Y = float.Parse(currentLine[2]);
                        tmpMaterial.Ka.Z = float.Parse(currentLine[3]);
                    }
                    //Difuse
                    else if (line.StartsWith("Kd "))
                    {
                        tmpMaterial.Kd.X = float.Parse(currentLine[1]);
                        tmpMaterial.Kd.Y = float.Parse(currentLine[2]);
                        tmpMaterial.Kd.Z = float.Parse(currentLine[3]);
                    }
                    //Specular
                    else if (line.StartsWith("Ks "))
                    {
                        tmpMaterial.Ks.X = float.Parse(currentLine[1]);
                        tmpMaterial.Ks.Y = float.Parse(currentLine[2]);
                        tmpMaterial.Ks.Z = float.Parse(currentLine[3]);
                    }
                    //Emisive
                    else if (line.StartsWith("Ke "))
                    {
                        tmpMaterial.Ke.X = float.Parse(currentLine[1]);
                        tmpMaterial.Ke.Y = float.Parse(currentLine[2]);
                        tmpMaterial.Ke.Z = float.Parse(currentLine[3]);
                    }
                    // Specular Exponent
                    else if (line.StartsWith("Ns"))
                    {
                        tmpMaterial.Ns = float.Parse(currentLine[1]);
                    }
                    // Optical Density
                    else if (line.StartsWith("Ni"))
                    {
                        tmpMaterial.Ni = float.Parse(currentLine[1]);
                    }
                    // Dissolve
                    else if (line.StartsWith("d"))
                    {
                        tmpMaterial.d = float.Parse(currentLine[1]);
                    }
                    // Illumination
                    else if (line.StartsWith("illum"))
                    {
                        tmpMaterial.illum = int.Parse(currentLine[1]);
                    }
                    // Ambient Texture Map
                    else if (line.StartsWith("map_Ka"))
                    {
                        tmpMaterial.map_Ka = currentLine[1];
                    }
                    // Diffuse Texture Map
                    else if (line.StartsWith("map_Kd"))
                    {
                        tmpMaterial.map_Kd = currentLine[1];
                    }
                    // Specular Texture Map
                    else if (line.StartsWith("map_Ks"))
                    {
                        tmpMaterial.map_Ks = currentLine[1];
                    }
                    //Emisive Texture Map
                    else if (line.StartsWith("map_Ke"))
                    {
                        tmpMaterial.map_Ke = currentLine[1];
                    }
                    // Specular Hightlight Map
                    else if (line.StartsWith("map_Ns"))
                    {
                        tmpMaterial.map_Ns = currentLine[1];
                    }
                    // Alpha Texture Map
                    else if (line.StartsWith("map_d"))
                    {
                        tmpMaterial.map_d = currentLine[1];
                    }
                    // Bump Map
                    else if (line.StartsWith("map_Bump") || line.StartsWith("map_bump"))
                    {
                        tmpMaterial.map_bump = currentLine[1];
                    }
                }

                // Add last material
                Materials.Add(tmpMaterial);

                // Test to see if anything was loaded
                // If not return null
                if (Materials.Count == 0)
                    return null;
                // If so return list of materials
                else
                    return Materials;
            }
            //if error occured return null
            catch (Exception ex)
            {
                MessageBox.Show(Errors.GetErrorMessage(ErrorType.ParseMaterialError) + ex.Message);
                return null;
            }
        }
    }
}

