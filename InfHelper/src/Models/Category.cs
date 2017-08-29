using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;

namespace InfHelper.Models
{
    public class Category
    {
        /// <summary>
        /// Name of the category - retrieved from brackets [CategoryName]
        /// </summary>
        public string Name { get; set; }
        public List<Key> Keys { get; set; } = new List<Key>();

        public Key this[string id]
        {
            get
            {
                var results = Keys.Where(k => string.Compare(k.Id,id,StringComparison.OrdinalIgnoreCase) == 0);
                
                if (results.Count() > 1)
                {
                    throw new MultipleKeysWithSameIdException();
                }
                return results.FirstOrDefault();
            }
            set
            {
                int index = -1;
                for (var i = 0; i < Keys.Count; i++)
                {
                    if (Keys[i].Id != id) continue;
                    //id match found
                    if (index == -1)
                    {
                        index = i;
                    }
                    else
                    {
                        //multiple id match
                        throw new MultipleKeysWithSameIdException();
                    }
                }

                //add new
                if (index == -1)
                {
                    Keys.Add(value);
                }

                //udpate existing
                else
                {
                    Keys[index] = value;
                }
            }
        }
    }
}