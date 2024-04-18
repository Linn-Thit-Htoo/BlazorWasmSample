using Client.Models.Entities;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace Client.Pages.Blog
{
    public partial class CreateBlog
    {
        private BlogDataModel requestModel = new();
        private async Task Save()
        {
            string jsonStr = JsonConvert.SerializeObject(requestModel);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, MediaTypeNames.Application.Json);
            HttpResponseMessage response = await HttpClient.PostAsync("/api/Blog", content);
            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                Snackbar.Add(message, Severity.Success);
            }
            else
            {
                Snackbar.Add("Creating Fail.", Severity.Error);
            }
            Nav.NavigateTo("/");
        }
    }
}
