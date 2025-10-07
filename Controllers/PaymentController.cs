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

        await _paymentService.AddPaymentAsync(payment);
        return Ok();
    }
}