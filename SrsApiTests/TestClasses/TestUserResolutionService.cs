using SrsApi.DbContext;
using SrsApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrsApiTests.TestClasses
{
    internal class TestUserResolutionService : IUserResolutionService
    {
        public string GetCurrentSessionUserId(ApplicationDbContext dbContext)
        {
            return "test-user-id"; // Return a test user ID for unit tests
        }
    }
}
