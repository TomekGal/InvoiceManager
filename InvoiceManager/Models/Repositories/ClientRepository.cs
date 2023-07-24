using InvoiceManager.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;


namespace InvoiceManager.Models.Repositories
{
    public class ClientRepository
    {
       
        public List<Client> GetClients(string userId)
        {
           using(var context = new ApplicationDbContext())
            {
                return context.Clients.Where(x => x.UserId == userId).ToList();
            }
        }

        public Address GetAdress(int addressId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Addresses.SingleOrDefault(x => x.Id == addressId);
            }
           
        }

        public void Add(Client client)
        {
            using (var context = new ApplicationDbContext())
            {
               
                context.Clients.Add(client);
                context.SaveChanges();
            }
        }

        public Client GetClient(int id)
        {

            using (var context = new ApplicationDbContext())
            {
               return context.Clients.SingleOrDefault(x => x.Id == id);

            }
        }

        public void Update(Client client)
        {

        }

        public void Delete(int id)
        {
           using(var context = new ApplicationDbContext())
            {
              var clientToDelete=  context.Clients.SingleOrDefault(x => x.Id == id);
                context.Clients.Remove(clientToDelete);
                context.SaveChanges();
            }
        }
    }
}