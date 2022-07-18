using Firebase.Database.Query;
using Firebase.Storage;
using PM2E2GRUPO4.Modelo;
using PM2E2GRUPO4.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO4.VistasModelo
{
    public class VMusuarios
    {

        List<Musuarios> Usuarios = new List<Musuarios>();
        string rutafoto;
        string Idusuario;

        public async Task<List<Musuarios>> mostrar_usuarios()
        {
            var data = await Conexionfirebase.firebase
                .Child("Usuarios")
                .OrderByKey()
                .OnceAsync<Musuarios>();
            foreach (var rdr in data)
            {
                Musuarios parametros = new Musuarios();
                parametros.Id_usuario = rdr.Key;
                parametros.latitud = rdr.Object.latitud;
                parametros.logintud = rdr.Object.logintud;
                parametros.Icono = rdr.Object.Icono;
              
                Usuarios.Add(parametros);

            }
            return Usuarios;
        }

        public async Task<string> insertar_usuario(Musuarios parametros)
        {
            //child agregar o poder utilizar una tabla y PostAsync es para insertat datos a la tabla

            var data = await Conexionfirebase.firebase
                  .Child("Usuarios")
                  .PostAsync(new Musuarios()
                  {
                      logintud = parametros.logintud,
                      latitud = parametros.latitud,
                      Icono = parametros.Icono,
                      

                  });
            Idusuario = data.Key;
            return Idusuario;
        }

        public async Task<string> SubirImagenesStorage(Stream ImagenStream, string Idusuarios)
        {
            var dataAlmacenamiento = await new FirebaseStorage("exameniipn.appspot.com")
                .Child("Usuarios")
                .Child(Idusuarios + ".jpg")
                .PutAsync(ImagenStream);
            rutafoto = dataAlmacenamiento;
            return rutafoto;
        }



        public async Task EditarFoto(Musuarios parametros)
        {
            var obtenerData = (await Conexionfirebase.firebase
                .Child("Usuarios") //comparamos si es la misma key
                .OnceAsync<Musuarios>()).Where(a => a.Key == parametros.Id_usuario).FirstOrDefault();

            await Conexionfirebase.firebase
                .Child("Usuarios")
                .Child(obtenerData.Key)
                .PutAsync(new Musuarios()
                {
                    logintud = parametros.logintud,
                    latitud = parametros.latitud,
                    Icono = parametros.Icono,
                   

                });
        }

        public async Task EliminarUsuarios(Musuarios parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Usuarios")
                .OnceAsync<Musuarios>()).Where((a) => a.Key == parametros.Id_usuario).FirstOrDefault();
            //eliminar
            await Conexionfirebase.firebase.Child("Usuarios").Child(data.Key).DeleteAsync();
        }
        //eliminar la img
        public async Task EliminarImagen(string nombre)
        {
            await new FirebaseStorage("usuarios-f3e76.appspot.com")
                .Child("Usuarios")
                .Child(nombre)
                .DeleteAsync();
        }


        public async Task<List<Musuarios>> ObtenerDatosUsuarios(Musuarios parametros)
        {
            var data = (await Conexionfirebase.firebase
                .Child("Usuarios")
                .OrderByKey()
                .OnceAsync<Musuarios>()).Where(a => a.Key == parametros.Id_usuario);
            foreach (var rdr in data)
            {
                parametros.logintud = rdr.Object.logintud;
                parametros.latitud = rdr.Object.latitud;
                parametros.Icono = rdr.Object.Icono;
              

                Usuarios.Add(parametros);
            }
            return Usuarios;
        }

    }
}
