using System;
using System.Collections.Generic;
using System.Text;

namespace VotacionesResidenciales.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> GuardarCambiosAsync(CancellationToken ct = default);
    }
}
