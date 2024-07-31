using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Delta.Polling.FrontEnd.Infrastructure.BackEnd;

// TODO: Jika memungkinkan, buat class ini menjadi Singleton Service
public static class RestResponseExtensions
{
    public static ResponseResult<T> ToResponseResult<T>(this RestResponse restResponse)
    {
        var responseResult = new ResponseResult<T>();

        try
        {
            if (restResponse.IsSuccessful)
            {
                responseResult.Result = CreateResultResponse<T>(restResponse);
            }
            else
            {
                responseResult.Problem = CreateErrorResponse(restResponse);
            }
        }
        catch (Exception exception)
        {
            var detail = new StringBuilder();
            _ = detail.AppendLine($"Unhandled exception occured when retrieving response from the {nameof(BackEnd)} service.");
            _ = detail.AppendLine($"Response content from Back-End service: {restResponse.Content}.");
            _ = detail.AppendLine($"Exception type: {exception.GetType().FullName}.");
            _ = detail.AppendLine($"Exception message: {exception.Message}.");
            _ = detail.AppendLine($"Exception stack trace: {exception.StackTrace}.");

            responseResult.Problem = new ProblemDetails
            {
                Type = "Unhandled Exception",
                Title = exception.Message,
                Status = (int)restResponse.StatusCode,
                Detail = detail.ToString(),
            };
        }

        return responseResult;
    }

    private static T CreateResultResponse<T>(RestResponse restResponse)
    {
        if (typeof(T).IsSubclassOf(typeof(FileResponse)))
        {
            if (restResponse.ContentHeaders is null)
            {
                throw new NullException(nameof(restResponse.ContentHeaders), typeof(IReadOnlyCollection<HeaderParameter>));
            }

            var contentDispositionContentHeader = restResponse.ContentHeaders
                .FirstOrDefault(headerParameter => headerParameter.Name == "Content-Disposition");

            if (contentDispositionContentHeader is null)
            {
                throw new NullException(nameof(contentDispositionContentHeader), typeof(HeaderParameter));
            }

            var contentDisposition = new ContentDisposition(contentDispositionContentHeader.Value);

            if (string.IsNullOrWhiteSpace(contentDisposition.FileName))
            {
                throw new NullException(nameof(contentDisposition.FileName), typeof(string));
            }

            if (restResponse.RawBytes is null)
            {
                throw new NullException(nameof(restResponse.RawBytes), typeof(byte[]));
            }

            if (restResponse.ContentType is null)
            {
                throw new NullException(nameof(restResponse.ContentType), typeof(string));
            }

            var response = new
            {
                FileName = contentDisposition.FileName,
                Content = restResponse.RawBytes,
                ContentType = restResponse.ContentType
            };

            var serializedFileResponse = JsonSerializer.Serialize(response, JsonSerializerOptionsFor.Serialize);

            return JsonSerializer.Deserialize<T>(serializedFileResponse, JsonSerializerOptionsFor.Deserialize)
                ?? throw new JsonDeserializationFailedException(serializedFileResponse, typeof(T));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(restResponse.Content))
            {
                if (new NoContentResponse() is not T noContentResponse)
                {
                    throw new Exception($"Cannot cast {nameof(NoContentResponse)} into {typeof(T).FullName}.");
                }

                return noContentResponse;
            }
            else
            {
                return JsonSerializer.Deserialize<T>(restResponse.Content, JsonSerializerOptionsFor.Deserialize)
                    ?? throw new JsonDeserializationFailedException(restResponse.Content, typeof(T));
            }
        }
    }

    private static ProblemDetails CreateErrorResponse(RestResponse restResponse)
    {
        Console.WriteLine($"restResponse.StatusCode: {restResponse.StatusCode}");
        Console.WriteLine($"restResponse.ErrorException: {restResponse.ErrorException}");
        Console.WriteLine($"restResponse.ErrorMessage: {restResponse.ErrorMessage}");
        Console.WriteLine($"restResponse.Content: {restResponse.Content}");

        if (!string.IsNullOrWhiteSpace(restResponse.Content))
        {
            var deserializedErrorResponse = JsonSerializer.Deserialize<ProblemDetails>(restResponse.Content, JsonSerializerOptionsFor.Deserialize)
                 ?? throw new JsonDeserializationFailedException(restResponse.Content, typeof(ProblemDetails));

            Console.WriteLine($"ProblemDetails.Title: {deserializedErrorResponse.Title}");
            Console.WriteLine($"ProblemDetails.Type: {deserializedErrorResponse.Type}");
            Console.WriteLine($"ProblemDetails.Status: {deserializedErrorResponse.Status}");
            Console.WriteLine($"ProblemDetails.Detail: {deserializedErrorResponse.Detail}");
            Console.WriteLine($"ProblemDetails.Instance: {deserializedErrorResponse.Instance}");

            return deserializedErrorResponse;
        }

        return new ProblemDetails
        {
            Title = "Something Went Wrong",
            Type = "about:blank",
            Status = (int)restResponse.StatusCode,
            Detail = restResponse.ErrorException is not null ? restResponse.ErrorException.Message : "Unknown error detail."
        };
    }
}
