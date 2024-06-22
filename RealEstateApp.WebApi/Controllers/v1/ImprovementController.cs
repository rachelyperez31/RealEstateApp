using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements;
using RealEstateApp.Core.Application.Features.Improvements.Queries.GetImprovementById;
using RealEstateApp.WebApi.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Improvements Maintenance")]
    public class ImprovementController : BaseApiController
    {
        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("List")]
        [SwaggerOperation(
            Summary = "List of Improvements",
            Description = "Gets all improvements created"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            //return Ok(await Mediator.Send(new GetAllImprovementsQuery()));
            var response = await Mediator.Send(new GetAllImprovementsQuery());
            if (response.Data == null || !response.Data.Any())
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("GetById")]
        [SwaggerOperation(
            Summary = "Improvement by id",
            Description = "Gets an improvement by id as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(int id)
        {
            /*var improvement = await Mediator.Send(new GetImprovementByIdQuery { Id = id });

            if (improvement == null)
            { 
                return NoContent(); 
            }

            return Ok(improvement);*/

            var response = await Mediator.Send(new GetImprovementByIdQuery { Id = id });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [SwaggerOperation(
            Summary = "Creates a improvement",
            Description = "Gets the required parameters to create a new improvement"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImprovementDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post(CreateImprovementCommand command)
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
            Summary = "Updates a improvement",
            Description = "Gets the required parameters to update a improvement"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(UpdateImprovementCommand command)
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
            Summary = "Deletes a improvement",
            Description = "Gets the required parameters to delete a improvement"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteImprovementByIdCommand { Id = id });
            return NoContent();
        }

    }
}
