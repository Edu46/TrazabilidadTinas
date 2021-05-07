using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogisticaERP.Clases
{
    public class EBS12_ALMACENES
    {
        public Almacen Almacenes { get; set; }

        public class Almacen
        {
            public string resultado { get; set; }
            public string mensaje { get; set; }
            public List<ItemsAlmacenes> items { get; set; }

            [Serializable()]
            public class ItemsAlmacenes
            {
                public decimal idAlmacen { get; set; }
                public string almacen { get; set; }
                public string codAlmacen { get; set; }
                public string tipoCalculo { get; set; }
                public string centroCosto { get; set; }

            }
        }

        public bool ObtenerAlmacenes(decimal IdEmpresa, decimal TipoCalculo)
        {
            string json = "";
            bool resultado = false;

            EBS12_ALMACENES almacenesEBS12 = new EBS12_ALMACENES();

            try
            {

                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/almacenes/" + IdEmpresa.ToString() + "/" + TipoCalculo.ToString()).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    almacenesEBS12 = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_ALMACENES>(json);
                    Almacenes = almacenesEBS12.Almacenes;

                    if (Almacenes.resultado == "Si")
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


        public bool ObtenerAlmacen(string CodAlmacen)
        {
            string json = "";
            bool resultado = false;

            EBS12_ALMACENES almacenesEBS12 = new EBS12_ALMACENES();

            try
            {

                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/tarifasViajes/almacenes/" + CodAlmacen).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    almacenesEBS12 = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_ALMACENES>(json);
                    Almacenes = almacenesEBS12.Almacenes;

                    if (Almacenes.resultado == "Si")
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

        public bool ObtenerAlmacenesUnidadOperativa(decimal idEmpresaOracle)
        {
            string json;
            bool resultado = false;

            EBS12_ALMACENES almacenesEBS12 = new EBS12_ALMACENES();

            try
            {
                ClaseHttpCliente cliente = new ClaseHttpCliente();
                var response = ClaseHttpCliente.cliente.GetAsync("/cancelacion/pv/ebs12/1.0/empresa/"+ idEmpresaOracle.ToString() +"/almacenes").GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    almacenesEBS12.Almacenes = Newtonsoft.Json.JsonConvert.DeserializeObject<EBS12_ALMACENES.Almacen>(json);
                    Almacenes = almacenesEBS12.Almacenes;

                    if (Almacenes.resultado == "YES")
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