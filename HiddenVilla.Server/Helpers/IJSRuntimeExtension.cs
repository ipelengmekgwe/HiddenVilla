using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Server.Helpers
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }

        public static async ValueTask ToastrError(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }

        public static async ValueTask ToastrWarning(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "warning", message);
        }

        public static async ValueTask ToastrInfo(this IJSRuntime jSRuntime, string message)
        {
            await jSRuntime.InvokeVoidAsync("ShowToastr", "info", message);
        }

        public static async ValueTask SwalSuccess(this IJSRuntime jSRuntime, string message, string title = null)
        {
            await jSRuntime.InvokeVoidAsync("ShowSwal", "success", message, title);
        }

        public static async ValueTask SwalError(this IJSRuntime jSRuntime, string message, string title = null)
        {
            await jSRuntime.InvokeVoidAsync("ShowSwal", "error", message, title);
        }

        public static async ValueTask SwalWarning(this IJSRuntime jSRuntime, string message, string title = null)
        {
            await jSRuntime.InvokeVoidAsync("ShowSwal", "warning", message, title);
        }

        public static async ValueTask SwalInfo(this IJSRuntime jSRuntime, string message, string title = null)
        {
            await jSRuntime.InvokeVoidAsync("ShowSwal", "info", message, title);
        }

        public static async ValueTask SwalQuestion(this IJSRuntime jSRuntime, string message, string title = null)
        {
            await jSRuntime.InvokeVoidAsync("ShowSwal", "question", message, title);
        }
    }
}
