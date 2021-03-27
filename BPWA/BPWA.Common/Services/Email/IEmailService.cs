using System.Threading.Tasks;

namespace BPWA.Common.Services
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string body);
    }
}
