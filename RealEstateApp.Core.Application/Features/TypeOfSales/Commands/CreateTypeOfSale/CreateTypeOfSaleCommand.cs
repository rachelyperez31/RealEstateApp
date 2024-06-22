using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Commands.CreateTypeOfSale
{
    public class CreateTypeOfSaleCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Name of the type of sale.")]
        [Required]
        public string Name { get; set; }
        [SwaggerParameter(Description = "Description of the type of sale.")]
        [Required]
        public string Description { get; set; }
    }

    public class CreateTypeOfSaleCommandHandler : IRequestHandler<CreateTypeOfSaleCommand, Response<int>>
    {
        private readonly ITypeOfSaleRepository _typeOfSaleRepository;
        private readonly IMapper _mapper;

        public CreateTypeOfSaleCommandHandler(ITypeOfSaleRepository typeOfSaleRepository, IMapper mapper)
        {
            _typeOfSaleRepository = typeOfSaleRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTypeOfSaleCommand command, CancellationToken cancellationToken)
        {
            var typeOfSale = _mapper.Map<TypeOfSale>(command);
            typeOfSale = await _typeOfSaleRepository.AddAsync(typeOfSale);
            return new Response<int>(typeOfSale.Id);
        }
    }
}
