using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;
using TaleLearnCode.Todo.Web.Models;

namespace TaleLearnCode.Todo.Web.Controllers
{

	/// <summary>
	/// Controller for working with to do items.
	/// </summary>
	/// <seealso cref="Controller" />
	public class ItemController : Controller
	{

		// HACK: This should be replaced with an actual user identifier
		private readonly string _userId = "6F9BD7D9-6336-4EF6-97BA-99F9ABFBC15E";

		private readonly ITodoService _todoService;
		private readonly IMetadataService _metadataService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemController"/> class.
		/// </summary>
		/// <param name="todoService">A reference to an initialized <see cref="TodoService"/>.</param>
		/// <param name="metadataService">A reference to an initialized <see cref="MetadataService"/>.</param>
		public ItemController(ITodoService todoService, IMetadataService metadataService)
		{
			_todoService = todoService;
			_metadataService = metadataService;
		}

		/// <summary>
		/// Displays the index page (which contains the list of to do items for the user).
		/// </summary>
		/// <returns>The view to be displayed.</returns>
		[ActionName("Index")]
		public async Task<IActionResult> IndexAsync()
		{
			return View(await _todoService.GetItemsAsync(_userId));
		}

		/// <summary>
		/// Displays the create to do item page.
		/// </summary>
		/// <returns>The view to be displayed.</returns>
		[ActionName("Create")]
		public IActionResult Create()
		{
			return View();
		}

		/// <summary>
		/// Saves the to do item and then redirects the user to the index page.
		/// </summary>
		/// <param name="item">The item to be saved.</param>
		/// <returns>The index view to be displayed.</returns>
		[HttpPost]
		[ActionName("Create")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CreateAsync([Bind("Id,Name,Description,Completed")] Item item)
		{
			if (ModelState.IsValid)
			{
				item.Id = Guid.NewGuid().ToString();
				item.UserId = _userId;
				await _todoService.AddItemAsync(item);
				return RedirectToAction("Index");
			}
			return View(item);
		}

		/// <summary>
		/// Saves the updates to the to do item and redirects to the index page.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index view to be displayed.</returns>
		[HttpPost]
		[ActionName("Edit")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EditAsync([Bind("Id,Name,Description,Completed,ItemStatus")] Item item)
		{
			if (ModelState.IsValid)
			{
				item.UserId = _userId;
				item.ItemStatus.Name =
					(await _metadataService.GetMetadataFromCacheAsync<ItemStatus>()).ToList()
					.Where(s => s.Id == item.ItemStatus.Id).First().Name;
				await _todoService.UpdateItemAsync(item);
				return RedirectToAction("Index");
			}
			return View(item);
		}

		/// <summary>
		/// Displays the edit to do item view.
		/// </summary>
		/// <param name="id">Identifier of the to do item to be edited.</param>
		/// <returns>The edit view to be displayed.</returns>
		[ActionName("Edit")]
		public async Task<ActionResult> EditAsync(string id)
		{

			if (id == null) return BadRequest();

			Item item = await _todoService.GetItemAsync(id, _userId);
			if (item == null) return NotFound();

			var viewModel = new ItemModel()
			{
				Item = item,
				ItemStatuses = await _metadataService.GetMetadataFromCacheAsync<ItemStatus>()
			};


			return View(viewModel);

		}

		/// <summary>
		/// Displays the delete to do item view.
		/// </summary>
		/// <param name="id">Identifier of the to do item to be deleted.</param>
		/// <returns>The delete view to be displayed.</returns>
		[ActionName("Delete")]
		public async Task<ActionResult> DeleteAsync(string id)
		{

			if (id == null) return BadRequest();

			Item item = await _todoService.GetItemAsync(id, _userId);
			if (item == null) return NotFound();

			return View(item);

		}

		/// <summary>
		/// Deletes the specified to do item.
		/// </summary>
		/// <param name="id">Identifier of the to do item to be deleted.</param>
		/// <returns>The index (list) view.</returns>
		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmedAsync([Bind("id")] string id)
		{
			await _todoService.DeleteItemAsync(id, _userId);
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Displays the details of the specified to do item.
		/// </summary>
		/// <param name="id">Identifier of the to do item to be displayed.</param>
		/// <returns>The details view.</returns>
		[ActionName("Details")]
		public async Task<ActionResult> DetailsAsync(string id)
		{
			return View(await _todoService.GetItemAsync(id, _userId));
		}

	}

}