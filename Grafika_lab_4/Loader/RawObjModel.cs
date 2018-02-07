using OpenTK;
using System.Collections.Generic;

namespace Grafika_lab_4.Loader
{
    public class RawObjModel
    {
        public List<Vector3> Vertices = new List<Vector3>();
        public List<Vector3> Normals=new List<Vector3>();
        public List<Vector2> TextureCoordinates=new List<Vector2>();

        public List<Mesh> Meshes=new List<Mesh>();

        
    }
}
