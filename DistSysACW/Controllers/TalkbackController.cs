using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    public class TalkBackController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context) : base(context) { }


        [ActionName("Hello")]
        public string Get()
        {
            #region TASK1
            // TODO: add api/talkback/hello response
            Ok();
            return "Hello World";
            #endregion
        }

        [ActionName("Sort")]
        public IActionResult Get([FromQuery]int[] integers)
        {
            #region TASK1
            // TODO: 
            // sort the integers into ascending order
            if (integers.Length == 0)
            {
                return Ok("[]");
            }
            string[] numbers = new string[integers.Length];
            for (int i = 0; i < integers.Length; i++)
            {
                numbers[i] = integers[i].ToString();
            }
            for (int i = 0; i < numbers.Length; i++)
            {
                int holder;
                if (!Int32.TryParse(numbers[i], out holder))
                {
                    //return bad request
                    return BadRequest();
                }
            }
            Array.Sort(integers);
            string view = "[";
            for (int i = 0; i < integers.Length; i++)
            {
                if (i == 0)
                {
                    view += integers[i];
                }
                else
                {
                    view += ", " + integers[i];
                }
            }
            view += "]";
            return Content(view);
            // send the integers back as the api/talkback/sort response
            #endregion
        }
    }
}
