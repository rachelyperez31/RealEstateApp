using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.TypeOfProperty;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetAllTypeOfProperties
{
    public class GetAllTypeOfPropertiesQuery : IRequest<Response<IList<TypeOfPropertyDTO>>>
    {
        // parámetros que espera este request
    }

    public class GetAllTypeOfPropertiesQueryHandler : IRequestHandler<GetAllTypeOfPropertiesQuery, Response<IList<TypeOfPropertyDTO>>>
    {
        private readonly ITypeOfPropertyRepository _typeOfPropertyRepository;
        private readonly IMapper _mapper;

        public GetAllTypeOfPropertiesQueryHandler(ITypeOfPropertyRepository typeOfPropertyRepository, IMapper mapper)
        {
            _typeOfPropertyRepository = typeOfPropertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<TypeOfPropertyDTO>>> Handle(GetAllTypeOfPropertiesQuery request, CancellationToken cancellationToken)
        {
            return await GetAllDTOs();
        }

        #region Private
        public async Task<Response<IList<TypeOfPropertyDTO>>> GetAllDTOs()
        {
            var typeOfPropertyList = await _typeOfPropertyRepository.GetAllAsync();
            typeOfPropertyList = typeOfPropertyList.Where(tp => !tp.IsDeleted).ToList();

            //if (typeOfPropertyList == null || typeOfPropertyList.Count == 0) throw new ApiException($"There aren't type of properties registered in the system.", (int)HttpStatusCode.NoContent);

            //return _mapper.Map<Response<IList<TypeOfPropertyDTO>>>(typeOfPropertyList);
            return new Response<IList<TypeOfPropertyDTO>>(typeOfPropertyList.Select(tp => new TypeOfPropertyDTO
            {
                Id = tp.Id,
                Name = tp.Name,
                Description = tp.Description
            }).ToList());
        }
        #endregion
    }
}
