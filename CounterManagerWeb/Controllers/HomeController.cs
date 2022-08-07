using CounterManagerWeb.Api;
using Microsoft.AspNetCore.Mvc;

namespace CounterManagerWeb.Controllers {
    public class HomeController : Controller {

        private ICounterApi _counterApi;

        public HomeController(ICounterApi counterApi)
        {
            _counterApi = counterApi;
        }

        public async Task<IActionResult> Index()
        {
            var counters = await _counterApi.GetAllCounters();
            return View(counters);
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }
    }
}
