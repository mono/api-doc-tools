namespace mdoc.Test.NullableReferenceTypes.OperatorOverloading
{
    public class Student
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public static Student operator +(Student s1, Student s2)
        {
            return default;
        }

        public static Student? operator -(Student? s1, Student? s2)
        {
            return default;
        }

        public static Student operator *(Student s1, Student? s2)
        {
            return default;
        }
        public static Student operator /(Student? s1, Student s2)
        {
            return default;
        }

        public static implicit operator ExamScore(Student? s)
        {
            return default;
        }

        public static explicit operator Student?(ExamScore? s)
        {
            return default;
        }
    }

    public struct ExamScore
    {
        public int ClassId { get; set; }

        public int Score { get; set; }

        public static ExamScore operator +(ExamScore s1, ExamScore s2)
        {
            return default;
        }

        public static ExamScore? operator -(ExamScore? s1, ExamScore? s2)
        {
            return default;
        }

        public static ExamScore operator *(ExamScore s1, ExamScore? s2)
        {
            return default;
        }

        public static ExamScore operator /(ExamScore? s1, ExamScore s2)
        {
            return default;
        }

        public static implicit operator ExamScore(Student? s)
        {
            return default;
        }

        public static explicit operator Student?(ExamScore? s)
        {
            return default;
        }
    }
}
