using System;
using System.Collections.Generic;
using System.Linq;
using InfHelper.Exceptions;

namespace InfHelper.Models;

public class Category
{
    /// <summary>
    /// Name of the category - retrieved from brackets [CategoryName]
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public List<Key> Keys { get; set; } = [];

    /// <summary>
    /// Check if the Category is named like the given parameter - not case sensitive
    /// </summary>
    /// <param name="name">Compare to given name</param>
    /// <returns></returns>
    public bool IsNamed(string name)
    {
        return string.Compare(Name, name, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public Key this[string id]
    {
        get
        {
            var results = Keys.Where(k => string.Compare(k.Id,id,StringComparison.OrdinalIgnoreCase) == 0);
            
            if (results.Count() > 1)
            {
                throw new MultipleKeysWithSameIdException();
            }

            return results.FirstOrDefault() ??
                throw new KeyNotFoundException();
        }
        set
        {
            int index = -1;
            for (var i = 0; i < Keys.Count; i++)
            {
                if (Keys[i].Id != id)
                {
                    continue;
                }

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