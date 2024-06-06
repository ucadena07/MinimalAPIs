namespace MinimalAPIsMovies.Validation
{
    public static  class ValidationUtils
    {
        public static string NonEmptyMessage { get; set; } = "The field {PropertyName} is required.";
        public static string MaxLengthMessage { get; set; } = "The field {PropertyName} should be less then {MaxLength} characters.";
        public static string FirstLetterIsUppercaseMessage { get; set; } = "The field {PropertyName} should start with uppercase";
        public static string EmailMessage { get; set; } = "The field {PropertyName} is not a valid email address.";


        public static bool FirstLetterIsUppercase(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            var firstLetter = value[0].ToString();
            return firstLetter == firstLetter.ToUpper();
        }
    }
}
