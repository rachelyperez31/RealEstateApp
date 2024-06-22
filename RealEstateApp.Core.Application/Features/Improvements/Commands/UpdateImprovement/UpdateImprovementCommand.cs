using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Agent;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    public class UpdateImprovementCommand : IRequest<Response<UpdateImprovementResponse>>
    {
        [SwaggerParameter(Description = "Id of the improvement to be updated.")]
        [Required]
        public int Id { get; set; }

        [SwaggerParameter(Description = "New name of the improvement.")]
        [Required]
        public string Name { get; set; }

        [SwaggerParameter(Description = "New description of the improvement.")]
        [Required]
        public string Description { get; set; }
    }

    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, Response<UpdateImprovementResponse>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<UpdateImprovementResponse>> Handle(UpdateImprovementCommand command, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(command.Id);

            if (improvement == null)
            {
                throw new ApiException($"There isn't any improvement with this ID '{command.Id}' registered in the system.", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                var createdBy = improvement.CreatedBy;
                improvement = _mapper.Map<Improvement>(command);
                improvement.CreatedBy = createdBy;
                await _improvementRepository.UpdateAsync(improvement, improvement.Id);
                var improvementResponse = _mapper.Map<UpdateImprovementResponse>(improvement);
                return new Response<UpdateImprovementResponse>(improvementResponse);
            }
        }
    }
}
