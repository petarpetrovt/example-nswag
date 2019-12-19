using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExampleNSwag.Controllers
{
	[Authorize]
	[Route("api/test")]
	public class TestController : ControllerBase
	{
		/// <summary>
		/// Performs a test action.
		/// </summary>
		/// <param name="cancellationToken">the cancellation token</param>
		/// <returns>the login result</returns>
		[AllowAnonymous]
		[HttpGet("{param}")]
		[ProducesResponseType(200, Type = typeof(string))]
		[ProducesResponseType(400, Type = typeof(string))]
		public IActionResult GetParam([FromRoute(Name = "param")] string param)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest($"Invalid model state.");
			}

			return Ok(param);
		}
	}
}
