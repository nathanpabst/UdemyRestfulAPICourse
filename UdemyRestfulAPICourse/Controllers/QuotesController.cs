using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using UdemyRestfulAPICourse.Data;
using UdemyRestfulAPICourse.Models;

namespace UdemyRestfulAPICourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class QuotesController : ControllerBase
    {
        private QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }

        // the AllowAnonymous attribute will allow users to view all quotes
        // GET: api/Quotes
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [AllowAnonymous]
        public IActionResult Get(string sort)
        {
            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = _quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = _quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = _quotesDbContext.Quotes;
                    break;
            }
            return Ok(quotes);
        }

        [HttpGet("[action]")]
        public IActionResult PagingQuote(int? pageNumber, int? pageSize)
        {
            var quotes = _quotesDbContext.Quotes;
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            return Ok(quotes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        // search quote list by type && return a list of matches via LINQ query. Custom functions must include the attribute routing. 
        [HttpGet]
        [Route("[action]")]
        public IActionResult SearchQuotes(string type)
        {
            var results = _quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(results);
        }

        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var quote = _quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("Nice try, but this record does not exist. Go fish.");
            }
            return Ok(quote);
        }

        // see User Claims section 10:48. The following code allows the user to return a list of quotes that they created. 
        // Authorization not currently working within Postman
        // GET: api/Quotes/5 || GET Quotes by User
        
        //[HttpGet]
        //[Route("[action]")]
        //public IActionResult MyQuotes(int id)
        //{
        //    string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        //    var quotes = _quotesDbContext.Quotes.Where(q => q.UserId==userId);
        //    if (quotes == null)
        //    {
        //        return NotFound("Nice try, but you haven't added any quotes.");
        //    }
        //    return Ok(quotes);
        //}

        // POST: api/Quotes
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            // see User Claims section 10:48. The following code adds a layer of protection 
            // and ensures that only the author of the Quote has the ability to POST, PUT, and Delete the record. 
            // Authorization not currently working within Postman

            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //quote.UserId = userId;
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            // see User Claims section 10:48. Authorization not working within Postman
            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var entity = _quotesDbContext.Quotes.Find(id);
            if (entity == null)
            {
                return NotFound("Hmmm, I can't find this record. Please try again.");
            }
            // see User Claims section 10:48. Authorization not working within Postman
            //if (userId!=entity.UserId)
            //{
            //    return BadRequest("Sorry, no can has updatings skill.");
            //}
            else
            {
                entity.Title = quote.Title;
                entity.Author = quote.Author;
                entity.Description = quote.Description;
                entity.Type = quote.Type;
                entity.CreatedAt = quote.CreatedAt;
                _quotesDbContext.SaveChanges();
                return Ok("Record updated successfully.");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // see User Claims section 10:48. Authorization not working within Postman
            //string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var quote = _quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound("You cannot delete that which has not been created young grasshopper.");
            }
            // see User Claims section 10:48. Authorization not working within Postman
            //if (userId!=entity.UserId)
            //{
            //    return BadRequest("Sorry, no can has the required skills to do deletings.");
            //}
            else
            {
                _quotesDbContext.Quotes.Remove(quote);
                _quotesDbContext.SaveChanges();
                return Ok("She's gone. No turning back now mofo!");
            }
        }
    }
}
