using OpenTK;

namespace Grafika_lab_4.Loader
{
    public class Material
    {
        // Material Name
        public string Name;
        // Ambient 
        public Vector3 Ka=Vector3.One;
        // Diffuse 
        public Vector3 Kd = Vector3.One;
        // Specular 
        public Vector3 Ks = Vector3.One;
        //Emissive
        public Vector3 Ke = Vector3.One;
        // Specular Exponent
        public float Ns;
        // Optical Density
        public float Ni;
        // Dissolve
        public float d;
        // Illumination
        public int illum;
        // Ambient Texture Map
        public string map_Ka;
        // Diffuse Texture Map
        public string map_Kd;
        // Specular Texture Map
        public string map_Ks;
        // Emissive Texture Map
        public string map_Ke;
        // Specular Hightlight Map
        public string map_Ns;
        // Alpha Texture Map
        public string map_d;
        // Bump Map
        public string map_bump;
    }
}
