using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E2GRUPO4.Servicios
{
    public class Conexionfirebase
    {
        public static FirebaseClient firebase = new FirebaseClient("https://exameniipn-default-rtdb.firebaseio.com/");
    }
}
