using SrsApi.DbContext;
using SrsApi.Enums;
using SrsApi.Interfaces;
using SrsApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrsApiTests.Services
{
    [TestClass]
    public class IncludesTests
    {
        private readonly IBaseServiceWithIncludes<SrsItem> _srsItemService;

        public IncludesTests()
        {
            //TODO: need to moq the dbcontext, and then add tests to make sure that all expected .includes do something

            //_srsItemService = new SrsItemService();
        }

        
    }
}
