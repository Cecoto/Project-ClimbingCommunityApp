namespace ClimbingCommunity.Common
{
    public class EntityValidationConstants
    {
        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 30;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 30;

            public const int AgeMinValue = 2;
            public const int AgeMaxValue = 30;

            public const int ImageUrlMaxLength = 2048;

            public const int PhoneNumberMinLength = 10;
            public const int PhoneNumberMaxLength = 20;

            public const int EmailMinLength = 10;
            public const int EmailMaxLength = 20;

            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 25;

            public const int UserRoleMinLength = 6;
            public const int UserRoleMaxLength = 25;


        }
        public static class Climber
        {
            public const int ClimbingExperienceMinValue = 0;
            public const int ClimbingExperienceMaxValue = 100;
        }
        public static class ClimberSpeciality
        {
            public const int ClimberSpecialityNameMinLength = 3;
            public const int ClimberSpecialityNameMaxLength = 30;
        }

        public static class ClimbingTrip
        {
            public const int TitleMinLength = 5;
            public const int TitleMaxLength = 40;

            public const int DestinationMinLength = 5;
            public const int DestinationMaxLength = 40;

            public const int DurationMinValue = 1;
            public const int DurationMaxValue = 30;

        }
        public static class Coach
        {
            public const int CoachingExperienceMinValue = 0;
            public const int CoachingExperienceMaxValue = 50;

        }
        public static class Level
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 25;
        }
        public static class Target
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }
        public static class Training
        {
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 30;

            public const int LocationMinLength = 3;
            public const int LocationMaxLength = 50;

            public const int DurationMinValue = 1;
            public const int DurationMaxValue = 6;

            public const string PriceMinValue = "10";
            public const string PriceMaxValue = "50";

        }

        public static class TripType
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 30;
        }
    }
}
