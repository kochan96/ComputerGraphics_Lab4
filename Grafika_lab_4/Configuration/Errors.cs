namespace Grafika_lab_4.Configuration
{

    public enum Language
    {
        English
    }

    public enum ErrorType
    {
        FileMissingError,
        LoadModelError,
        ParseModelError,
        ParseMaterialError,
        LoadedEmptyModelError,
        LoadTextureError,
    }

    public static class Errors
    {
        public static Language Language { get; set; }
        public static string GetErrorMessage(ErrorType type)
        {
            switch (Language)
            {
                case Language.English:
                    return GetEnglishErrorMessage(type);

                default:
                    return GetEnglishErrorMessage(type);
            }
        }

        #region English
        private static string GetEnglishErrorMessage(ErrorType type)
        {
            switch(type)
            {
                case ErrorType.FileMissingError:
                    return "Could not find file: ";
                case ErrorType.LoadModelError:
                    return "Could not load model: ";
                case ErrorType.ParseModelError:
                    return "Could not parse model: ";
                case ErrorType.ParseMaterialError:
                    return "Could not parse materials: ";
                case ErrorType.LoadedEmptyModelError:
                    return "Loaded model is empty: ";
                case ErrorType.LoadTextureError:
                    return "Could not load texture: ";
                default:
                    return "Error: ";
            }
        }
        #endregion

    }




}
