
namespace TogglTool.Api
{
    public abstract class TogglBaseRepository
    {
        protected TogglApi Api { get; private set; }

        protected TogglBaseRepository(TogglApi togglApi)
        {
            Api = togglApi;
        }
    }
}
