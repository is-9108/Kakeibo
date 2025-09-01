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
            var monthly_Reports = new List<Monthly_report>();
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

                var command2 = new SqlCommand("SELECT * FROM Monthly_report", connection);

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
                using (var reader = command2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var monthly_Report = new Monthly_report()
                        {
                            Year = reader.GetInt32(0),
                            Month = reader.GetInt32(1),
                            TotalIncome = reader.GetInt32(2),
                            TotalExpense = reader.GetInt32(3),
                            Balance = reader.GetInt32(4)
                        };
                        monthly_Reports.Add(monthly_Report);
                    }
                }
            }
            ViewData["sumInc"] = sumInc;
            ViewData["sumExp"] = sumExp;
            ViewData["sum"] = sumInc - sumExp;
            var homeView = new HomeView()
            {
                Homes = homes,
                Monthly_Reports = monthly_Reports
            };

            return View(homeView);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
