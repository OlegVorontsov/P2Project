
namespace P2Project.SharedKernel
{
    public static class Constants
    {
        public const string DATABASE = "Database";
        
        public const int MAX_TINY_TEXT_LENGTH = 10;
        public const int MAX_SMALL_TEXT_LENGTH = 100;
        public const int MAX_MEDIUM_TEXT_LENGTH = 300;
        public const int MAX_BIG_TEXT_LENGTH = 1000;

        public const int MIN_AGE = 14;
        public const int MAX_AGE = 90;
        
        public const int MIN_GRADE = 0;
        public const int MAX_GRADE = 10;

        public const int MIN_WEIGHT_HEIGHT = 0;
        public const int MAX_WEIGHT_HEIGHT = 1000;

        public const string BUCKET_NAME_PHOTOS = "photos";
        public const string BUCKET_NAME_FILES = "files";
        
        public static string ACCOUNTS_CONFIGURATIONS_FOLDER_PATH = "..\\Accounts\\P2Project.Accounts.Infrastructure\\JsonConfigurations\\";
        public static string ROLES_JSON_FILE_NAME = "Roles.json";
        public static string PERMISSIONS_JSON_FILE_NAME = "Permissions.json";
    }
}
