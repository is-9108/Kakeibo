using System.Diagnostics;
using Kakeibo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Kakeibo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var homes = new List<Home>();
            int sumInc = 0;
            int sumExp = 0;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var command = new SqlCommand("SELECT " +
                    "Categorys.Name,SUM(Transactions.Amount),Categorys.isExpense " +
                    "FROM Transactions " +
                    "INNER JOIN Categorys on Transactions.CategoryId = Categorys.Id " +
                    "group by Categorys.Name,Categorys.isExpense", connection);
                
                using (var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        var home = new Home()
                        {
                            Title = reader.GetString(0), // ÉJÉeÉSÉäñº
                            Amount = reader.GetInt32(1), // çáåvã‡äz
                            IsExpense = reader.GetBoolean(2) // éxèoÇ©é˚ì¸Ç©
                        };
                        if (home.IsExpense)
                            sumExp += home.Amount;
                        else
                            sumInc += home.Amount;
                        homes.Add(home);
                    }
                }
            }
            ViewData["sumInc"] = sumInc;
            ViewData["sumExp"] = sumExp;
            ViewData["sum"] = sumInc - sumExp;
            return View(homes);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
