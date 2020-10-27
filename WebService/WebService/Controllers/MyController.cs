using WebService.Components;
using WebService_Lib.Attributes;

namespace WebService.Controllers
{
    [Controller]
    class MyController
    {
        [Autowired]
        private Logger logger;
    }
}
