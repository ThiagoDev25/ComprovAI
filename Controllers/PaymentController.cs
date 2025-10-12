using ComprovAI.Enums;
using ComprovAI.Models;
using ComprovAI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ComprovAI.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<IActionResult> Index()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        double total = await _paymentService.GetTotalPaymentsAsync();
        ViewBag.TotalPayments = total;
        return View(payments);
    }

  //  public IActionResult Create()
  //  {
  //     return View();
  //  }
    
    // Post
  //  [HttpPost]
  //  [ValidateAntiForgeryToken]
  //  public async Task<IActionResult> Create(PaymentModeModel payment)
  //  {
  //      if (payment.Type != PaymentType.Pix)
   //     {
   //         ModelState.AddModelError("CardBrand",$"{payment.Type}: Escolha a bandeira do cartão");
   //     }
   //     if (ModelState.IsValid)
    //    {
    //        await _paymentService.AddPaymentAsync(payment);
    //      return RedirectToAction(nameof(Index)); // Redireciona para  a lista
     // }

    //  return View(payment);
   // }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPayment([FromBody] PaymentModeModel payment)
    {
        TryValidateModel(payment);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var savedPayment = await _paymentService.AddPaymentAsync(payment); 

        // double newTotal = await _paymentService.GetTotalPaymentsAsync(); // o total não deveria ser calculado aqui, para resol

        return Json(new { success = true, payment = savedPayment /* total = newTotal */ });
    }

    [HttpDelete("Payment/DeletePayment/{id}/{type}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePayment(string id, PaymentType type) // o header no front deve ficar assim: headers: { 'Content-Type': 'application/json' }
    {
        try
        {
            await _paymentService.DeletePaymentAsync(id, type);
            return Ok(new { success = true, message = "Pagamento excluído com sucesso." });
            
        }
        catch (Exception e)
        {
            // Log do erro
            Console.WriteLine($"Erro ao excluir pagamento: {e.Message}");
            return StatusCode(500, new { success = false, message = e.Message });
        }   
    }
}