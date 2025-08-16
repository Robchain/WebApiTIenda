using WebApiTIenda.Application.DTOs.Admin;

namespace WebApiTIenda.Application.Interfaces.Admin
{
    public interface ILotAdminService
    {
        Task<ApiResponseDto<List<LotListDto>>> GetAllLotsAsync();
        Task<ApiResponseDto<LotDetailDto>> CreateLotAsync(LotCreateDto createDto);
        Task<ApiResponseDto<LotDetailDto>> UpdateLotAsync(int lotId, LotUpdateDto updateDto);
        Task<ApiResponseDto<bool>> DeleteLotAsync(int lotId);
    }
}
