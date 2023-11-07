using Newtonsoft.Json.Converters;

namespace SuperdarkLists.DomainModel.Rest.ErrorModel;

[JsonObject]
public class Error
{
    public Error(ErrorCode errorCode, string? message = null)
    {
        ErrorCode = errorCode;
        Message = message;
    }
    
    [JsonProperty(PropertyName = "error")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ErrorCode ErrorCode { get; set; }
    
    public string? Message { get; set; }
}