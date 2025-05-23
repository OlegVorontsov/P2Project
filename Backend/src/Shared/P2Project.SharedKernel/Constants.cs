﻿
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
        
        public const int MIN_EXPERIENCE = 0;
        public const int MAX_EXPERIENCE = 90;

        public const int MIN_WEIGHT_HEIGHT = 0;
        public const int MAX_WEIGHT_HEIGHT = 1000;

        public const string BUCKET_NAME_PHOTOS = "photos";
        public const string BUCKET_NAME_FILES = "files";
        public const string BUCKET_NAME_AVATARS = "avatars";
        
        public const string IMAGE_CONTENT_TYPE = "image/jpeg";
        
        public static string ACCOUNTS_CONFIGURATIONS_FOLDER_PATH = "..\\Accounts\\P2Project.Accounts.Infrastructure\\JsonConfigurations\\";
        public static string ACCOUNTS_JSON_FILE_NAME = "Accounts.json";
        public static string ROLES_JSON_FILE_NAME = "Roles.json";
        public static string PERMISSIONS_JSON_FILE_NAME = "Permissions.json";
        
        public static double DELETE_EXPIRED_SOFT_DELETED_SERVICE_DELAY_HOURS = 24;
        public static double LIFETIME_AFTER_SOFT_DELETION = 30;

        public static class Discussions
        {
            public const string IS_CLOSED = "Discussion is closed";
            public const string USER_NOT_IN = "User isn't in discussion";
            public const string NOT_USERS_MESSAGE = "Can't manage with not yours message";
        }

        public static class CacheConstants
        {
            public const string USERS_PREFIX = "users_";
            public const string PETS_PREFIX = "pets_";
            public const string REFRESH_SESSIONS_PREFIX = "refresh_sessions_";
            public const int DEFAULT_EXPIRATION_MINUTES = 5;
        }
    }
}
