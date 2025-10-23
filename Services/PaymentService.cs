using ComprovAI.Data;
using ComprovAI.Models;
using ComprovAI.Enums;
using System.Linq;
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

    public async Task<IEnumerable<PaymentTotalDto>> GetPaymentsTotalByTypeAsync()
    {
        var payments = (await GetAllPaymentsAsync()).ToList();

        var grouped = payments
            .GroupBy(p => new
            {
                p.Type,
                Brand = p.Type == PaymentType.Pix ? "Pix" : (p.CardBrand.ToString() ?? "Unknown") 
            })
            .Select(g => new PaymentTotalDto
            {
                Type = g.Key.Type,
                Brand = g.Key.Brand,
                Total = g.Sum(x => x.Value)
            })
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Brand)
            .ToList();

        return grouped;
    }

    public async Task AddPaymentAsync(PaymentModeModel payment) 
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(string id, PaymentType type)
    {
        var paymentToDelete = await _context.Payments.FindAsync(id, type);

        if (paymentToDelete != null)
        {
            _context.Payments.Remove(paymentToDelete);
            await _context.SaveChangesAsync();
        }
    }
}