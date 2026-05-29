using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Application.Common.Models
{
    public class Result<T>
    {
        public bool Exitoso { get; private set; }
        public T? Datos { get; private set; }
        public string? Error { get; private set; }
        public IEnumerable<string> Errores { get; private set; } = [];

        private Result() { }

        public static Result<T> Exito(T datos) => new()
        {
            Exitoso = true,
            Datos = datos
        };

        public static Result<T> Fallo(string error) => new()
        {
            Exitoso = false,
            Error = error
        };

        public static Result<T> Fallo(IEnumerable<string> errores) => new()
        {
            Exitoso = false,
            Errores = errores
        };
    }

    public class Result
    {
        public bool Exitoso { get; private set; }
        public string? Error { get; private set; }
        public IEnumerable<string> Errores { get; private set; } = [];

        private Result() { }

        public static Result Exito() => new() { Exitoso = true };

        public static Result Fallo(string error) => new()
        {
            Exitoso = false,
            Error = error
        };

        public static Result Fallo(IEnumerable<string> errores) => new()
        {
            Exitoso = false,
            Errores = errores
        };
    }
}
