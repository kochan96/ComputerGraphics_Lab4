using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafika_lab_4.Configuration
{
    public static class Resources
    {
        readonly static string ResourcesFolder = "Resources/";
        #region Shaders
        public readonly static string ShadersFolder = ResourcesFolder+ "Shaders/";

        public readonly static string AircraftVertexShader = ShadersFolder+"aircraft.vert";
        public readonly static string AircraftFragmentShader = ShadersFolder+"aircraft.frag";
        public readonly static string TerrainVertexShader =ShadersFolder + "terrain.vert";
        public readonly static string TerrainFragmentShader =  ShadersFolder + "terrain.frag";
        public readonly static string StaticVertexShader = ShadersFolder + "static.vert";
        public readonly static string StaticFragmentShader = ShadersFolder + "static.frag";
        #endregion

        #region HeightMaps
        public readonly static string HeightMapsFolder = ResourcesFolder+"HeightMaps/";

        public readonly static string RiverMountainHeightMap =  HeightMapsFolder + "RiverMountains.png";

        #endregion

        #region Textures
        public readonly static string TexturesFolder = ResourcesFolder+"Textures/";

        public readonly static string GreenTexture = TexturesFolder + "GreenTexture.png";
        public readonly static string RockTexture = TexturesFolder + "RockTexture.png";
        #endregion

        #region Models

        public readonly static string ModelsFolder = ResourcesFolder+"Models/";

        public readonly static string AircraftModel =ModelsFolder + "Aircraft.obj";

        #endregion

        #region Materials
        public readonly static string MaterialsFolder = ResourcesFolder + "Materials/";

        #endregion


    }
}
