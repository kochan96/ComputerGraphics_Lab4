namespace Grafika_lab_4.Configuration
{
    public static class Resources
    {
        readonly static string ResourcesFolder = "Resources/";
        #region Shaders
        public readonly static string ShadersFolder = ResourcesFolder + "Shaders/";

        public readonly static string AircraftVertexShader = ShadersFolder + "entity.vert";
        public readonly static string AircraftFragmentShader = ShadersFolder + "entity.frag";
        public readonly static string TerrainVertexShader = ShadersFolder + "terrain.vert";
        public readonly static string TerrainFragmentShader = ShadersFolder + "terrain.frag";
        public readonly static string StaticVertexShader = ShadersFolder + "sphere.vert";
        public readonly static string StaticFragmentShader = ShadersFolder + "sphere.frag";
        public readonly static string SkyBoxVertexShader = ShadersFolder + "skybox.vert";
        public readonly static string SkyBoxFragmentShader = ShadersFolder + "skybox.frag";
        #endregion

        #region HeightMaps
        public readonly static string HeightMapsFolder = ResourcesFolder + "HeightMaps/";

        public readonly static string RiverMountainHeightMap = HeightMapsFolder + "RiverMountains.png";
        public readonly static string MountainsHeightMap = HeightMapsFolder + "Mountains.png";

        #endregion

        #region Textures
        public readonly static string TexturesFolder = ResourcesFolder + "Textures/";

        public readonly static string GreenTexture = TexturesFolder + "GreenTexture.png";
        public readonly static string RockTexture = TexturesFolder + "RockTexture.png";
        public readonly static string TestTexture = TexturesFolder + "TestTexture.png";

        public readonly static string[] SkyCubeMapTextures ={
            TexturesFolder+"SkyBoxRight.png",
            TexturesFolder+"SkyBoxLeft.png",
            TexturesFolder+"SkyBoxTop.png",
            TexturesFolder+"SkyBoxBottom.png",
            TexturesFolder+"SkyBoxBack.png",
            TexturesFolder+"SkyBoxFront.png",
        };

        public readonly static string TreeTexture = TexturesFolder + "Tree.png";
        public static readonly string SphereTexture = TexturesFolder + "SphereTexture.png";
        #endregion

        #region Models

        public readonly static string ModelsFolder = ResourcesFolder + "Models/";

        public readonly static string AircraftModel = ModelsFolder + "Aircraft.obj";

        public readonly static string TreeModel = ModelsFolder + "Tree.obj";
        #endregion

        #region Materials
        public readonly static string MaterialsFolder = ResourcesFolder + "Materials/";


        #endregion


    }
}
