﻿using Microsoft.AspNetCore.Http;

namespace Delta.Polling.FrontEnd.Logics.Common.Extensions;

public static class FormFileExtensions
{
    public static byte[] ToBytes(this IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();

        formFile.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }
}
