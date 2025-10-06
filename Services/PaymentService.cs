using ComprovAI.Data;
using ComprovAI.Models;
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
        try
        {
            double total = await _context.Payments.SumAsync(p => p.Value);
            if (total == 0)
                throw new Exception("Não há pagamentos.");
            return total;
        }
        catch (Exception ex)
        {
            // Aqui você pode logar o erro ou tratar conforme necessário
            throw new Exception($"Erro ao obter total de pagamentos: {ex.Message}");
        }
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
}