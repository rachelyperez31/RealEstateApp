using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeOfProperty
{
    public class CreateTypeOfPropertyCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Name of the type of property.")]
        [Required]
        public string Name { get; set; }
        [SwaggerParameter(Description = "Description of the type of property.")]
        [Required]
        public string Description { get; set; }
    }

    public class CreateTypeOfPropertyCommandHandler : IRequestHandler<CreateTypeOfPropertyCommand, Response<int>>
    {
        private readonly ITypeOfPropertyRepository _typeOfPropertyRepository;
        private readonly IMapper _mapper;

        public CreateTypeOfPropertyCommandHandler(ITypeOfPropertyRepository typeOfPropertyRepository, IMapper mapper)
        {
            _typeOfPropertyRepository = typeOfPropertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTypeOfPropertyCommand command, CancellationToken cancellationToken)
        {
            var typeOfProperty = _mapper.Map<TypeOfProperty>(command);
            typeOfProperty = await _typeOfPropertyRepository.AddAsync(typeOfProperty);
            return new Response<int>(typeOfProperty.Id);
        }
    }
}
