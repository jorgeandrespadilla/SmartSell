﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Shared.Dto
{
    public class SubastaItemDto
    {
        public int ID { get; set; }
        public string UriImagen { get; set; }
        public string NombreProducto { get; set; }
        public float MontoActual { get; set; }
        public DateTime Fecha { get; set; }
        public bool Vigente { get; set; }

        public SubastaItemDto(int id, string uriImagen, string nombreProducto, float montoActual, DateTime fecha, bool vigente)
        {
            ID = id;
            UriImagen = uriImagen;
            NombreProducto = nombreProducto;
            MontoActual = montoActual;
            Fecha = fecha;
            Vigente = vigente;
        }
    }
}
