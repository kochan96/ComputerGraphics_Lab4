namespace Grafika_lab_4.Configuration
{
    public static class Resources
    {
        static readonly string ResourcesFolder = "Resources";
        public static readonly string ShadersFolder = $"{ResourcesFolder}/Shaders";

        public static readonly string EntityVertexShader = $"{ShadersFolder}/entity.vert";
        public static readonly string EntityFragmentShader = $"{ShadersFolder}/entity.frag";
        public static readonly string SkyBoxVertexShader = $"{ShadersFolder}/skybox.vert";
        public static readonly string SkyBoxFragmentShader = $"{ShadersFolder}/skybox.frag";

        public static readonly string HeightMapsFolder = $"{ResourcesFolder}/HeightMaps";

        public static readonly string RiverMountainHeightMap = $"{HeightMapsFolder}/RiverMountains.png";
        public static readonly string MountainsHeightMap = $"{HeightMapsFolder}/Mountains.png";

        public static readonly string TexturesFolder = $"{ResourcesFolder}/Textures";

        public static readonly string GreenTexture = $"{TexturesFolder}/GreenTexture.png";
        public static readonly string RockTexture = $"{TexturesFolder}/RockTexture.png";
        public static readonly string TestTexture = $"{TexturesFolder}/TestTexture.png";

        public static readonly string[] SkyCubeMapTextures ={
            $"{TexturesFolder}/SkyBoxRight.png",
            $"{TexturesFolder}/SkyBoxLeft.png",
            $"{TexturesFolder}/SkyBoxTop.png",
            $"{TexturesFolder}/SkyBoxBottom.png",
            $"{TexturesFolder}/SkyBoxBack.png",
            $"{TexturesFolder}/SkyBoxFront.png",
        };

        public static readonly string TreeTexture = $"{TexturesFolder}/Tree.png";
        public static readonly string SphereTexture = $"{TexturesFolder}/SphereTexture.png";

        public static readonly string ModelsFolder = $"{ResourcesFolder}/Models";

        public static readonly string AircraftModel = $"{ModelsFolder}/Aircraft.obj";
        public static readonly string TreeModel = $"{ModelsFolder}/Tree.obj";

        public static readonly string MaterialsFolder = $"{ResourcesFolder}/Materials";
    }
}
