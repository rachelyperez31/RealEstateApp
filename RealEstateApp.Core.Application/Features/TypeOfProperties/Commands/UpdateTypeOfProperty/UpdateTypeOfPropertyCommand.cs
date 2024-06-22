using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeOfProperty
{
    public class UpdateTypeOfPropertyCommand : IRequest<Response<UpdateTypeOfPropertyResponse>>
    {
        [SwaggerParameter(Description = "ID of the type of property to be updated.")]
        [Required]
        public int Id { get; set; }

        [SwaggerParameter(Description = "New name of the type of property.")]
        [Required]
        public string Name { get; set; }

        [SwaggerParameter(Description = "New description of the type of property.")]
        [Required]
        public string Description { get; set; }
    }

    public class UpdateTypeOfPropertyCommandHandler : IRequestHandler<UpdateTypeOfPropertyCommand, Response<UpdateTypeOfPropertyResponse>>
    {
        private readonly ITypeOfPropertyRepository _typeOfPropertyRepository;
        private readonly IMapper _mapper;

        public UpdateTypeOfPropertyCommandHandler(ITypeOfPropertyRepository typeOfPropertyRepository, IMapper mapper)
        {
            _typeOfPropertyRepository = typeOfPropertyRepository;
            _mapper = mapper;
        }
        public async Task<Response<UpdateTypeOfPropertyResponse>> Handle(UpdateTypeOfPropertyCommand command, CancellationToken cancellationToken)
        {
            var typeOfProperty = await _typeOfPropertyRepository.GetByIdAsync(command.Id);

            if (typeOfProperty == null)
            {
                throw new ApiException($"There isn't any type of property with this ID '{command.Id}' in the system.", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                var createdBy = typeOfProperty.CreatedBy;
                typeOfProperty = _mapper.Map<TypeOfProperty>(command);
                typeOfProperty.CreatedBy = createdBy;
                await _typeOfPropertyRepository.UpdateAsync(typeOfProperty, typeOfProperty.Id);
                var typeOfPropertyResponse = _mapper.Map<UpdateTypeOfPropertyResponse>(typeOfProperty);
                return new Response<UpdateTypeOfPropertyResponse>(typeOfPropertyResponse);
            }
        }
    }
}
