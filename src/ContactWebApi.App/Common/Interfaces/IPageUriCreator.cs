namespace ContactWebApi.App.Common.Interfaces
{
    public interface IPageUriCreator
    {
        Uri? CreateUri(int page, int pageSize);
    }
}
