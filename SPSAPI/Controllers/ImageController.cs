using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPSAPI.Data;
using SPSModels.Models;

namespace SPSAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : ControllerBase
	{
		private readonly ApplicationDBContext _context;
		
		public ImageController(ApplicationDBContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("getById/{id}")]
		public IActionResult GetImage(string id)
		{
			FileStream stream = System.IO.File.Open($"Images/{id}", FileMode.Open);
			return File(stream, "image/jpeg");
		}

		[HttpPost]
		[Route("uploadImage/{id}")]
		[Authorize(Roles = UserTypes.Admin)]
		public async Task<IActionResult> UploadImage(int id, [FromForm] IFormFile image)
		{
			string fileName = Guid.NewGuid().ToString();
			string imagePath = $@"Images\{fileName}{Path.GetExtension(image.FileName)}";

			SparePart? sparePart = _context.SparePart.Find(id);
			if (sparePart == null)
			{
				return NotFound();
			}

			using (FileStream fileStream = new(imagePath, FileMode.Create))
			{
				image.CopyTo(fileStream);
			}

			System.IO.File.Delete(sparePart.Image);

			sparePart.Image = imagePath;

			_context.SparePart.Update(sparePart);
			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
