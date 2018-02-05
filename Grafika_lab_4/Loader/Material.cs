using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.Loader
{
    public struct Material
    {
        // Material Name
        public string Name;
        // Ambient 
        public Vector3 Ka;
        // Diffuse 
        public Vector3 Kd;
        // Specular 
        public Vector3 Ks;
        //Emissive
        public Vector3 Ke;
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
