namespace CleanArchitecture.Domain.Constants;

public static class MaxLengths
{
    public static class User
    {
        public const int Email = 320;
        public const int FirstName = 100;
        public const int LastName = 100;
        public const int Password = 128;
    }

    public static class Tenant
    {
        public const int Name = 255;
    }
}