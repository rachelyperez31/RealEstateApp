using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetImprovementById
{
    public class GetImprovementByIdQuery : IRequest<Response<ImprovementDTO>>
    {
        // parámetros que espera este request
        [SwaggerParameter(Description = "ID of the improvement you want to obtain.")]
        [Required]
        public int Id { get; set; }
    }

    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, Response<ImprovementDTO>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<ImprovementDTO>> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetDTO(request.Id);
        }

        #region Private
        private async Task<Response<ImprovementDTO>> GetDTO(int id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);

            //if (improvement == null || improvement.IsDeleted) throw new ApiException($"There isn't any improvement with this ID {id} registered in the system.", (int)HttpStatusCode.NoContent);
            if (improvement == null) return new Response<ImprovementDTO>(null);

            var improvementDTO = _mapper.Map<ImprovementDTO>(improvement);
            var response = new Response<ImprovementDTO>(improvementDTO);

            return response;
        }
        #endregion
    }
}
