using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.UpdateTypeOfSale
{
    public class UpdateTypeOfSaleCommand : IRequest<Response<UpdateTypeOfSaleResponse>>
    {
        [SwaggerParameter(Description = "ID of the type of sale to be updated.")]
        [Required]
        public int Id { get; set; }

        [SwaggerParameter(Description = "New name of the type of sale.")]
        [Required]
        public string Name { get; set; }

        [SwaggerParameter(Description = "New description of the type of sale.")]
        [Required]
        public string Description { get; set; }
    }

    public class UpdateTypeOfSaleCommandHandler : IRequestHandler<UpdateTypeOfSaleCommand, Response<UpdateTypeOfSaleResponse>>
    {
        private readonly ITypeOfSaleRepository _typeOfSaleRepository;
        private readonly IMapper _mapper;

        public UpdateTypeOfSaleCommandHandler(ITypeOfSaleRepository typeOfSaleRepository, IMapper mapper)
        {
            _typeOfSaleRepository = typeOfSaleRepository;
            _mapper = mapper;
        }

        public async Task<Response<UpdateTypeOfSaleResponse>> Handle(UpdateTypeOfSaleCommand command, CancellationToken cancellationToken)
        {
            var typeOfSale = await _typeOfSaleRepository.GetByIdAsync(command.Id);

            if (typeOfSale == null)
            {
                throw new ApiException($"There isn't any type of property with this ID '{command.Id}' in the system.", (int)HttpStatusCode.BadRequest);
            }
            else
            {
                var createdBy = typeOfSale.CreatedBy;
                typeOfSale = _mapper.Map<TypeOfSale>(command);
                typeOfSale.CreatedBy = createdBy;
                await _typeOfSaleRepository.UpdateAsync(typeOfSale, typeOfSale.Id);
                var typeOfSaleResponse = _mapper.Map<UpdateTypeOfSaleResponse>(typeOfSale);
                return new Response<UpdateTypeOfSaleResponse>(typeOfSaleResponse);
            }
        }
    }
}
