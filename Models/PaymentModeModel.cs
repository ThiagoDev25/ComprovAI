using ComprovAI.Enums;

namespace ComprovAI.Models;
public class PaymentModeModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public double Value { get; set; }
    public PaymentType Type { get; set; }
    public CardBrand? CardBrand { get; set; }
    
}