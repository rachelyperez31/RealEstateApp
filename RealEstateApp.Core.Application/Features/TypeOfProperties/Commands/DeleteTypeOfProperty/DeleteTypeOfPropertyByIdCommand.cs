using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeOfProperty
{
    public class DeleteTypeOfPropertyByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "ID of the type of property to be deleted.")]
        [Required]
        public int Id { get; set; }
    }

    public class DeleteTypeOfPropertyByIdCommandHandler : IRequestHandler<DeleteTypeOfPropertyByIdCommand, Response<int>>
    {
        private readonly ITypeOfPropertyRepository _typeOfPropertyRepository;

        public DeleteTypeOfPropertyByIdCommandHandler(ITypeOfPropertyRepository typeOfPropertyRepository)
        {
            _typeOfPropertyRepository = typeOfPropertyRepository;
        }
        public async Task<Response<int>> Handle(DeleteTypeOfPropertyByIdCommand command, CancellationToken cancellationToken)
        {
            var typeOfProperty = await _typeOfPropertyRepository.GetByIdAsync(command.Id);
            //if (typeOfProperty == null) throw new ApiException($"There isn't any type of property with this ID {command.Id} in the system.", (int)HttpStatusCode.NoContent);
            await _typeOfPropertyRepository.DeleteAsync(typeOfProperty);
            return new Response<int>(typeOfProperty.Id);
        }
    }
}
