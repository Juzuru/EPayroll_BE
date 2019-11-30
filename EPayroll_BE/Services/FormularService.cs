using EPayroll_BE.Models;
using EPayroll_BE.Repositories;
using EPayroll_BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPayroll_BE.Services
{
    public class FormularService : IFormularService
    {
        private readonly IFormularRepository _formularRepository;

        public FormularService(IFormularRepository formularRepository)
        {
            _formularRepository = formularRepository;
        }

        public Guid Add(FormularCreateModel model)
        {
            Formular formular = new Formular
            {
                Description = model.Description
            };

            _formularRepository.Add(formular);
            _formularRepository.SaveChanges();

            return formular.Id;
        }
    }

    public interface IFormularService
    {
        Guid Add(FormularCreateModel model);
    }
}
