﻿using ContactsManager.Domaine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Persistence.Repository
{
    public interface IContactRepository: IGenericRepository<Contact>
    {
    }
}
