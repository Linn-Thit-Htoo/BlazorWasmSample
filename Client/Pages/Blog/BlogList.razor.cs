using Client.Models.Entities;
using Client.Shared;
using MudBlazor;
using Newtonsoft.Json;

namespace Client.Pages.Blog
{
    public partial class BlogList
    {
        List<BlogDataModel>? lst;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchData();
            }
        }

        private void Edit(long id)
        {
            Nav.NavigateTo($"/blog/edit/{id}");
        }

        private async Task Delete(long id)
        {
            try
            {
                var parameters = new DialogParameters<ConifrmDialog>
                {
                    {
                        x => x.Message,
                        "Are you sure want to delete?"
                    }
                };

                var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

                var dialog = await DialogService.ShowAsync<ConifrmDialog>("Confirm", parameters, options);
                var result = await dialog.Result;
                if (result.Canceled)
                    return;

                HttpResponseMessage response = await HttpClient.DeleteAsync($"/api/Blog/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    Snackbar.Add(message, Severity.Success);
                    await FetchData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task FetchData()
        {
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync("/api/Blog");
                if (response.IsSuccessStatusCode)
                {
                    string jsonStr = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<BlogDataModel>>(jsonStr)!;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
