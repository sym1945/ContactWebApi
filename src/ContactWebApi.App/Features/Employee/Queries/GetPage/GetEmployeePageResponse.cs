using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageResponse
    {
        public Uri? First { get; set; }
        public Uri? Prev { get; set; }
        public Uri? Next { get; set; }
        public Uri? Last { get; set; }

        public int CurrentPageNo { get; init; }
        public int TotalPageCount { get; init; }

        public int ItemCount { get; init; }
        public IList<EmployeeDto> Items { get; init; }

        public GetEmployeePageResponse(IList<EmployeeDto> items, int currentPageNo, int totalPageCount, int pageSize, IPageUriCreator? uriCreator)
        {
            Items = items;
            ItemCount = items.Count;
            CurrentPageNo = currentPageNo;
            TotalPageCount = totalPageCount;

            if (uriCreator != null)
            {
                if (currentPageNo > 1)
                {
                    First = uriCreator.CreateUri(1, pageSize);
                    Prev = uriCreator.CreateUri(currentPageNo - 1, pageSize);
                }

                if (currentPageNo < totalPageCount)
                {
                    Next = uriCreator.CreateUri(currentPageNo + 1, pageSize);
                    Last = uriCreator.CreateUri(totalPageCount, pageSize);
                }
            }
        }

    }
}
