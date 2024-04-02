using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace hangfire_webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("Hello form hangfire webapi");
        //}

        [HttpPost]
        [Route("[action]")]

        public IActionResult Welcome()
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeEmail("Welcome to our app"));
            return Ok($"Job Id = {jobId}.Welcome email sent to the user!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Discount()
        {
            int timeInSeconds = 30;
            var jobId = BackgroundJob.Schedule(() => SendWelcomeEmail("Welcome to our app"), TimeSpan.FromSeconds(timeInSeconds));
            return Ok($"Job Id = {jobId}.Discount email will be sent in {timeInSeconds} seconds!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database updated."), Cron.Minutely);
            return Ok("Database check job initiated");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Confirm()
        {
            int timeInSeconds = 30;
            var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You ask to be unsubscribed"), TimeSpan.FromSeconds(timeInSeconds));
            BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You are unsubscribed"));
            return Ok("Confirmation job created");
        }
        public void SendWelcomeEmail(string email)
        {
            Console.WriteLine(email);
        }
    }
}
