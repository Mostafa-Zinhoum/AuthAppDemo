using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService
{
    internal class Service<TEntity,TDto, TGetDto, TGetAllDto, TAddDto, TUpdateDto, TDeleteDto> : IService<TDto, TGetDto, TGetAllDto, TAddDto, TUpdateDto, TDeleteDto>
    {
        //private readonly Repositry<TEntity> repositry;
        public Service()
        {
        }

        public virtual TDto Add(TAddDto dto)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(TDeleteDto dto)
        {
            throw new NotImplementedException();
        }

        public virtual TDto Get(TGetDto dto)
        {
            throw new NotImplementedException();
        }

        public virtual ICollection<TGetDto> GetAll(TGetAllDto dto)
        {
            throw new NotImplementedException();
        }

        public virtual TUpdateDto Update(TUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
