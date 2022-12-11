using Stemauto.Entities;
using Stemauto.ViewModels.Shared;

namespace Stemauto.ViewModels.Sale
{
    public class IndexVM
    {
        public List<Car> Cars { get; set; }

        public FilterVM Filter { get; set; }

        public PagerVM Pager { get; set; }

    }
}
