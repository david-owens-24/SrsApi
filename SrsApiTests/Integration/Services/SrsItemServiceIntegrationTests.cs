using Microsoft.EntityFrameworkCore;
using Moq;
using SrsApi.DbContext;
using SrsApi.Interfaces;
using SrsApi.Services;
using SrsApiTests.TestClasses;

namespace SrsApiTests.Integration.Services
{
    [TestClass]
    public class SrsItemServiceIntegrationTests
    {
        private ApplicationDbContext _context;
        private SrsItemService _srsItemService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options, new TestUserResolutionService());

            // Seed a SrsItemLevel and SrsItemDetails
            var level = new SrsItemLevel
            {
                Id = 1,
                UID = Guid.NewGuid(),
                Level = 1,
                Name = "Level 1"
            };

            var details = new SrsItemDetails
            {
                Id = 1,
                UID = Guid.NewGuid(),
                Question = "Sample Question"
            };

            // Add related entities to the context first
            _context.SrsItemLevels.Add(level);
            _context.SrsItemDetails.Add(details);
            _context.SaveChanges();

            var srsItem = new SrsItem
            {
                Id = 1,
                UID = Guid.NewGuid(),
                Order = 1,
                Level = level,
                Details = details,
                Answers = new List<SrsAnswer>()
            };

            _context.SrsItems.Add(srsItem);
            _context.SaveChanges();

            _srsItemService = new SrsItemService(_context);
        }

        [TestMethod]
        public void Setup_HasAtLeastOneSrsItemWithLevelAndDetails()
        {
            var items = _context.SrsItems.Include(x => x.Level).Include(x => x.Details).ToList();
            Assert.IsTrue(items.Count > 0);
            Assert.IsNotNull(items[0].Level);
            Assert.IsNotNull(items[0].Details);
        }

        [TestMethod]
        public async Task Add_SrsItem_AddsToDataSet()
        {
            var level = new SrsItemLevel
            {
                Id = 2,
                Level = 2,
                Name = "Level 2"
            };

            var details = new SrsItemDetails
            {
                UID = Guid.NewGuid(),
                Question = "Another Question"
            };

            var srsItem = new SrsItem
            {
                UID = Guid.NewGuid(),
                Order = 2,
                Level = level,
                Details = details,
                Answers = new List<SrsAnswer>()
            };

            await _srsItemService.Add(srsItem);
            _context.SaveChanges();

            var srsItemInDatabase = await _srsItemService.GetByUID(srsItem.UID);

            Assert.IsNotNull(srsItemInDatabase);
        }

        // TODO: re-do the above test to only check for unique stuff done by the SrsItemService
        // like setting the Order correctly
        // as Create, Update, Read, Delete tests should be done on the BaseService class
    }
}