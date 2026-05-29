using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Enums
{
    public enum TipoPesoVoto
    {
        Igualitario = 1,    // 1 residente = 1 voto
        PorCoeficiente = 2  // peso según coeficiente del inmueble
    }
}
