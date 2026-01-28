using InstitutionPanel.Core.Interfaces;
using InstitutionPanel.Data;
using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.DTOs.Institution;
using InstitutionPanel.Models.Entities;
using InstitutionPanel.Models.ValueObject;
using InstitutionPanel.Models.ViewModels;
using InstitutionPanel.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InstitutionPanel.Services.Implementations
{
    public class InstitutionService : IInstitutionService
    {
        private readonly ApplicationDbContext _context;
        public InstitutionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<InstitutionVM>> CreateInstitutionAsync(AddInstitutionCommand command)
        {
            var institution = new Institution
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Address = command.Address,
                Email = Email.From(command.Email),
                ContactNo = ContactNumber.From(command.ContactNo),
                PhoneNo = PhoneNumber.From(command.PhoneNo),
                IsActive = command.IsActive
            };

            await _context.Institutions.AddAsync(institution);
            await _context.SaveChangesAsync();

            var vm = MapToVM(institution);

            return ApiResponse<InstitutionVM>.SuccessResponse(
                vm,
                $"Institution '{institution.Name}' added successfully."
            );
        }

        public async Task<ApiResponse<List<InstitutionVM>>> GetAllInstitutionAsync()
        {
            var institutions = await _context.Institutions
                                            .AsNoTracking()
                                            .Select(x => new InstitutionVM
                                            {
                                                Id = x.Id,
                                                Name = x.Name,
                                                Email = x.Email,
                                                Description = x.Description,
                                                Address = x.Address,
                                                ContactNo = x.ContactNo,
                                                PhoneNo = x.PhoneNo,
                                                IsActive = x.IsActive,
                                                Created = x.CreatedAt,
                                                UpdatedAt = x.UpdatedAt
                                            })
                                            .OrderByDescending(d => d.UpdatedAt)
                                            .ThenByDescending(d => d.Created)
                                            .ToListAsync();

            return ApiResponse<List<InstitutionVM>>.SuccessResponse(institutions);
        }

        public async Task<ApiResponse<InstitutionVM>> GetInstitutionByIdAsync(Guid id)
        {
            var institution = await _context.Institutions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (institution == null)
            {
                return ApiResponse<InstitutionVM>.FailureResponse(
                    "Institution not found"
                );
            }

            return ApiResponse<InstitutionVM>.SuccessResponse(MapToVM(institution));
        }

        public async Task<ApiResponse<bool>> UpdateInstitutionAsync(UpdateInstitutionCommand command)
        {
            var institution = await _context.Institutions
                .FirstOrDefaultAsync(x => x.Id == command.Id);

            if (institution == null)
            {
                return ApiResponse<bool>.FailureResponse("Institution not found");
            }

            institution.Name = command.Name;
            institution.Email = Email.From(command.Email);
            institution.Description = command.Description;
            institution.Address = command.Address;
            institution.ContactNo = ContactNumber.From(command.ContactNo);
            institution.PhoneNo = PhoneNumber.From(command.PhoneNo);
            institution.IsActive = command.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Institution updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteInstitutionAsync(Guid id)
        {
            bool hasActiveUser = await _context.Users
                                            .AsNoTracking()
                                            .AnyAsync(u => u.InstitutionId == id && u.IsActive);

            if (hasActiveUser)
                return ApiResponse<bool>.FailureResponse("You cannot delete institution with active user/s.");

            var institution = await _context.Institutions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (institution == null)
            {
                return ApiResponse<bool>.FailureResponse("Institution not found");
            }

            _context.Institutions.Remove(institution);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Institution deleted successfully");
        }

        public async Task<bool> IsInstitutionActiveAsync(Guid institutionId)
        {
            return await _context.Institutions
                .AsNoTracking()
                .AnyAsync(x => x.Id == institutionId && x.IsActive);
        }

        private static InstitutionVM MapToVM(Institution entity)
        {
            return new InstitutionVM
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Description = entity.Description,
                Address = entity.Address,
                ContactNo = entity.ContactNo,
                PhoneNo = entity.PhoneNo,
                IsActive = entity.IsActive
            };
        }
    }
}
