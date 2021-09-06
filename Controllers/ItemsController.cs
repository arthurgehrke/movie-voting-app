using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Catalog.Entities;
using Catalog.Repositories;
using Catalog.Dtos;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository; 

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

		[HttpGet]
		public IEnumerable<ItemDto> GetItems()
        {
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

		[HttpGet("{id}")]
		public ActionResult<ItemDto> GetItem(Guid id)
        {
			var item = repository.GetItem(id);

			if(item is null)
            {
				return NotFound();
            }

			return item.AsDto();
        }

		[HttpPost]
		public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
		{
			Item item = new()
			{
				Id = Guid.NewGuid(),
				Name = itemDto.Name,
				Price = itemDto.Price,
				CreatedAt = DateTimeOffset.UtcNow
			};

			repository.CreateItem(item);

			return CreatedAtAction(nameof(GetItem), new { id = item.Id}, item.AsDto());
		}
    }
}
