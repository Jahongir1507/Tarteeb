//=================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free to use to bring order in your workplace
//=================================

using Microsoft.AspNetCore.Mvc;
using Tarteeb.Api.Brokers.Storages;

namespace Tarteeb.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        IStorageBroker broker;

        public HomeController(IStorageBroker broker)
        {
            this.broker = broker;
        }

        [HttpGet]
        public ActionResult<string> GetHomeMessage() => "Tarteeb is running...";

        [HttpGet("tickets")]
        public IActionResult GetAll()
        {
          return  Ok(broker.SelectAllTickets());
        }
            
            
    }
}
