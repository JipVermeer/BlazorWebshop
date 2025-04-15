using Blazored.Toast.Services;
using Microsoft.JSInterop;

namespace BlazorWebshop.Services
{
    public class NotificationService
    {
        //    private readonly IToastService _toastService;

        //    public NotificationService(IToastService toastService)
        //    {
        //        Console.WriteLine("NOTIFICATIONSERVICE REACHED");
        //        _toastService = toastService;
        //    }

        //    public string Message { get; private set; }
        //    public ToastLevel Level { get; private set; }

        //    public void SetNotification(string message, ToastLevel level = ToastLevel.Info)
        //    {
        //        Console.WriteLine($"SETNOTIFICATION CALLED: {message}, {level}");
        //        Message = message;
        //        Level = level;
        //    }

        //    public void ShowNotification()
        //    {
        //        Console.WriteLine("NOTIFICATIONSERVICE.SHOWNOTIFICATION REACHED");

        //        Console.WriteLine(Message);
        //        Console.WriteLine(Level);

        //        if (!string.IsNullOrEmpty(Message))
        //        {
        //            Console.WriteLine("MESSAGE NOT NULL");
        //            _toastService.ShowToast(Level, Message);
        //            Message = string.Empty;
        //        }
        //    }

        // MOET VIA SESSION STORAGE VANWEGE SCOPED/SINGLETON PROBLEEM

        private readonly IJSRuntime _jsRuntime;
        private readonly IToastService _toastService;

        public string Message { get; private set; } = "";
        public ToastLevel Level { get; private set; } = ToastLevel.Info;

        public NotificationService(IToastService toastService, IJSRuntime jsRuntime)
        {
            _toastService = toastService;
            _jsRuntime = jsRuntime;
        }

        public async Task SetNotification(string message, ToastLevel level = ToastLevel.Info)
        {
            Message = message;
            Level = level;

            Console.WriteLine($"IN SETNOTIFICATION - MESSAGE: {Message}, LEVEL: {Level}");

            // Sla notificatie op in sessionstorage
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "notification_message", Message);
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "notification_level", level.ToString());
        }

        public async Task ShowNotification()
        {
            // Laad notificatie
            var storedMessage = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "notification_message");
            var storedLevel = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "notification_level");

            if (!string.IsNullOrEmpty(storedMessage) && Enum.TryParse(storedLevel, out ToastLevel parsedLevel))
            {
                Message = storedMessage;
                Level = parsedLevel;
            }

            if (!string.IsNullOrEmpty(Message))
            {
                _toastService.ShowToast(Level, Message);

                // Maak sessionstorage weer leeg
                await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "notification_message");
                await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "notification_level");

                var messageCheck = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "notification_message");
                var levelCheck = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "notification_level");
                storedMessage = null;
                storedLevel = null;
                Message = null;
            }
        }
    }
}
