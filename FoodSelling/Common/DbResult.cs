namespace FoodSelling.Common
{
    public class DbResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DbResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public static DbResult SuccessResult(string message) => new DbResult(true, message);
        public static DbResult FailureResult(string message) => new DbResult(false, message);
    }
}
