using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetMind.Areas.Admin.Models;

namespace NetMind.Areas.Admin.Controllers
{

    public enum SortTo {
        First, Prev, Next, Last
    }

    public class SortOrderHelper<T> where T : Ordable
    {
        public static async Task SortOrder(DbContext context, DbSet<T> dbset, SortTo sortto, T item)
        {
            await Task.Run(async () => {
                List<T> tmpList = null;

                switch (sortto)
                {
                    // Moving to the first position 
                    case SortTo.First:
                        tmpList = await dbset.Where(p => p.Order < item.Order).OrderBy(p => p.Order).ToListAsync();
                        tmpList.Insert(0, item);
                        int i = 0;
                        foreach (T _item in tmpList)
                        {
                            _item.Order = ++i;
                            dbset.Update(_item);
                        }
                        await context.SaveChangesAsync();
                        break;
                    // Moving one position up
                    case SortTo.Prev:
                        var lastItem = await dbset.Where(p => p.Order < item.Order).OrderBy(p => p.Order).LastOrDefaultAsync();
                        if (lastItem is not null)
                        {
                            i = lastItem.Order;
                            lastItem.Order = item.Order;
                            item.Order = i;
                            dbset.Update(lastItem);
                            dbset.Update(item);
                            await context.SaveChangesAsync();
                        }
                        break;
                    // Moving one position down
                    case SortTo.Next:
                        var nextItem = await dbset.Where(p => p.Order > item.Order).OrderBy(p => p.Order).FirstOrDefaultAsync();
                        if (nextItem is not null)
                        {
                            i = nextItem.Order;
                            nextItem.Order = item.Order;
                            item.Order = i;
                            dbset.Update(item);
                            dbset.Update(nextItem);
                            await context.SaveChangesAsync();
                        }
                        break;
                    // Moving to the last position
                    default:
                        tmpList = await dbset.Where(p => p.Order > item.Order).OrderBy(p => p.Order).ToListAsync();
                        tmpList.Add(item);
                        int j = item.Order;
                        foreach (T _item in tmpList)
                        {
                            _item.Order = j++;
                            dbset.Update(_item);
                        }
                        await context.SaveChangesAsync();
                        break;
                }
            });
        }
    }
}
