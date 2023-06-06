using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Common.Validator;
using MediatR;
using System.ComponentModel.DataAnnotations;
using static ContactWebApi.App.Constants.Employee.EmployeePaging;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequest : IRequest<GetEmployeePageResponse>
    {
        private IPageUriCreator? _PageUriCreator;

        [Required]
        [MinValueInt32(PageMin)]
        public int? Page { get; set; }
        [Required]
        [Range(PageMin, PageMax)]
        public int? PageSize { get; set; }

        public IPageUriCreator? GetPageUriCreator()
        {
            return _PageUriCreator;
        }

        public void SetPageUriCreator(IPageUriCreator creator)
        {
            _PageUriCreator = creator;
        }
    }
}
