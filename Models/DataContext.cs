using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Inmueble> Inmuebles { get; set; }
        public DbSet<WebApplicationPrueba.Models.Usuario> Usuario { get; set; }
        public DbSet<WebApplicationPrueba.Models.Pago> Pago { get; set; }
        public DbSet<WebApplicationPrueba.Models.Contrato> Contrato { get; set; }
    }
}
