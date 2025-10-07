using ComprovAI.Models;

namespace ComprovAI.Services;

public interface IPaymentService
{
    Task<List<PaymentModeModel>> GetAllPaymentsAsync();
    
    Task<double> GetTotalPaymentsAsync();
    Task AddPaymentAsync(PaymentModeModel payment);
    Task DeletePaymentAsync(string id, PaymentModeModel type);
}