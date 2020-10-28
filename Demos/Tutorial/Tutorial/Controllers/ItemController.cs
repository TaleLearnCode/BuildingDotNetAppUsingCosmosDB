using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;

namespace Tutorial.Controllers
{
	public class ItemController : Controller
	{

		private readonly ITodoService _todoService;

		public ItemController(ITodoService todoService)
		{
			_todoService = todoService;
		}

		[ActionName("Index")]
		public async Task<IActionResult> IndexAsync()
		{
			return View(await _todoService.GetItemsAsync("SELECT * FROM c"));
		}

		[ActionName("Create")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ActionName("Create")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync([Bind("Id,Name,Description,Completed")] Item item)
		{
			if (ModelState.IsValid)
			{
				item.Id = Guid.NewGuid().ToString();
				await _todoService.AddItemAsync(item);
				return RedirectToAction("Index");
			}
			return View(item);
		}

		[HttpPost]
		[ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed")] Item item)
		{
			if (ModelState.IsValid)
			{
				await _todoService.UpdateItemAsync(item.Id, item);
				return RedirectToAction("Index");
			}
			return View(item);
		}

		[ActionName("Edit")]
		public async Task<ActionResult> EditAsync(string id)
		{

			if (id == null) return BadRequest();

			Item item = await _todoService.GetItemAsync(id);
			if (item == null) return NotFound();

			return View(item);

		}

		[ActionName("Deleted")]
		public async Task<ActionResult> DeleteAsync(string id)
		{

			if (id == null) return BadRequest();

			Item item = await _todoService.GetItemAsync(id);
			if (item == null) return NotFound();

			return View(item);

		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmedAsync([Bind("id")] string id)
		{
			await _todoService.DeleteItemAsync(id);
			return RedirectToAction("Index");
		}

		[ActionName("Details")]
		public async Task<ActionResult> DetailsAsync(string id)
		{
			return View(await _todoService.GetItemAsync(id));
		}

	}

}