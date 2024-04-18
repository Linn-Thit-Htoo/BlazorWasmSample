using Client.Models.Entities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Client.Pages.Blog
{
    public partial class EditBlog
    {
        [Parameter]
        public long id { get; set; }
        public BlogDataModel? blog;

        protected override async Task OnInitializedAsync()
        {
            HttpResponseMessage response = await HttpClient.GetAsync($"/api/Blog/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonStr = await response.Content.ReadAsStringAsync();
                blog = JsonConvert.DeserializeObject<BlogDataModel>(jsonStr)!;
            }
        }
        private async Task Update()
        {
            string jsonStr = JsonConvert.SerializeObject(blog);
            HttpContent content = new StringContent(jsonStr, Encoding.UTF8, Application.Json);
            HttpResponseMessage response = await HttpClient.PutAsync($"/api/Blog/{id}", content);
            var message = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Nav.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(message, MudBlazor.Severity.Error);
            }
        }
    }
}
