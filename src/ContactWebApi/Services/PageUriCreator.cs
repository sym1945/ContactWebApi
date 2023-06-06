using ContactWebApi.App.Common.Interfaces;

namespace ContactWebApi.Services
{
    public class PageUriCreator : IPageUriCreator
    {
        private readonly Func<int, int, Uri> _CreateUriFunction;

        public PageUriCreator(Func<int, int, Uri> createFunction)
        {
            _CreateUriFunction = createFunction;
        }

        public Uri? CreateUri(int page, int pageSize)
        {
            return _CreateUriFunction.Invoke(page, pageSize);
        }

    }
}
