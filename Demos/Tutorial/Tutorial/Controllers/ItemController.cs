﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaleLearnCode.Todo.Domain;
using TaleLearnCode.Todo.Services;
using Tutorial.Models;

namespace Tutorial.Controllers
{
	public class ItemController : Controller
	{

		// HACK: This should be replaced with an actual user identifier
		private readonly string _userId = "6F9BD7D9-6336-4EF6-97BA-99F9ABFBC15E";

		private readonly ITodoService _todoService;
		private readonly IMetadataService _metadataService;

		public ItemController(ITodoService todoService, IMetadataService metadataService)
		{
			_todoService = todoService;
			_metadataService = metadataService;
		}

		[ActionName("Index")]
		public async Task<IActionResult> IndexAsync()
		{
			return View(await _todoService.GetItemsAsync(_userId));
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
				item.UserId = _userId;
				await _todoService.AddItemAsync(item);
				return RedirectToAction("Index");
			}
			return View(item);
		}

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

		[ActionName("Delete")]
		public async Task<ActionResult> DeleteAsync(string id)
		{

			if (id == null) return BadRequest();

			Item item = await _todoService.GetItemAsync(id, _userId);
			if (item == null) return NotFound();

			return View(item);

		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DeleteConfirmedAsync([Bind("id")] string id)
		{
			await _todoService.DeleteItemAsync(id, _userId);
			return RedirectToAction("Index");
		}

		[ActionName("Details")]
		public async Task<ActionResult> DetailsAsync(string id)
		{
			return View(await _todoService.GetItemAsync(id, _userId));
		}

	}

}