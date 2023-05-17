using System;

namespace RestourantDesktop.DialogManager.Dialogs.AuthorizeDialog.Pages
{
    static class Auth
    {
        public static event EventHandler<bool> authEvent;

        public static void SendAuth(bool result)
        {
            authEvent?.Invoke(null, result);
        }
    }
}
