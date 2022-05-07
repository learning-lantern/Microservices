using Newtonsoft.Json;

namespace LearningLantern.Common;

public static class HttpHelperMethods
{
    public static string ToJsonStringContent<T>(this T data) =>
        JsonConvert.SerializeObject(data);

    public static IEnumerable<KeyValuePair<string, string?>> GetQueryKeyValuePairs(this HttpRequestMessage request)
    {
        var list = new List<KeyValuePair<string, string?>>();

        if (request.RequestUri is null)
            return list;

        list.AddRange(request.RequestUri.Query.Split("&").Select(s => s.Split("="))
            .Select(tmp => new KeyValuePair<string, string?>(tmp[0], tmp[1])));

        return list;
    }

    public static string? GetQueryString(this HttpRequestMessage request, string key)
    {
        var queryStrings = request.GetQueryKeyValuePairs();


        var match = queryStrings.FirstOrDefault(
            kv => string.Compare(kv.Key, key, StringComparison.OrdinalIgnoreCase) == 0);

        return string.IsNullOrEmpty(match.Value) ? null : match.Value;
    }
}