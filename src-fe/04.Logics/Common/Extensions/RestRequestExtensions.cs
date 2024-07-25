using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Delta.Polling.FrontEnd.Logics.Common.Extensions;

public static class RestRequestExtensions
{
    private const string UniversalDateTimeFormat = "yyyy/MM/dd HH:mm:ss";

    public static void AddParameters<T>(this RestRequest restRequest, T request) where T : notnull
    {
        foreach (var property in typeof(T).GetProperties())
        {
            var value = property.GetValue(request);

            if (value is null)
            {
                continue;
            }

            if (value is IFormFile formFile)
            {
                _ = restRequest.AddFile(property.Name, formFile.ToBytes(), formFile.FileName, contentType: formFile.ContentType);
            }
            else
            {
                var specialValueAttributes = property.GetCustomAttributes(typeof(SpecialValueAttribute), false);

                if (specialValueAttributes.Length != 0)
                {
                    var specialValueAttribute = (SpecialValueAttribute)specialValueAttributes.First();

                    if (specialValueAttribute.ValueType == SpecialValueType.Json)
                    {
                        _ = restRequest.AddParameter(property.Name, JsonSerializer.Serialize(value, JsonSerializerOptionsFor.Serialize));
                    }
                }
                else if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var index = 0;

                    foreach (var childItem in ((IEnumerable)value).OfType<object>())
                    {
                        if (childItem.GetType().IsValueType)
                        {
                            _ = restRequest.AddParameter(property.Name, childItem.ToString());
                        }
                        else
                        {
                            foreach (var childItemProperty in childItem.GetType().GetProperties())
                            {
                                var childItemValue = childItemProperty.GetValue(childItem);

                                if (childItemValue is not null)
                                {
                                    var name = $"{property.Name}[{index}].{childItemProperty.Name}";
                                    _ = restRequest.AddParameter(name, childItemValue.ToString());
                                }
                            }
                        }

                        index++;
                    }
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    var dateTime = (DateTime)value;

                    _ = restRequest.AddParameter(property.Name, dateTime.ToString(UniversalDateTimeFormat));
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    var nullableDateTime = (DateTime?)value;

                    _ = restRequest.AddParameter(property.Name, nullableDateTime.Value.ToString(UniversalDateTimeFormat));
                }
                else
                {
                    _ = restRequest.AddParameter(property.Name, value.ToString());
                }
            }
        }
    }
}
