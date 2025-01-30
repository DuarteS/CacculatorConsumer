using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalculatorApi.Data;
using CalculatorApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatorApi.Repositories
{
    public class CalculationRepository : IRepository<Calculation>
    {
        private readonly AppDbContext _context;

        public CalculationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Calculation calculation)
        {
            await _context.Calculations.AddAsync(calculation);
            await _context.SaveChangesAsync();
        }
    }
}