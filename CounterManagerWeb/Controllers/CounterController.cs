using CounterManagerWeb.Api;
using Microsoft.AspNetCore.Mvc;

namespace CounterManagerWeb.Controllers {
    public class CounterController : Controller {

        private ICounterApi _counterApi;

        public CounterController(ICounterApi counterApi)
        {
            _counterApi = counterApi;
        }


        public async Task<IActionResult> Create(CounterModel request)
        {
            await _counterApi.CreateCounter(request);
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Update(long id, [Bind(Prefix = "counter")] CounterModel request)
        {
            await _counterApi.UpdateCounter(id, request);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(long id)
        {
            await _counterApi.DeleteCounter(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
