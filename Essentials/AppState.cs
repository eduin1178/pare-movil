using PARE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAREMAUI.Essentials
{
    public class AppState
    {
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        LoginResponseModel _currentUser;
        public LoginResponseModel CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                _currentUser = value;
                NotifyStateChanged();
            }
        }
    }
}
