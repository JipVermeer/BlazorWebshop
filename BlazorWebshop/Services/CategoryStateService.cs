namespace BlazorWebshop.Services
{
    public class CategoryStateService
    {
        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
