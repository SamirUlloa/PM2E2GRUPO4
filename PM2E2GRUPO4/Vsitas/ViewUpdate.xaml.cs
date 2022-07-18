using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM2E2GRUPO4.Modelo;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E2GRUPO4.Vsitas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewUpdate : ContentPage
    {
        public ViewUpdate()
        {
            InitializeComponent();
        }

        public ViewUpdate(Modelo.Musuarios RegItem)
        {
            InitializeComponent();
            txtlatitud.Text = RegItem.latitud;
            txtlongitud.Text = RegItem.logintud;

        }

        private void upFirma_Clicked(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Clicked(object sender, EventArgs e)
        {

        }

        private void btnCordenadas_Clicked(object sender, EventArgs e)
        {

        }
    }
}