using ComprovAI.Enums;

namespace ComprovAI.Models;
public class PaymentModeModel
{
    public int Id { get; set; }
    public double Value { get; set; }
    public PaymentType Type { get; set; }
    public CardBrand? CardBrand { get; set; }
    
}