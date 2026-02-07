public class ResultDto
{
     public bool IsSuccess { get; set; }
  
    public string? Error { get; set; }
       public static ResultDto Fail(string error)
    {
        return new ResultDto
        {
        Error = error,
        IsSuccess = false
        };
    }

    public static ResultDto Success()
    {
        return new ResultDto
        {
            IsSuccess = true
        };
    }
}