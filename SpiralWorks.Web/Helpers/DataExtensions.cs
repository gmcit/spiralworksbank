using SpiralWorks.Interfaces;
using System.Reflection;

namespace SpiralWorks.Web.Helpers
{
    public static class DataExtensions
    {
        public static void CopyTo<T>(this object self, T to)
        {
            if (self != null)
            {
                foreach (var pi in to.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
                {
                    pi.SetValue(to, self.GetType().GetProperty(pi.Name)?.GetValue(self, null) ?? null, null);
                }
            }
        }
        public static string GetAccountNumber(this int self, IUnitOfWork uow)
        {
            var dto = uow.Accounts.FindById(self);
            return dto?.AccountNumber ?? "Unknown Account";
        }
        public static string EmptyOrDefault(this decimal self)
        {
            return (self.ToString().Equals("0.00") ? string.Empty : self.ToString());
        }
    }
}
