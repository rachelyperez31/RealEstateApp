using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.TypeOfSale;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;

namespace RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetAllTypeOfSales
{
    public class GetAllTypeOfSalesQuery : IRequest<Response<IList<TypeOfSaleDTO>>>
    {
        // parámetros que espera este request
    }

    public class GetAllTypeOfSalesQueryHandler : IRequestHandler<GetAllTypeOfSalesQuery, Response<IList<TypeOfSaleDTO>>>
    {
        private readonly ITypeOfSaleRepository _typeOfSaleRepository;
        private readonly IMapper _mapper;

        public GetAllTypeOfSalesQueryHandler(ITypeOfSaleRepository typeOfSaleRepository, IMapper mapper)
        {
            _typeOfSaleRepository = typeOfSaleRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<TypeOfSaleDTO>>> Handle(GetAllTypeOfSalesQuery request, CancellationToken cancellationToken)
        {
            return await GetAllDTO();
        }

        #region Private
        private async Task<Response<IList<TypeOfSaleDTO>>> GetAllDTO()
        {
            var typeOfSales = await _typeOfSaleRepository.GetAllAsync();
            typeOfSales = typeOfSales.Where(ts => !ts.IsDeleted).ToList();

            //if (typeOfSales == null || typeOfSales.Count == 0) throw new ApiException($"There are no types of sales registered in the system.", (int)HttpStatusCode.NoContent);

            //return _mapper.Map<Response<IList<TypeOfSaleDTO>>>(typeOfSales);
            return new Response<IList<TypeOfSaleDTO>>(typeOfSales.Select(tp => new TypeOfSaleDTO
            {
                Id = tp.Id,
                Name = tp.Name,
                Description = tp.Description
            }).ToList());
        }
        #endregion
    }
}
