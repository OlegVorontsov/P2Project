using CSharpFunctionalExtensions;
using P2Project.SharedKernel;
using P2Project.SharedKernel.Errors;

namespace P2Project.Volunteers.Domain.ValueObjects.Volunteers
{
    public record VolunteerInfo
    {
        public const string DB_COLUMN_AGE = "age";
        public const string DB_COLUMN_GRADE = "grade";

        private VolunteerInfo(int age, int grade)
        {
            Age = age;
            Grade = grade;
        }
        public int Age { get; }
        public int Grade { get; }

        public static Result<VolunteerInfo, Error> Create(
            int age, int grade)
        {
            if (Constants.MIN_AGE >= age || age >= Constants.MAX_AGE)
                return Errors.General.ValueIsInvalid(nameof(age));
            
            if (Constants.MIN_GRADE >= grade || grade >= Constants.MAX_GRADE)
                return Errors.General.ValueIsInvalid(nameof(grade));

            var newVolunteerInfo = new VolunteerInfo(age, grade);

            return newVolunteerInfo;
        }
    }
}
