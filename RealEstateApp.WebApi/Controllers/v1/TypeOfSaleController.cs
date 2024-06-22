using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.TypeOfSale;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.CreateTypeOfSale;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.DeleteTypeOfSale;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.UpdateTypeOfSale;
using RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetAllTypeOfSales;
using RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetTypeOfSaleById;
using RealEstateApp.WebApi.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Type of Sales Maintenance")]
    public class TypeOfSaleController : BaseApiController
    {
        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("List")]
        [SwaggerOperation(
            Summary = "List of Type of Sales",
            Description = "Gets all the types of sales created"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfSaleDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            //return Ok(await Mediator.Send(new GetAllTypeOfSalesQuery()));
            var response = await Mediator.Send(new GetAllTypeOfSalesQuery());
            if (response.Data == null || !response.Data.Any())
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("GetById")]
        [SwaggerOperation(
            Summary = "Type of sale by id",
            Description = "Gets a type of sale by id as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfSaleDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(int id)
        {
            //return Ok(await Mediator.Send(new GetTypeOfSaleByIdQuery { Id = id }));
            var response = await Mediator.Send(new GetTypeOfSaleByIdQuery { Id = id });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [SwaggerOperation(
            Summary = "Creates a type of sale",
            Description = "Gets the required parameters to create a new type of sale"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TypeOfSaleDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post(CreateTypeOfSaleCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //return Ok(await Mediator.Send(command));
            return CreatedAtAction(nameof(this.Post), await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        [SwaggerOperation(
            Summary = "Updates a type of sale",
            Description = "Gets the required parameters to update a type of sale"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfSaleDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(UpdateTypeOfSaleCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deletes a type of sale",
            Description = "Gets the required parameters to delete a type of sale"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTypeOfSaleByIdCommand { Id = id });
            return NoContent();
        }
    }
}
