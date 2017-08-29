using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;

namespace InfHelper.Models
{
    public class InfData
    {
        public List<Category> Categories { get; set; } = new List<Category>();

        public Category this[string name]
        {
            get
            {
                var results = Categories.Where(k => string.Compare(k.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
                if (results.Count() > 1)
                {
                    throw new MultipleCategoriesWithSameNameException();
                }
                return results.FirstOrDefault();
            }
            set
            {
                int index = -1;
                for (var i = 0; i < Categories.Count; i++)
                {
                    if (Categories[i].Name != name) continue;
                    //id match found
                    if (index == -1)
                    {
                        index = i;
                    }
                    else
                    {
                        //multiple id match
                        throw new MultipleCategoriesWithSameNameException();
                    }
                }

                //add new
                if (index == -1)
                {
                    Categories.Add(value);
                }

                //udpate existing
                else
                {
                    Categories[index] = value;
                }
            }
        }
    }
}