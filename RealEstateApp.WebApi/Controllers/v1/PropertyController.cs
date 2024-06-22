using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetPropertyByCode;
using RealEstateApp.Core.Application.Features.Properties.Queries.GetPropertyById;
using RealEstateApp.WebApi.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin,Developer")]
    [SwaggerTag("Properties Maintenace")]
    public class PropertyController : BaseApiController
    {
        [HttpGet("List")]
        [SwaggerOperation(
          Summary = "List of Properties",
          Description = "Gets all the properties created with the information of the corresponding agent"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            //return Ok(await Mediator.Send(new GetAllPropertiesQuery()));
            var response = await Mediator.Send(new GetAllPropertiesQuery());
            if (response.Data == null || !response.Data.Any())
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [HttpGet("GetById")]
        [SwaggerOperation(
          Summary = "Property by id",
          Description = "Gets a property by an id as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(int id)
        {
            //return Ok(await Mediator.Send(new GetPropertyByIdQuery { Id = id }));
            var response = await Mediator.Send(new GetPropertyByIdQuery { Id = id });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [HttpGet("GetByCode")]
        [SwaggerOperation(
          Summary = "Property by code",
          Description = "Gets a property by an code as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(string code)
        {
            //return Ok(await Mediator.Send(new GetPropertyByCodeQuery { Code = code }));
            var response = await Mediator.Send(new GetPropertyByCodeQuery { Code = code });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }
    }
}
