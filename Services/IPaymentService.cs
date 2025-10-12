using ComprovAI.Models;
using ComprovAI.Enums;

namespace ComprovAI.Services;

public interface IPaymentService
{
    Task<List<PaymentModeModel>> GetAllPaymentsAsync();
    
    Task<double> GetTotalPaymentsAsync();
    Task<PaymentModeModel> AddPaymentAsync(PaymentModeModel payment);
    Task DeletePaymentAsync(string id, PaymentType type);
}