using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.TypeOfProperty;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetTypeOfPropertyById
{
    public class GetTypeOfPropertyByIdQuery : IRequest<Response<TypeOfPropertyDTO>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "Must enter the type of property ID you want to obtain.")]
        [Required]
        public int Id { get; set; }
    }

    public class GetTypeOfPropertyByIdQueryHandler : IRequestHandler<GetTypeOfPropertyByIdQuery, Response<TypeOfPropertyDTO>>
    {
        private readonly ITypeOfPropertyRepository _typeOfPropertyRepository;
        private readonly IMapper _mapper;

        public GetTypeOfPropertyByIdQueryHandler(ITypeOfPropertyRepository typeOfPropertyRepository, IMapper mapper)
        {
            _typeOfPropertyRepository = typeOfPropertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<TypeOfPropertyDTO>> Handle(GetTypeOfPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetDTO(request.Id);
        }

        #region Private
        private async Task<Response<TypeOfPropertyDTO>> GetDTO(int id)
        {
            var typeOfProperty = await _typeOfPropertyRepository.GetByIdAsync(id);

            //if (typeOfProperty == null || typeOfProperty.IsDeleted) throw new ApiException($"There isn't any type of property with this ID {id} in the system.", (int)HttpStatusCode.NoContent);

            var typeOfPropertyDTO = _mapper.Map<TypeOfPropertyDTO>(typeOfProperty);
            var response = new Response<TypeOfPropertyDTO>(typeOfPropertyDTO);

            return response;
        }
        #endregion
    }
}
