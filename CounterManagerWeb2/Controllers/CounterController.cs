using CounterManagerWeb2.Api;
using Microsoft.AspNetCore.Mvc;

namespace CounterManagerWeb2.Controllers {
    public class CounterController : Controller {

        private ICounterApi _counterApi;

        public CounterController(ICounterApi counterApi)
        {
            _counterApi = counterApi;
        }

        public async Task<IActionResult> Create([FromForm] CounterRequest request)
        {
            await _counterApi.CreateCounter(request);
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> Update(long id, [FromForm] CounterRequest request)
        {
            await _counterApi.UpdateCounter(id, request);
            return RedirectToPage("Index");
        }
    }
}
