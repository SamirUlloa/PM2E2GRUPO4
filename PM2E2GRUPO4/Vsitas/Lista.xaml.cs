using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using PM2E2GRUPO4.VistasModelo;
using Map = Xamarin.Essentials.Map;
using PM2E2GRUPO4.Modelo;

namespace PM2E2GRUPO4.Vsitas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lista : ContentPage
    {
        string Idusuario;
        string EstadoImagen;
        public Lista()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            VMusuarios datos = new VMusuarios();
            var lista = await datos.mostrar_usuarios();
            lstFirmas.ItemsSource = lista;

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

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {

        }

        private void SwipeItem_Invoked_1(object sender, EventArgs e)
        {

        }

        private async void SwipeItem_Invoked_2(object sender, EventArgs e)
        {
            //Rutas para el rastreo de la dirección
            var location = new Location(16.351835, -86.464745);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

            await Map.OpenAsync(location, options);
        }

        private async void SwipeItem_Invoked_3(object sender, EventArgs e)
        {


            ///////////////////////////////////////////////////////////////////////////////////

            double lat = 16.351362;
            double lon = -86.455821;

            Position position = new Position(lat, lon);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map(mapSpan);

            Pin pin = new Pin
            {
                Label = "Roatán",
                Address = "French Harbour",
                Type = PinType.Place,
                Position = position
            };
            map.Pins.Add(pin);

            Content = map;
        }

        private async void lstFirmas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var usuario = (Musuarios)e.SelectedItem;
            Idusuario = usuario.Id_usuario;
            double lat = double.Parse(usuario.latitud);
            double lon = double.Parse(usuario.logintud);
           

            String rest = await DisplayActionSheet("Acciones",null,null, "Ubicacion", "¿Como llegar?","Update","Eliminar");
            if(rest == "Ubicacion")
            {
                Position position = new Position(lat, lon);
                MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
                Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map(mapSpan);

                Pin pin = new Pin
                {
                    Label = "Honduras",
                    Address = "Honduras",
                    Type = PinType.Place,
                    Position = position
                };
                map.Pins.Add(pin);

                Content = map;
            }else if (rest == "¿Como llegar?")
            {
                var location = new Location(lat, lon);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

                await Map.OpenAsync(location, options);

            }else if (rest == "Update") 
            {
                await Navigation.PushAsync(new ViewUpdate(usuario));

            }else if (rest == "Eliminar")
            {
                await EliminarUsuario();
                await EliminarImagenUsuario();

            }

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
            try
            {
                await funcion.EliminarImagen(Idusuario + ".jpg");
            }
            catch (Exception e){
            }
        }

    }
}