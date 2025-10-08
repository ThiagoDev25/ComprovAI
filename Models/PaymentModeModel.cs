using ComprovAI.Enums;
using Newtonsoft.Json;

namespace ComprovAI.Models;
public class PaymentModeModel
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public double Value { get; set; }
    public PaymentType Type { get; set; }
    public CardBrand? CardBrand { get; set; }
    
    [JsonProperty("_etag")]
    public string? ETag { get; set; }
    
}