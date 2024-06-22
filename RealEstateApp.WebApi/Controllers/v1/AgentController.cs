using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.DTOs.Agent;
using RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatusAgent;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperties;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAllAgents;
using RealEstateApp.WebApi.Controllers.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Agents Maintenance")]
    public class AgentController : BaseApiController
    {
        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("List")]
        [SwaggerOperation(
            Summary = "List of Agents",
            Description = "Gets all agents created"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get()
        {
            //return Ok(await Mediator.Send(new GetAllAgentsQuery()));
            var response = await Mediator.Send(new GetAllAgentsQuery());
            if (response.Data == null || !response.Data.Any())
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("GetById")]
        [SwaggerOperation(
            Summary = "Agent by id",
            Description = "Gets an agent by id as a filter"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(string id)
        {
            //return Ok(await Mediator.Send(new GetAgentByIdQuery { Id = id }));
            var response = await Mediator.Send(new GetAgentByIdQuery { Id = id });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin,Developer")]
        [HttpGet("GetAgentProperties")]
        [SwaggerOperation(
            Summary = "Agent properties",
            Description = "Gets the properties created by the agent filtered by the agent id"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAgentProperties(string agentId)
        {
            //return Ok(await Mediator.Send(new GetAgentPropertiesQuery { AgentId = agentId }));
            var response = await Mediator.Send(new GetAgentPropertiesQuery { AgentId = agentId });
            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("ChangeStatus")]
        [SwaggerOperation(
            Summary = "Change agent activation status",
            Description = "Gets the required parameters to change the agent status filtered by agent id and  a boolean value (status)\n\n" +
            "true → active\n\n" +
            "false → disabled"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(ChangeStatusAgentCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

    }
}
