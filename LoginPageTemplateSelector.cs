using Microsoft.Maui.Controls;

namespace ReminderApplication
{
    public class LoginPageTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate loginPageTemplate;

        public LoginPageTemplateSelector()
        {
            loginPageTemplate = new DataTemplate(() => new View.LoginPage(new ViewModel.LoginViewModel()));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return loginPageTemplate;
        }
    }
}
