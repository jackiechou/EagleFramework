namespace Eagle.Core.Settings
{
    public enum StatusCode
    {
        NetworkError = 0,
        RequestTypeViolation = 302,
        Unauthorized = 401,
        SessionExpired = 403,
        PageNotFound = 404,
        InternalServerError = 500,
        ServiceUnavailable = 503,
        TimeOut = 590,
        Unknown = 419,
    }
}
