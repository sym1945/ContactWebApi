using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Common.Validator;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequest : IRequest<GetEmployeePageResponse>
    {
        private IPageUriCreator? _PageUriCreator;

        [Required]
        [MinValueInt32(1)]
        public int? Page { get; set; }
        [Required]
        [Range(1, 100)]
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
