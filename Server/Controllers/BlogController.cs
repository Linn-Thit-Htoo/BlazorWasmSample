using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models.Entities;
using Server.Models.RequestModels;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            try
            {
                List<BlogDataModel> lst = await _appDbContext.Blogs
                    .AsNoTracking()
                    .OrderByDescending(x => x.BlogId)
                    .ToListAsync();

                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(long id)
        {
            try
            {
                BlogDataModel? item = await _appDbContext.Blogs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.BlogId == id);
                if (item is null)
                    return NotFound("No data found.");

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDataModel requestModel)
        {
            try
            {
                if (string.IsNullOrEmpty(requestModel.BlogTitle))
                    return BadRequest();
                else if (string.IsNullOrEmpty(requestModel.BlogAuthor))
                    return BadRequest();
                else if (string.IsNullOrEmpty(requestModel.BlogContent))
                    return BadRequest();

                await _appDbContext.Blogs.AddAsync(requestModel);
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(201, "Blog Created.") : BadRequest("Creating Fail.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog([FromBody] UpdateBlogRequestModel requestModel, long id)
        {
            try
            {
                if (string.IsNullOrEmpty(requestModel.BlogTitle))
                    return BadRequest();
                else if (string.IsNullOrEmpty(requestModel.BlogAuthor))
                    return BadRequest();
                else if (string.IsNullOrEmpty(requestModel.BlogContent))
                    return BadRequest();

                BlogDataModel? item = await _appDbContext.Blogs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.BlogId == id);
                if (item is null)
                    return NotFound("No data found.");

                item.BlogTitle = requestModel.BlogTitle;
                item.BlogAuthor = requestModel.BlogAuthor;
                item.BlogContent = requestModel.BlogContent;
                _appDbContext.Entry(item).State = EntityState.Modified;
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(202, "Updating Successful!") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(long id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                BlogDataModel? item = await _appDbContext.Blogs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.BlogId == id);

                if (item is null)
                    return NotFound("No data found.");

                _appDbContext.Entry(item).State = EntityState.Modified;
                _appDbContext.Blogs.Remove(item);
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(202, "Blog Deleted.") : BadRequest("Deleting Fail.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
