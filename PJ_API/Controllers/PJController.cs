using Journal_BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Journal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : Controller
    {
        private static JournalProcess journal = new JournalProcess();

        [HttpGet]
        public IEnumerable<string> GetEntries()
        {
            return journal.GetEntries();
        }

        [HttpPost("add")]
        public IActionResult AddEntry([FromBody] string entry)
        {
            if (string.IsNullOrWhiteSpace(entry))
                return BadRequest("Entry cannot be empty.");

            journal.AddEntry(entry);
            return Ok("Entry added successfully.");
        }

        [HttpDelete("delete/{index}")]
        public IActionResult DeleteEntry(int index)
        {
            if (journal.DeleteEntry(index))
                return Ok("Entry deleted successfully.");
            else
                return NotFound("Invalid index.");
        }

        [HttpPatch("update/{index}")]
        public IActionResult UpdateEntry(int index, [FromBody] string newEntry)
        {
            if (journal.UpdateEntry(index, newEntry))
                return Ok("Entry updated successfully.");
            else
                return NotFound("Invalid index or empty entry.");
        }

        [HttpGet("search")]
        public IActionResult SearchEntry(string keyword)
        {
            if (journal.SearchEntry(keyword))
                return Ok("Keyword found in journal.");
            else
                return NotFound("Keyword not found.");
        }

        [HttpPost("validate")]
        public IActionResult ValidateAccount([FromBody] LoginRequest request)
        {
            bool isValid = journal.ValidateAccount(request.Username, request.Password);
            return Ok(isValid);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}