using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases
{
    public class EBS12_VIAJES_PEDIDOS_VENTAS 
    {
        public ViajesEntregas ViajesPedidosVentas { get; set; }
        public ViajeRepresentante ViajesRepresentantes { get; set; }

        public class ViajesEntregas
        {

            public decimal noViaje { get; set; }
            public decimal idViaje { get; set; }
            public decimal idEntrega { get; set; }
            public decimal idAlmacenOrigen { get; set; }
            public string almacenOrigen { get; set; }
            public string codsubInventarioOrigen { get; set; }
            public string codAlmacenOrigen { get; set; }
            public string transportista { get; set; }
            public decimal idCliente { get; set; }
            public decimal idProveedor { get; set; }
            public decimal orderNumber { get; set; }
            public string rutaViaje { get; set; }
            public string resultado { get; set; }
            public string mensaje { get; set; }

        }


        public class ViajeRepresentante
        {
            public decimal noViaje { get; set; }
            public decimal idCategoria { get; set; }
            public List<Items> items { get; set; }
            public string resultado { get; set; }
            public string mensaje { get; set; }

            public class Items 
            {
                public decimal idViaje { get; set; }
                public decimal idEntrega { get; set; }
                public decimal pedidoVenta { get; set; }
                public string ciudad { get; set; }
                public string estado { get; set; }
                public decimal idAlmacen { get; set; }
                public string almacen { get; set; }
                public string cajas { get; set; }
                public decimal totalCajas { get; set; }
                public string transportista { get; set; }
                public string codsubInventarioOrigen { get; set; }
                public string sitio { get; set; }
                public decimal idCiudad { get; set; }
                public decimal idMunicipio { get; set; }
                public decimal costoCaja { get; set; }
                public decimal idTarifaRepresentante { get; set; }

            }

        }

        public bool ObtenerViajesPedidosVentas(Int64 NoViaje)
        {
            string json = "";
            bool resultado = false;

            EBS12_VIAJES_PEDIDOS_VENTAS viajes = new EBS12_VIAJES_PEDIDOS_VENTAS();

            try
            {

                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/viajesPedidosVentas/" + NoViaje.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    viajes = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_VIAJES_PEDIDOS_VENTAS>(json);
                    ViajesPedidosVentas = viajes.ViajesPedidosVentas;

                    if (ViajesPedidosVentas.resultado == "Si")
                        resultado = true;
                }
                else
                    throw new Exception(response.ReasonPhrase);

                return resultado;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        public bool ObtenerViajesRepresentantes(Int64 NoViaje, Int64 IdCategoria)
        {
            string json = "";
            bool resultado = false;

            EBS12_VIAJES_PEDIDOS_VENTAS viajes = new EBS12_VIAJES_PEDIDOS_VENTAS();

            try
            {

                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/viajesPedidosVentas/representantes/" + NoViaje.ToString() + "/" + IdCategoria.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    viajes = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_VIAJES_PEDIDOS_VENTAS>(json);
                    ViajesRepresentantes = viajes.ViajesRepresentantes;

                    if (ViajesRepresentantes.resultado == "Si")
                        resultado = true;
                }
                else
                    throw new Exception(response.ReasonPhrase);

                return resultado;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        } 
    }
}