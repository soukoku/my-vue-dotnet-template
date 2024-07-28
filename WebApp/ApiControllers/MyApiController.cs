using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MyApiController : ControllerBase
    {
        /// <summary>
        /// Randomly returns a 200 OK or a 500 Internal Server Error response.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(MyApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public ActionResult RandomlyGetError()
        {
            var value = new System.Random().Next(2);
            if (value == 0)
            {
                return Ok(new MyApiResponse { Value = value });
            }
            else
            {
                return StatusCode(500, new ApiResponse { Error = "An error occurred" });
            }
        }
    }

    public class ApiResponse
    {
        public string? Error { get; set; }
    }

    public class MyApiResponse : ApiResponse
    {
        public int Value { get; set; }
    }
}
