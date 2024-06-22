using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.DeleteTypeOfSale
{
    public class DeleteTypeOfSaleByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "ID of the type of sale to be deleted.")]
        [Required]
        public int Id { get; set; }
    }

    public class DeleteTypeOfSaleByIdCommandHandler : IRequestHandler<DeleteTypeOfSaleByIdCommand, Response<int>>
    {
        private readonly ITypeOfSaleRepository _typeOfSaleRepository;

        public DeleteTypeOfSaleByIdCommandHandler(ITypeOfSaleRepository typeOfSaleRepository)
        {
            _typeOfSaleRepository = typeOfSaleRepository;
        }
        public async Task<Response<int>> Handle(DeleteTypeOfSaleByIdCommand command, CancellationToken cancellationToken)
        {
            var typeOfSale = await _typeOfSaleRepository.GetByIdAsync(command.Id);
            //if (typeOfSale == null) throw new ApiException($"There isn't any type of sale with this ID {command.Id} in the system.", (int)HttpStatusCode.NoContent);
            await _typeOfSaleRepository.DeleteAsync(typeOfSale);
            return new Response<int>(typeOfSale.Id);
        }
    }
}

