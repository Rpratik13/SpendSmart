using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private SpendSmartDbContext _context;

    public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }

    public IActionResult Expenses()
    {
        List<Expense> expenses = _context.Expenses.ToList();

        decimal totalExpense = expenses.Sum(expense => expense.Value);

        ViewBag.Expenses = totalExpense;

        return View(expenses);
    }

    public IActionResult CreateEditExpenses(int? Id)
    {
        if (Id != null)
        {
            var dbExpense = _context.Expenses.SingleOrDefault(item => item.Id == Id);

            return View(dbExpense);
        }

        return View();
    }

    public IActionResult Delete(int Id)
    {
        var dbExpense = _context.Expenses.SingleOrDefault(item => item.Id == Id);

        if (dbExpense != null)
        {
            _context.Expenses.Remove(dbExpense);
            _context.SaveChanges();
        }

        return RedirectToAction("Expenses");
    }

    public IActionResult CreateEditExpenseForm(Expense expense)
    {
        Console.WriteLine(expense.Id);
        if (expense.Id == 0)
        {
            _context.Expenses.Add(expense);
        }
        else
        {
            _context.Expenses.Update(expense);
        }

        _context.SaveChanges();

        return RedirectToAction("Expenses");
    }
}
