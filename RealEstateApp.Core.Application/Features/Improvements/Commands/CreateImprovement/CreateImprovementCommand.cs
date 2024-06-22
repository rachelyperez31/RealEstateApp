using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{
    public class CreateImprovementCommand : IRequest<Response<int>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "Name of the improvement.")]
        [Required]
        public string Name { get; set; }
        [SwaggerParameter(Description = "Description of the improvement.")]
        [Required]
        public string Description { get; set; }
    }

    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, Response<int>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateImprovementCommand command, CancellationToken cancellationToken)
        {

            var improvement = _mapper.Map<Improvement>(command);
            improvement = await _improvementRepository.AddAsync(improvement);
            return new Response<int>(improvement.Id);
        }
    }
}
