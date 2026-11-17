using DmitriyZhilkinKt_41_23.Interfaces.DisciplineInterfaces;
using DmitriyZhilkinKt_41_23.Interfaces.GradeInterfaces;
using DmitriyZhilkinKt_41_23.Interfaces.GroupInterfaces;
using DmitriyZhilkinKt_41_23.Interfaces.SpecialtyInterfaces;
using DmitriyZhilkinKt_41_23.Interfaces.StudentInterfaces;

namespace DmitriyZhilkinKt_41_23.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<IGradeService, GradeService>();

            return services;
        }
    }
}
