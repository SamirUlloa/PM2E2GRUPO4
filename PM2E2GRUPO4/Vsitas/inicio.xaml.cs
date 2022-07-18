using Plugin.Media;
using Plugin.Media.Abstractions;
using PM2E2GRUPO4.Modelo;
using PM2E2GRUPO4.VistasModelo;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM2E2GRUPO4.Vsitas;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Xamarin.Essentials;

namespace PM2E2GRUPO4.Vsitas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class inicio : ContentPage
    {
        public inicio()
        {
            InitializeComponent();
            obtenerUbicacion();
        }

        Stream image_;
        string rutafoto;
        string Idusuario;
        string estado;
        string EstadoImagen;
        double longi;
        double lati;
        

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtlatitud.Text) || !string.IsNullOrEmpty(txtlongitud.Text))
            {
                await InsertarUsuarios();
                await SubirImagenesStore();
                await EditarFoto();
            }
            else
            {

                await DisplayAlert("Campos Vacios", "LLenar los campos", "Ok");

            }
        }

        private async Task EditarFoto()
        {
            VMusuarios funcion = new VMusuarios();
            Musuarios parametros = new Musuarios();

            parametros.latitud = txtlatitud.Text;
            parametros.logintud = txtlongitud.Text;
            parametros.Icono = rutafoto;
            parametros.Id_usuario = Idusuario;

            await funcion.EditarFoto(parametros);
            await DisplayAlert("Listo", "Usuario Agregado", "OK");
            await mostrarUsuarios();
            limpiar();

        }
        private async Task InsertarUsuarios()
        {

            VMusuarios funcion = new VMusuarios();

            Musuarios parametros = new Musuarios();

            parametros.latitud = txtlatitud.Text;
            parametros.logintud = txtlongitud.Text;
            parametros.Icono = "-";    
            Idusuario = await funcion.insertar_usuario(parametros);

            image_ = await imagenCelular.GetImageStreamAsync(SignatureImageFormat.Jpeg);


        }

        private async Task SubirImagenesStore()
        {
            VMusuarios funcion = new VMusuarios();
            rutafoto = await funcion.SubirImagenesStorage(image_, Idusuario);
        }


        private async void limpiar()
        {
            txtlatitud.Text = "";
            txtlongitud.Text = "";
            imagenCelular.Clear();
        }






        private async Task mostrarUsuarios()
        {
            VMusuarios funcion = new VMusuarios();
            var dt1 = await funcion.mostrar_usuarios();
            //listaUsuarios.ItemsSource = dt1;
        }



        private async void btnIcono_Clicked(object sender, EventArgs e)
        {
            Idusuario = (sender as ImageButton).CommandParameter.ToString();
            //await ObtenerDatoUsuario();
        }

        public async void obtenerUbicacion()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 100;
            if (locator.IsGeolocationAvailable)
            {
                if (locator.IsGeolocationEnabled)
                {
                    if (!locator.IsListening)
                    {
                        await locator.StartListeningAsync(TimeSpan.FromSeconds(1), 5);
                    }
                    locator.PositionChanged += (cambio, args) =>
                    {
                        var loc = args.Position;
                        txtlongitud.Text = loc.Longitude.ToString();
                        longi = double.Parse(txtlongitud.Text);
                        txtlatitud.Text = loc.Latitude.ToString();
                        lati = double.Parse(txtlatitud.Text);
                    };
                }
            }
        }

     

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            await EliminarUsuario();
            await EliminarImagenUsuario();
            await mostrarUsuarios();
        }

        private async Task EliminarUsuario()
        {
            VMusuarios funcion = new VMusuarios();
            Musuarios parametros = new Musuarios();
            parametros.Id_usuario = Idusuario;
            await funcion.EliminarUsuarios(parametros);
        }

        private async Task EliminarImagenUsuario()
        {
            VMusuarios funcion = new VMusuarios();
            await funcion.EliminarImagen(Idusuario + ".jpg");
        }

        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            if (EstadoImagen == "LLENO")
            {
                await EliminarImagenUsuario();
                await SubirImagenesStore();
            }
            await EditarFoto();
        }

        private async void btnLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Lista());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
 

            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    Title = "Active la Localización GPS";
                }
                else
                {
                    Title = "GPS esta Activado";
                }
            }
            catch (FeatureNotSupportedException)
            {
            }
            catch (FeatureNotEnabledException)
            {
            }
            catch (PermissionException)
            {
            }
            catch (System.Exception)
            {
            }
        }
    }
}