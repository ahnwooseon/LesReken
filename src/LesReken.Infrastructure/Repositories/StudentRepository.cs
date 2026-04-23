using LesReken.Application.Interfaces;
using LesReken.Domain.Models;
using LesReken.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LesReken.Infrastructure.Repositories;

public class StudentRepository(ApplicationDbContext context) : IStudentRepository
{
    public async Task<Student> AddAsync(Student student, CancellationToken ct = default)
    {
        await context.Students.AddAsync(student, ct);
        await context.SaveChangesAsync(ct);
        return student;
    }

    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Students.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyCollection<Student>> GetAllAsync(CancellationToken ct = default) =>
        await context.Students.ToListAsync(ct);

    public async Task<bool> UpdateAsync(Student student, CancellationToken ct = default)
    {
        context.Entry(student).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        Student? student = await context.Students.FindAsync(new object[] { id }, ct);
        if (student is null)
            return false;

        context.Students.Remove(student);
        await context.SaveChangesAsync(ct);
        return true;
    }
}
