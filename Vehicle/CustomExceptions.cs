namespace Vehicle
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(string message, Dictionary<string, string[]> errors = null)
            : base(message)
        {
            Errors = errors;
        }
    }
}
