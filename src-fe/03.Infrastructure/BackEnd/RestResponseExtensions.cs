using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Delta.Polling.FrontEnd.Services.BackEnd;
using Delta.Polling.Both.Common.Statics;

namespace Delta.Polling.FrontEnd.Infrastructure.BackEnd;

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
                responseResult.Error = CreateErrorResponse(restResponse);
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

            responseResult.Error = new ErrorResponse
            {
                Type = "Unhandled Exception",
                Title = exception.Message,
                Status = restResponse.StatusCode,
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
                throw new Exception("Response headers is null");
            }

            var contentDispositionContentHeader = restResponse.ContentHeaders
                .FirstOrDefault(x =>
                {
                    return x.Name == "Content-Disposition";
                })
                ?? throw new Exception("Content-Disposition Content Header is null");

            if (contentDispositionContentHeader.Value is not string contentDispositionValue)
            {
                throw new Exception("Content-Disposition Value is null");
            }

            var contentDisposition = new ContentDisposition(contentDispositionValue);
            var fileName = contentDisposition.FileName
                ?? throw new NullException(nameof(contentDisposition.FileName), typeof(string));

            if (restResponse.RawBytes is null)
            {
                throw new NullException(nameof(restResponse.RawBytes), typeof(byte[]));
            }

            if (restResponse.ContentType is null)
            {
                throw new NullException(nameof(restResponse.ContentType), typeof(string));
            }

            var response = new FileResponse
            {
                FileName = fileName,
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

    private static ErrorResponse CreateErrorResponse(RestResponse restResponse)
    {
        var errorResponse = new ErrorResponse
        {
            Title = "Something Went Wrong",
            Type = "about:blank",
            Status = restResponse.StatusCode,
            Detail = restResponse.ErrorException is not null ? restResponse.ErrorException.Message : "Unknown error detail."
        };

        Console.WriteLine($"restResponse.StatusCode: {restResponse.StatusCode}");
        Console.WriteLine($"restResponse.ErrorException: {restResponse.ErrorException}");
        Console.WriteLine($"restResponse.ErrorMessage: {restResponse.ErrorMessage}");
        Console.WriteLine($"restResponse.Content: {restResponse.Content}");

        if (!string.IsNullOrWhiteSpace(restResponse.Content))
        {
            var deserializedErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(restResponse.Content, JsonSerializerOptionsFor.Deserialize);

            if (deserializedErrorResponse is not null)
            {
                return deserializedErrorResponse;
            }

            errorResponse.Title = "Failed to deserialize JSON content.";
            errorResponse.Detail = $"Unable to deserialize {restResponse.Content} into {nameof(ErrorResponse)} object.";
        }

        if (restResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            if (restResponse.Headers is not null)
            {
                var headerIsTokenExpired = restResponse.Headers.FirstOrDefault(x =>
                {
                    return x.Name == "IsTokenExpired";
                });

                if (headerIsTokenExpired is not null)
                {
                    if (headerIsTokenExpired.Value is not null)
                    {
                        if (headerIsTokenExpired.Value.ToString() == true.ToString())
                        {
                            errorResponse.Title = "Expired Access Token.";
                            errorResponse.Type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                            errorResponse.Detail = "The Access Token is expired.";
                        }
                    }
                }
            }
        }

        return errorResponse;
    }
}
