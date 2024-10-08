using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Abstraction;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Can't create problem response for successful result");

        var problem = Results.Problem(statusCode: result.Error.statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))?.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {

                "errors", new[]
                {
                    result.Error.code,
                    result.Error.description

                }
            }
        };
        return new ObjectResult(problemDetails);
    }
}

// check this is chat gpt suggestion!!

//var problemDetails = new ProblemDetails
//{
//    Title = result.Error.description,
//    Status = result.Error.statusCode
//};
//problemDetails.Extensions.Add("errorCode", result.Error.code);
//return new ObjectResult(problemDetails) { StatusCode = result.Error.statusCode };
