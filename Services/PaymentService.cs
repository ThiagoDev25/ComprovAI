using ComprovAI.Data;
using ComprovAI.Models;
using ComprovAI.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ComprovAI.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<double> GetTotalPaymentsAsync()
    {

        var payments = await _context.Payments.ToListAsync();

        if (payments.Count == 0)
        {
            return 0.0;
        }

        double total = 0.0; 
        foreach (var payment in payments)
        {
            total += payment.Value;
        };

        return total;
    }
    
    public async Task<List<PaymentModeModel>> GetAllPaymentsAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task AddPaymentAsync(PaymentModeModel payment) 
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(string id, PaymentModeModel type)
    {
        var paymentToDelete = await _context.Payments
            .WithPartitionKey(type.ToString()) // Especifica a chave de partição
            .FirstOrDefaultAsync(p => p.Id == id); // Busca pelo Id

        if (paymentToDelete != null)
        {
            _context.Payments.Remove(paymentToDelete);
            await _context.SaveChangesAsync();
        }
    }
}