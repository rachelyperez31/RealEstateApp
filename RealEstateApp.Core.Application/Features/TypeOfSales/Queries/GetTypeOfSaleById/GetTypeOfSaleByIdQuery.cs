using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.TypeOfSale;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetTypeOfSaleById
{
    public class GetTypeOfSaleByIdQuery : IRequest<Response<TypeOfSaleDTO>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "Debe colocar el id del tipo de venta que desea obtener")]
        [Required]
        public int Id { get; set; }
    }

    public class GetTypeOfSaleByIdQueryHandler : IRequestHandler<GetTypeOfSaleByIdQuery, Response<TypeOfSaleDTO>>
    {
        private readonly ITypeOfSaleRepository _typeOfSaleRepository;
        private readonly IMapper _mapper;

        public GetTypeOfSaleByIdQueryHandler(ITypeOfSaleRepository typeOfSaleRepository, IMapper mapper)
        {
            _typeOfSaleRepository = typeOfSaleRepository;
            _mapper = mapper;
        }

        public async Task<Response<TypeOfSaleDTO>> Handle(GetTypeOfSaleByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetDTO(request.Id);
        }

        #region Private
        private async Task<Response<TypeOfSaleDTO>> GetDTO(int id)
        {
            var typeOfSale = await _typeOfSaleRepository.GetByIdAsync(id);

            //if (typeOfSale == null || typeOfSale.IsDeleted) throw new ApiException($"There is not any type of sale registered in the system", (int)HttpStatusCode.NoContent);

            var typeOfSaleDTO = _mapper.Map<TypeOfSaleDTO>(typeOfSale);
            var response = new Response<TypeOfSaleDTO>(typeOfSaleDTO);
            return response;
        }
        #endregion
    }
}

