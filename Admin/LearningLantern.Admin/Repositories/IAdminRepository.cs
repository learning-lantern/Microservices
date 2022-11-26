using Microsoft.AspNetCore.Identity;

namespace LearningLantern.Admin.Repositories
{
    public interface IAdminRepository
    {
        public Task<IdentityResult> AddToRoleInstructorAsync(string userId);
        public Task<IdentityResult> AddToRoleAdminAsync(string userId);
        public Task<IdentityResult> AddToRoleUniversityAdminAsync(string userId);
        public Task<IdentityResult> CreateRolesAsync();
    }
}
