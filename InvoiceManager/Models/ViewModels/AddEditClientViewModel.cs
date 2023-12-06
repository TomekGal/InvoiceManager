using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InvoiceManager.Models.Domains;

namespace InvoiceManager.Models.ViewModels
{
    public class AddEditClientViewModel
    {
        public  Client Client { get; set; }
        public Address Address { get; set; }

        public int AdressId { get; set; }

        //public int UserId { get; set; }
    }
}