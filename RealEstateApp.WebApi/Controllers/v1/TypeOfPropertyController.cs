using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.TypeOfProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeOfProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeOfProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeOfProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetAllTypeOfProperties;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetTypeOfPropertyById;
using RealEstateApp.WebApi.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Type of Properties Maintenance")]
    public class TypeOfPropertyController : BaseApiController
    {
        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("List")]
        [SwaggerOperation(
            Summary = "List of Type of Properties",
            Description = "Gets all the types of properties created"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfPropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            //return Ok(await Mediator.Send(new GetAllTypeOfPropertiesQuery()));
            var response = await Mediator.Send(new GetAllTypeOfPropertiesQuery());
            if (response.Data == null || !response.Data.Any())
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("GetById")]
        [SwaggerOperation(
            Summary = "Type of property by id",
            Description = "Gets a type of property by id as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfPropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(int id)
        {
            //return Ok(await Mediator.Send(new GetTypeOfPropertyByIdQuery { Id = id }));
            var response = await Mediator.Send(new GetTypeOfPropertyByIdQuery { Id = id });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [SwaggerOperation(
            Summary = "Creates a type of property",
            Description = "Gets the required parameters to create a new type of property"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TypeOfPropertyDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post(CreateTypeOfPropertyCommand command)
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
            Summary = "Updates a type of property",
            Description = "Gets the required parameters to update a type of property"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfPropertyDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(UpdateTypeOfPropertyCommand command)
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
            Summary = "Deletes a type of property",
            Description = "Gets the required parameters to delete a type of property"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTypeOfPropertyByIdCommand { Id = id });
            return NoContent();
        }
    }
}