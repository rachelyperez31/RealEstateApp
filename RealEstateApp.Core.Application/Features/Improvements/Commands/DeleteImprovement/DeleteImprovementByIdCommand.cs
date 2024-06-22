using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovement
{
    public class DeleteImprovementByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "ID of the improvement to be deleted.")]
        [Required]
        public int Id { get; set; }
    }

    public class DeleteImprovementByIdCommandHandler : IRequestHandler<DeleteImprovementByIdCommand, Response<int>>
    {
        private readonly IImprovementRepository _improvementRepository;

        public DeleteImprovementByIdCommandHandler(IImprovementRepository improvementRepository)
        {
            _improvementRepository = improvementRepository;
        }

        public async Task<Response<int>> Handle(DeleteImprovementByIdCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);
            //if (improvement == null) throw new ApiException($"There isn't any improvement with this ID {command.Id} registered in the system.", (int)HttpStatusCode.NoContent);

            await _improvementRepository.DeleteAsync(improvement);
            return new Response<int>(improvement.Id);
        }
    }
}
