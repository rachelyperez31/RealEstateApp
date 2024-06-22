using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;

namespace RealEstateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements
{
    public class GetAllImprovementsQuery : IRequest<Response<IList<ImprovementDTO>>>
    {
        // parámetros que espera este request
    }

    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, Response<IList<ImprovementDTO>>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAllImprovementsQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        public async Task<Response<IList<ImprovementDTO>>> Handle(GetAllImprovementsQuery request, CancellationToken cancellationToken)
        {
            return await GetAllDTOs();
        }

        #region Private
        private async Task<Response<IList<ImprovementDTO>>> GetAllDTOs()
        {
            var improvements = await _improvementRepository.GetAllAsync();
            improvements = improvements.Where(i => !i.IsDeleted).ToList();

            //if (improvements == null || improvements.Count == 0) throw new ApiException($"There are not improvements registered in the system.", (int)HttpStatusCode.NoContent);

            return new Response<IList<ImprovementDTO>>(improvements.Select(tp => new ImprovementDTO
            {
                Id = tp.Id,
                Name = tp.Name,
                Description = tp.Description
            }).ToList());
        }
        #endregion
    }
}
