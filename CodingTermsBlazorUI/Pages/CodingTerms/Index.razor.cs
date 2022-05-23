using CodingTermsUI.Models.Entities;
using Radzen;
using System.Net.Http.Json;

namespace CodingTermsUI.Pages.CodingTerms
{
    public partial class Index
    {
        protected override async Task OnInitializedAsync()
        {
            terms = await LoadTerms();
        }

        protected async Task UpdateTerm(int id, string title, string description, string keywords, DialogService ds)
        {
            var existingTerm = terms.Where(x => x.Id == id).FirstOrDefault();
            existingTerm.Title = title;
            existingTerm.Description = description;
            existingTerm.Keywords = keywords;
            existingTerm.Modified = DateTime.Now;
            existingTerm.ModifiedBy = "system";
            var status = await Http.PutAsJsonAsync<Term>("update", existingTerm);

            if (status.IsSuccessStatusCode)
            {
                ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Updated Sucessfully", Detail = status.ReasonPhrase, Duration = 4000 });
                terms = await LoadTerms();
            }
            else
            {
                ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "An Error Occurred...", Detail = status.ReasonPhrase, Duration = 4000 });
            }
            ds.Close(true);
        }

        protected async Task AddTerm(string title, string description, string keywords, DialogService ds)
        {
            var newTerm = new Term();

            newTerm.Title = title;
            newTerm.Description = description;
            newTerm.Keywords = keywords;
            newTerm.Created = DateTime.Now;
            newTerm.CreatedBy = "system";
            var status = await Http.PostAsJsonAsync<Term>("create", newTerm);

            if (status.IsSuccessStatusCode)
            {
                ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Success, Summary = "Created Sucessfully", Detail = status.ReasonPhrase, Duration = 4000 });
                terms = await LoadTerms();
            }
            else
            {
                ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "An Error Occurred...", Detail = status.ReasonPhrase, Duration = 4000 });
            }
            ds.Close(true);
        }

        void ShowNotification(NotificationMessage message)
        {
            NotificationService.Notify(message);
        }

        internal async Task<IEnumerable<Term>> LoadTerms()
        {
            _spinnerService.Show();
            var results = await Http.GetFromJsonAsync<IEnumerable<Term>>("list");
            _spinnerService.Hide();
            return results;
        }

    }
}
