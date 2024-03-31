using Microsoft.AspNetCore.Mvc;

namespace SimpleMoviesService.Utils;

public static class ProblemDetailsBuilder
{
    public static ProblemDetails BadRequest(params string[] messages)
    {
        return new ProblemDetails
        {
            Status = 400,
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            Title = "Bad Request",
            Detail = string.Join(" && ", messages)
        };
    }
}
