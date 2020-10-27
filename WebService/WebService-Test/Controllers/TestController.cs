using WebService_Test.Components;
using WebService_Lib.Attributes;

namespace WebService_Test.Controllers
{

    [Controller]
    public class TestController
    {
        [Autowired]
        private TestLogger logger;

        public TestLogger Logger => logger;
    }
}